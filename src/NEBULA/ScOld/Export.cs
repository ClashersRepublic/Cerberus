using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BL.Assets.Editor.ScOld
{
    public class Export : ScObject
    {
        #region Constructors
        public Export(ScFile scFile)
        {
            _scFile = scFile;
        }
        #endregion

        #region Fields & Properties
        private ushort _exportId;
        private string _exportName;
        private MovieClip _dataObject;
        private ScFile _scFile;

        public override ushort Id => _exportId;
        public override List<ScObject> Children => _dataObject.Children;
        #endregion

        #region Methods
        public override ScObject GetDataObject()
        {
            return _dataObject;
        }

        public override int GetDataType()
        {
            return 7;
        }

        public override string GetDataTypeName()
        {
            return "Exports";
        }

        public override string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ExportId: " + _exportId);
            return sb.ToString();
        }

        public override string GetName()
        {
            return _exportName;
        }

        public override void Rename(string name) => _exportName = name;

        public override void Write(FileStream input)
        {
            input.Seek(0, SeekOrigin.Begin);
            byte[] file = new byte[input.Length];
            input.Read(file, 0, file.Length);

            int cursor = (int)_scFile.GetStartExportsOffset();

            input.Seek(_scFile.GetStartExportsOffset(), SeekOrigin.Begin);

            ushort exportCount = BitConverter.ToUInt16(file, cursor);
            input.Write(BitConverter.GetBytes((ushort)(exportCount + 1)), 0, 2);
            cursor += 2;

            input.Seek(exportCount * 2, SeekOrigin.Current);
            cursor += exportCount * 2;
            input.Write(BitConverter.GetBytes(_exportId), 0, 2);

            for (int i = 0; i < exportCount; i++)
            {
                byte nameLength = file[cursor];
                cursor++;
                byte[] exportName = new byte[nameLength];
                Array.Copy(file, cursor, exportName, 0, nameLength);
                input.WriteByte(nameLength);
                input.Write(exportName, 0, nameLength);
                cursor += nameLength;
            }

            input.WriteByte((byte)_exportName.Length);
            input.Write(Encoding.UTF8.GetBytes(_exportName), 0, (byte)_exportName.Length);

            while (cursor < file.Length)
            {
                input.WriteByte(file[cursor]);
                cursor++;
            }

            //refresh all offsets
            foreach (Texture t in _scFile.GetTextures())
            {
                long offset = t.GetOffset();
                if (offset > 0)
                    offset += 2 + 1 + _exportName.Length;
                else
                    offset = offset - 2 - 1 - _exportName.Length;
                t.SetOffset(offset);
            }
            foreach (Shape s in _scFile.GetShapes())
            {
                long offset = s.GetOffset();
                if (offset > 0)
                    offset += 2 + 1 + _exportName.Length;
                else
                    offset = offset - 2 - 1 - _exportName.Length;
                s.SetOffset(offset);
                foreach (ShapeChunk sc in s.GetChunks())
                {
                    long chunkOffset = sc.GetOffset();
                    if (chunkOffset > 0)
                        chunkOffset += 2 + 1 + _exportName.Length;
                    else
                        chunkOffset = chunkOffset - 2 - 1 - _exportName.Length;
                    sc.SetOffset(chunkOffset);
                }
            }
            foreach (MovieClip mc in _scFile.GetMovieClips())
            {
                long offset = mc.GetOffset();
                if (offset > 0)
                    offset += 2 + 1 + _exportName.Length;
                else
                    offset = offset - 2 - 1 - _exportName.Length;
                mc.SetOffset(offset);
            }
            _scFile.SetEofOffset(_scFile.GetEofOffset() + 2 + 1 + _exportName.Length);
            //ne pas oublier eofoffset
        }

        public void SetDataObject(MovieClip sd)
        {
            _dataObject = sd;
        }

        public void SetId(ushort id)
        {
            _exportId = id;
        }

        public void SetExportName(string name)
        {
            _exportName = name;
        }
        #endregion
    }
}
