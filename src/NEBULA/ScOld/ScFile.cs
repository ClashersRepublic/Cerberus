using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.IO;
using System.Windows.Forms;
using CR.Assets.Editor.Compression;

namespace CR.Assets.Editor.ScOld
{
    public class ScFile
    {

        #region Constructors

        public ScFile(string infoFile, string textureFile)
        {
            _textures = new List<ScObject>();
            _shapes = new List<ScObject>();
            _exports = new List<ScObject>();
            _movieClips = new List<ScObject>();
            _pendingChanges = new List<ScObject>();
            _matrixs = new List<Matrix>();
            _colors = new List<Tuple<Color, byte, Color>>();
            _infoFile = infoFile;
            _textureFile = textureFile;
        }

        #endregion

        #region Fields & Properties

        private ushort _exportCount;
        private readonly List<ScObject> _textures;
        private readonly List<ScObject> _shapes;
        private readonly List<ScObject> _exports;
        private readonly List<Matrix> _matrixs;
        private readonly List<Tuple<Color, byte, Color>> _colors;
        private readonly List<ScObject> _movieClips;
        private readonly List<ScObject> _pendingChanges;

        private readonly string _infoFile;
        private readonly string _textureFile;
        private long _eofOffset;
        private long _eofTexOffset;
        private long _exportStartOffset;

        #endregion

        public void AddChange(ScObject change)
        {
            if (_pendingChanges.IndexOf(change) == -1)
                _pendingChanges.Add(change);
        }

        public void AddExport(Export export)
        {
            _exports.Add(export);
        }

        public void AddShape(Shape shape)
        {
            _shapes.Add(shape);
        }

        public void AddTexture(Texture texture)
        {
            _textures.Add(texture);
        }

        public void AddMovieClip(MovieClip movieClip)
        {
            _movieClips.Add(movieClip);
        }

        public long GetEofOffset()
        {
            return _eofOffset;
        }

        public long GetEofTexOffset()
        {
            return _eofTexOffset;
        }

        public List<ScObject> GetExports()
        {
            return _exports;
        }

        public string GetInfoFileName()
        {
            return _infoFile;
        }

        public string GetTextureFileName()
        {
            return _textureFile;
        }

        public List<ScObject> GetMovieClips()
        {
            return _movieClips;
        }

        public List<Matrix> GetMatrixs()
        {
            return _matrixs;
        }

        public List<ScObject> GetShapes()
        {
            return _shapes;
        }

        public long GetStartExportsOffset()
        {
            return _exportStartOffset;
        }

        public List<ScObject> GetTextures()
        {
            return _textures;
        }

        public void SetEofOffset(long offset)
        {
            _eofOffset = offset;
        }


        public void SetEofTexOffset(long offset)
        {
            _eofTexOffset = offset;
        }

        public void SetStartExportsOffset(long offset)
        {
            _exportStartOffset = offset;
        }

        public void Save(FileStream input, FileStream texinput)
        {
            // Flushing depending edits.
            List<ScObject> exports = new List<ScObject>();
            foreach (ScObject data in _pendingChanges)
            {
                switch (data.GetDataType())
                {
                    case 7:
                        exports.Add(data);
                        break;
                    case 2:
                        data.Write(texinput);
                        break;
                    default:
                        data.Write(input);
                        break;
                }
            }
            _pendingChanges.Clear();

            if (exports.Count > 0)
            {
                foreach (ScObject data in exports)
                {
                    data.Write(input);
                }
            }

            // Saving metadata/header.
            input.Seek(0, SeekOrigin.Begin);
            input.Write(BitConverter.GetBytes((ushort)_shapes.Count), 0, 2);
            input.Write(BitConverter.GetBytes((ushort)_movieClips.Count), 0, 2);
            input.Write(BitConverter.GetBytes((ushort)_textures.Count), 0, 2);
        }

        public void Load()
        {
            var sw = Stopwatch.StartNew();
            while (true)
            {
                
                using (var texReader = new BinaryReader(File.OpenRead(_textureFile)))
                {
                    Byte[] IsCompressed = texReader.ReadBytes(2);
                    if (BitConverter.ToString(IsCompressed) == "53-43")
                    {
                        texReader.BaseStream.Seek(26, SeekOrigin.Begin);
                        DialogResult result =
                            MessageBox.Show(
                                "The tool detected that you have load a compressed file.\nWould you like to decompress and load it?\nPlease note,Choosing to decompressed will override the compressed file with a new one",
                                "SC File Is Compresed", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.Yes)
                        {
                            string IsSc = BitConverter.ToString(texReader.ReadBytes(2));
                            texReader.Close();
                            if (IsSc == "53-43")
                                Lzham.DecompressSc(_textureFile);
                            else
                                Lzma.DecompressCR(_textureFile);
                            continue;
                        }
                        break;
                    }
                    else if (BitConverter.ToString(IsCompressed) == "5D-00")
                    {
                        DialogResult result =  MessageBox.Show("The tool detected that you have load a compressed file.\nWould you like to decompress and load it?\nPlease note,Choosing to decompressed will override the compressed file with a new one", "SC File Is Compresed", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                        if (result == DialogResult.Yes)
                        {
                            texReader.Close();
                            Lzma.DecompressCoC(_textureFile);
                            continue;
                        }
                        break;
                    }
                    texReader.BaseStream.Seek(0, SeekOrigin.Begin);
                    while (true)
                    {

                        long texoffset = texReader.BaseStream.Position;
                        var packetId = texReader.ReadByte();
                        var packetSize = texReader.ReadUInt32();
                        if (packetSize > 0)
                        {
                            var tex = new Texture(this);
                            tex.SetOffset(texoffset);
                            tex.Read(packetId, packetSize, texReader);
                            this._textures.Add(tex);
                            if (texReader.BaseStream.Position != texReader.BaseStream.Length)
                            {
                                continue;
                            }
                        }
                        _eofTexOffset = texoffset;
                        break;
                    }
                }
                break;
            }

            while (true)
            {
                using (var reader = new BinaryReader(File.OpenRead(_infoFile)))
                {
                    Byte[] IsCompressed = reader.ReadBytes(2);
                    if (BitConverter.ToString(IsCompressed) == "53-43")
                    {
                        reader.BaseStream.Seek(26, SeekOrigin.Begin);
                        DialogResult result =
                            MessageBox.Show(
                                "The tool detected that you have load a compressed file.\nWould you like to decompress and load it?\nPlease note,Choosing to decompressed will override the compressed file with a new one",
                                @"SC File Is Compresed", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.Yes)
                        {
                            string IsSc = BitConverter.ToString(reader.ReadBytes(2));
                            reader.Close();
                            if (IsSc == "53-43")
                                Lzham.DecompressSc(_infoFile);
                            else
                                Lzma.DecompressCR(_infoFile);
                            continue;
                        }
                        break;
                    }
                    else if (BitConverter.ToString(IsCompressed) == "5D-00")
                    {
                        DialogResult result = MessageBox.Show("The tool detected that you have load a compressed file.\nWould you like to decompress and load it?\nPlease note,Choosing to decompressed will override the compressed file with a new one", "SC File Is Compresed", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                        if (result == DialogResult.Yes)
                        {
                            reader.Close();
                            Lzma.DecompressCoC(_infoFile);
                            continue;
                        }
                        break;
                    }

                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    var shapeCount = reader.ReadUInt16(); // a1 + 8
                    var movieClipCount = reader.ReadUInt16(); // a1 + 12
                    var textureCount = reader.ReadUInt16(); // a1 + 16
                    var textFieldCount = reader.ReadUInt16(); // a1 + 24
                    var matrixCount = reader.ReadUInt16(); // a1 + 28
                    var colorTransformCount = reader.ReadUInt16(); // a1 + 32

                    int colorTransfromID = 0;

#if DEBUG
                    Console.WriteLine(@"ShapeCount: " + shapeCount);
                    Console.WriteLine(@"MovieClipCount: " + movieClipCount);
                    Console.WriteLine(@"TextureCount: " + textureCount);
                    Console.WriteLine(@"TextFieldCount: " + textFieldCount);
                    Console.WriteLine(@"Matrix2x3Count: " + matrixCount);
                    Console.WriteLine(@"ColorTransformCount: " + colorTransformCount);
#endif
                    // 5 useless bytes, not even used by Supercell
                    reader.ReadByte(); // 1 octet
                    reader.ReadUInt16(); // 2 octets
                    reader.ReadUInt16(); // 2 octets

                    _exportStartOffset = reader.BaseStream.Position;
                    _exportCount = reader.ReadUInt16(); // a1 + 20
                    Console.WriteLine(@"ExportCount: " + _exportCount);

                    // Reads the Export IDs.
                    for (int i = 0; i < _exportCount; i++)
                    {
                        var export = new Export(this);
                        export.SetId(reader.ReadUInt16());
                        _exports.Add(export);
                    }

                    // Reads the Export names.
                    for (int i = 0; i < _exportCount; i++)
                    {
                        var nameLength = reader.ReadByte();
                        var name = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));
                        var export = (Export)_exports[i];
                        export.SetExportName(name);
                    }

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        long offset = reader.BaseStream.Position;

                        var datatag = reader.ReadByte();
                        var tag = datatag.ToString("X2"); //Block type
                        var tagSize = reader.ReadUInt32(); //Size in bytes
#if DEBUG
                        //Console.WriteLine("dataBlockTag: " + tag);
                        //Console.WriteLine("dataBlockSize: " + tagSize);
#endif
                        switch (tag)
                        {
                            default:
                               //Console.WriteLine("dataBlockTag: " + tag);
                               //Console.WriteLine("dataBlockSize: " + tagSize);
                               var defaultDataBlockContent = reader.ReadBytes(Convert.ToInt32(tagSize));
                                defaultDataBlockContent = null;
                               break;
                            case "00":
                                _eofOffset = offset;
                                foreach (ScObject t in _exports)
                                {
                                    int index = _movieClips.FindIndex(movie => movie.Id == t.Id);
                                    if (index != -1)
                                        ((Export)t).SetDataObject((MovieClip)_movieClips[index]);
                                }
                                break;
                            case "01":
                            case "18":
                                if (tagSize > 6)
                                {
                                    var texture = new Texture(this);
                                    texture.SetOffset(offset);
                                    texture.Read(UInt32.Parse(tag), tagSize, reader);
                                    //_eofTexOffset = reader.BaseStream.Position;
                                    this._textures.Add(texture);
                                }
                                else
                                {
                                    var pixelFormat = reader.ReadByte();
                                    var width = reader.ReadUInt16();
                                    var height = reader.ReadUInt16();

                                    Console.WriteLine("pixelFormat: " + pixelFormat);
                                    Console.WriteLine("width: " + width);
                                    Console.WriteLine("height: " + height);
                                }
                                break;
                            case "02":
                            case "12":
                                var shape = new Shape(this);
                                shape.SetOffset(offset);
                                shape.Read(reader, tag);
                                this._shapes.Add(shape);
                                break;

                            case "03":
                            case "0C":
                                var movieClip = new MovieClip(this, datatag);
                                movieClip.SetOffset(offset);
                                movieClip.Read(reader, tag);
                                _movieClips.Add(movieClip);
                                break;
                            case "08":
                                float[] Points = new float[6];
                                for (int Index = 0; Index < 6; Index++)
                                {
                                    Points[Index] = reader.ReadInt32();
                                }
                                Matrix _Matrix = new Matrix(Points[0] / 1024, Points[1] / 1024, Points[2] / 1024,
                                    Points[3] / 1024, Points[4] / -20, Points[5] / -20);
                                this._matrixs.Add(_Matrix);
                                //Console.WriteLine("\t matrixVal: " + Points[0] / 1024 + "\n\t matrixVal: " + Points[1] / 1024 + "\n\t matrixVal: " + Points[2] / 1024 + "\n\t matrixVal: " + Points[3] / 1024 + "\n\t matrixVal: " + Points[4] / -20 + "\n\t matrixVal: " + Points[5] / -20);
                                break;
                            case "09":
                                var ra = reader.ReadByte();
                                var ga = reader.ReadByte();
                                var ba = reader.ReadByte();
                                var am = reader.ReadByte();
                                var rm = reader.ReadByte();
                                var gm = reader.ReadByte();
                                var bm = reader.ReadByte();
                                this._colors.Add(new Tuple<Color, byte, Color>(Color.FromArgb(ra, ga, ba), am, Color.FromArgb(rm, gm, bm)));
                                //int id = this._colors.Count - 1;
                                //Console.WriteLine("\t -ct-" + id + "--ColorA: rgb(" + this._colors[id].Item1.R + "," + this._colors[id].Item1.G + "," + this._colors[id].Item1.B + ") & " + this._colors[id].Item2 + " & rgb(" + this._colors[id].Item3.R + "," + this._colors[id].Item3.G + "," + this._colors[id].Item3.B + ")");
                                break;
                        }
                    }

                    sw.Stop();
                    Program.Interface.Text = $@"Royale Editor :  {Path.GetFileNameWithoutExtension(_textureFile)}";
                    Program.Interface.Update();
                    Console.WriteLine(@"SC File loading finished in {0}ms", sw.Elapsed.TotalMilliseconds);
                }
                break;
            }
        }
    }
}
