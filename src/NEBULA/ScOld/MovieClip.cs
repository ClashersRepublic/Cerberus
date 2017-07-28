using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CR.Assets.Editor.ScOld
{
    public sealed class MovieClip : ScObject
    {
        #region Constructors

        public MovieClip(ScFile scs, short dataType)
        {
            _scFile = scs;
            _dataType = dataType;
            _shapes = new List<ScObject>();
        }

        public MovieClip(MovieClip mv)
        {
            _scFile = mv.GetStorageObject();
            _dataType = mv.GetMovieClipDataType();
            _shapes = new List<ScObject>();

            this.SetOffset(-Math.Abs(mv.GetOffset()));

            //Duplicate MovieClip
            using (FileStream input = new FileStream(_scFile.GetInfoFileName(), FileMode.Open))
            {
                input.Seek(Math.Abs(mv.GetOffset()), SeekOrigin.Begin);
                using (var br = new BinaryReader(input))
                {

                    var packetId = br.ReadByte().ToString("X2");
                    var packetSize = br.ReadUInt32();
                    this.Read(br, packetId);
                }
            }

            //Set new clip id
            ushort maxMovieClipId = this.Id;
            foreach (MovieClip clip in _scFile.GetMovieClips())
            {
                if (clip.Id > maxMovieClipId)
                    maxMovieClipId = clip.Id;
            }
            maxMovieClipId++;
            this.SetId(maxMovieClipId);

            //Get max shape id
            ushort maxShapeId = 30000; //avoid collision with other objects in MovieClips
            foreach (Shape shape in _scFile.GetShapes())
            {
                if (shape.Id > maxShapeId)
                    maxShapeId = shape.Id;
            }
            maxShapeId++;

            //Duplicate shapes associated to clip
            List<ScObject> newShapes = new List<ScObject>();
            foreach (Shape s in _shapes)
            {
                Shape newShape = new Shape(s);
                newShape.SetId(maxShapeId);
                maxShapeId++;
                newShapes.Add(newShape);

                _scFile.AddShape(newShape); //Add to global shapelist
                _scFile.AddChange(newShape);
            }
            this._shapes = newShapes;
        }

        #endregion

        #region Fields & Properties

        private short _dataType;
        private ushort _clipId;
        private short _frameCount;
        private List<ScObject> _shapes;
        private ScFile _scFile;
        private long _offset;

        public override ushort Id => _clipId;
        public override List<ScObject> Children => _shapes;

        #endregion

        #region Methods

        public override int GetDataType()
        {
            return 1;
        }

        public override string GetDataTypeName()
        {
            return "MovieClips";
        }

        public short GetMovieClipDataType()
        {
            return _dataType;
        }

        public long GetOffset()
        {
            return _offset;
        }

        public List<ScObject> GetShapes()
        {
            return _shapes;
        }

        public ScFile GetStorageObject()
        {
            return _scFile;
        }

        public string findExportnames(uint clipID)
        {
            var exportResult = "";
            //exportNames[exportIDs.IndexOf(clipID)]
            var result = Enumerable.Range(0, _scFile.GetExports().Count).Where(i => _scFile.GetExports()[i].Id == clipID).ToList();
            bool isfirst = true;
            foreach (var item in result)
            {
                if (!isfirst)
                    exportResult += "-";
                else
                    isfirst = false;
                exportResult += _scFile.GetExports()[item].GetName();
            }
            return exportResult;
        }

        public override void Read(BinaryReader br, string packetId)
        {
            //Console.WriteLine(@"MovieClip data type: " + _dataType);
            /*StringBuilder hex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
                hex.AppendFormat("{0:x2}", b);
            Console.WriteLine(hex.ToString());*/

            _clipId = br.ReadUInt16();
            br.ReadByte(); //a1 + 34
            _frameCount = br.ReadInt16();
            

            //Console.WriteLine("ClipID: " + _clipId + " -> ExportName: " + exportName);
            int cnt1 = br.ReadInt32();
            ushort[] sa1 = new ushort[cnt1 * 3];
            for (int i = 0; i < cnt1; i++)
            {
                sa1[3 * i] = br.ReadUInt16();
                sa1[3 * i + 1] = br.ReadUInt16();
                sa1[3 * i + 2] = br.ReadUInt16();
            }


            int cnt2 = br.ReadInt16();
            ushort[] sa2 = new ushort[cnt2]; //a1 + 8
            for (int i = 0; i < cnt2; i++)
            {
                sa2[i] = br.ReadUInt16();
                int index = _scFile.GetShapes().FindIndex(shape => shape.Id == sa2[i]);
                if (index != -1)
                    _shapes.Add(_scFile.GetShapes()[index]);;
            }

            for (int i = 0; i < cnt2; i++)
            {
                var cnt2Opacity = br.ReadByte(); //Opacity or blend mode? 
            }

            //read ascii
            for (int i = 0; i < cnt2; i++)
            {
                byte stringLength = br.ReadByte();
                if (stringLength < 255)
                    br.ReadBytes(stringLength);
            }

            while (true)
            {
                byte v26;
                while (true)
                {
                    int lenght;
                    while (true)
                    {
                        v26 = br.ReadByte();
                        lenght = br.ReadInt32();
                        if (v26 != 5)
                            break;
                    }
                    if (v26 == 11)
                    {
                        short frameId = br.ReadInt16();
                        byte frameNameLength = br.ReadByte();
                        if (frameNameLength < 255)
                        {
                            var unk2 = Encoding.UTF8.GetString(br.ReadBytes(frameNameLength));
                            //Console.WriteLine("\t frameid : "+ frameId + " -> unk2: " + unk2);
                        }
                    }
                    else if (v26 == 31)
                    {
                        var type31 = br.ReadBytes(lenght);
                        //Console.WriteLine("Type 31 " + BitConverter.ToString(type31));
                    }
                    else
                        break;
                }
                if (v26 == 0)
                    break;

               // Console.WriteLine("Left " + BitConverter.ToString(br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position))));
                Console.WriteLine("Unknown tag " + v26.ToString());
                //break;
            }
        }

        public override void Write(FileStream input)
        {
            if (_offset < 0) //new
            {
                using (
                    FileStream readInput = new FileStream(_scFile.GetInfoFileName(), FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite))
                {
                    //Positionnement des curseurs
                    readInput.Seek(Math.Abs(_offset), SeekOrigin.Begin);
                    input.Seek(_scFile.GetEofOffset(), SeekOrigin.Begin);

                    //type and length
                    byte[] dataType = new byte[1];
                    readInput.Read(dataType, 0, 1);
                    byte[] dataLength = new byte[4];
                    readInput.Read(dataLength, 0, 4);
                    input.Write(dataType, 0, 1);
                    input.Write(dataLength, 0, 4);

                    //movieclip
                    readInput.Seek(2, SeekOrigin.Current);
                    input.Write(BitConverter.GetBytes(_clipId), 0, 2);

                    input.WriteByte((byte) readInput.ReadByte());
                    readInput.Seek(2, SeekOrigin.Current);
                    input.Write(BitConverter.GetBytes(_frameCount), 0, 2);

                    //int cnt1 = br.ReadInt32();
                    byte[] cnt1 = new byte[4];
                    readInput.Read(cnt1, 0, 4);
                    input.Write(cnt1, 0, 4);

                    for (int i = 0; i < BitConverter.ToInt32(cnt1, 0); i++)
                    {
                        byte[] uk1 = new byte[2];
                        readInput.Read(uk1, 0, 2);
                        input.Write(uk1, 0, 2);

                        byte[] uk2 = new byte[2];
                        readInput.Read(uk2, 0, 2);
                        input.Write(uk2, 0, 2);

                        byte[] uk3 = new byte[2];
                        readInput.Read(uk3, 0, 2);
                        input.Write(uk3, 0, 2);
                    }

                    //int cnt2 = br.ReadInt16();
                    byte[] cnt2 = new byte[2];
                    readInput.Read(cnt2, 0, 2);
                    input.Write(cnt2, 0, 2);

                    int cptShape = 0;
                    for (int i = 0; i < BitConverter.ToInt16(cnt2, 0); i++)
                    {
                        byte[] id = new byte[2];
                        readInput.Read(id, 0, 2);

                        int index = _scFile.GetShapes().FindIndex(shape => shape.Id == BitConverter.ToInt16(id, 0));
                        if (index != -1)
                        {
                            input.Write(BitConverter.GetBytes(_shapes[cptShape].Id), 0, 2);
                            cptShape++;
                        }
                        else
                        {
                            input.Write(id, 0, 2);
                        }
                    }
                    for (int i = 0; i < BitConverter.ToInt16(cnt2, 0); i++)
                    {
                        input.WriteByte((byte) readInput.ReadByte());
                    }


                    //read ascii
                    for (int i = 0; i < BitConverter.ToInt16(cnt2, 0); i++)
                    {
                        byte stringLength = (byte) readInput.ReadByte();
                        input.WriteByte(stringLength);
                        if (stringLength < 255)
                        {
                            for (int j = 0; j < stringLength; j++)
                                input.WriteByte((byte) readInput.ReadByte());
                        }
                    }

                    byte[] lenght = new byte[4];
                    while (true)
                    {
                        byte v26;
                        while (true)
                        {
                            while (true)
                            {
                                v26 = (byte) readInput.ReadByte();
                                input.WriteByte(v26);

                                //br.ReadInt32();
                                readInput.Read(lenght, 0, 4);
                                input.Write(lenght, 0, 4);

                                if (v26 != 5)
                                    break;
                            }
                            if (v26 == 11)
                            {
                                //short frameId = br.ReadInt16();
                                byte[] frameId = new byte[2];
                                readInput.Read(frameId, 0, 2);
                                input.Write(frameId, 0, 2);

                                byte frameNameLength = (byte) readInput.ReadByte();
                                input.WriteByte(frameNameLength);

                                if (frameNameLength < 255)
                                {
                                    for (int i = 0; i < frameNameLength; i++)
                                    {
                                        input.WriteByte((byte) readInput.ReadByte());
                                    }
                                }
                            }
                            else if (v26 == 31)
                            {
                                int l = Convert.ToInt32(lenght);
                                byte[] data = new byte[l];
                                readInput.Read(data, 0, l);
                                input.Write(data, 0, l);
                            }
                            else
                                break;
                        }
                        if (v26 == 0)
                            break;
                        Console.WriteLine("Unknown tag " + v26);
                    }
                }
                _offset = _scFile.GetEofOffset();
                _scFile.SetEofOffset(input.Position);
                input.Write(new byte[] {0, 0, 0, 0, 0}, 0, 5);
            }
        }

        public void SetId(ushort id)
        {
            _clipId = id;
        }

        public void SetOffset(long position)
        {
            _offset = position;
        }

        #endregion
    }
}