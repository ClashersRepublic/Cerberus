using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows;
using CR.Assets.Editor.ScOld.ImageFormats;

namespace CR.Assets.Editor.ScOld
{
    public class Texture : ScObject, IDisposable
    {
        #region Constants

        private static readonly Dictionary<byte, Type> s_imageTypes;

        #endregion

        #region Constructors

        static Texture()
        {
            s_imageTypes = new Dictionary<byte, Type>
            {
                {0, typeof(ImageRgba8888)},
                {1, typeof(ImageRgba8888)},
                {2, typeof(ImageRgba4444)},
                {3, typeof(ImageRgba5551 )},
                {4, typeof(ImageRgb565)},
                {6, typeof(ImageLuminance8Alpha8)},
                {10, typeof(ImageLuminance8)}

            };
        }

        public Texture(ScFile scs)
        {
            _scFile = scs;
            _textureId = (ushort) _scFile.GetTextures().Count();
        }

        public Texture(Texture t)
        {
            _imageType = t.GetImageType();
            _scFile = t.GetStorageObject();
            _textureId = (ushort) _scFile.GetTextures().Count();
            if (s_imageTypes.ContainsKey(_imageType))
            {
                _image = (ScImage) Activator.CreateInstance(s_imageTypes[_imageType]);
            }
            else
            {
                _image = new ScImage();
            }
            _image.SetBitmap(new Bitmap(t.Bitmap));
            _offset = t.GetOffset() > 0 ? -t.GetOffset() : -1/*t.GetOffset()*/;
        }

        #endregion

        #region Fields & Properties

        internal uint PacketId;
        internal byte _imageType;
        internal ushort _textureId;


        internal bool _disposed;
        internal ScFile _scFile;
        internal ScImage _image;
        internal long _offset;

        public override Bitmap Bitmap => _image.GetBitmap();

        #endregion

        #region Methods

        public override ushort Id => _textureId;

        public override int GetDataType()
        {
            return 2;
        }

        public override string GetDataTypeName()
        {
            return "Textures";
        }

        internal ScImage GetImage()
        {
            return _image;
        }

        public byte GetImageType()
        {
            return _imageType;
        }

        public override string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TextureId: " + _textureId);
            sb.AppendLine("ImageType: " + _imageType);
            sb.AppendLine("ImageFormat: " + _image.GetImageTypeName());
            sb.AppendLine("Width: " + _image.GetWidth());
            sb.AppendLine("Height: " + _image.GetHeight());
            return sb.ToString();
        }

        public long GetOffset()
        {
            return _offset;
        }

        public ScFile GetStorageObject()
        {
            return _scFile;
        }

        public ushort GetTextureId()
        {
            return _textureId;
        }

        public override bool IsImage()
        {
            return true;
        }

        public new void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _image.Dispose();
            }

            _disposed = true;
        }
        public void Read(uint packetID, uint packetSize, BinaryReader br)
        {
            this.PacketId = packetID;
            _imageType = br.ReadByte();

            if (s_imageTypes.ContainsKey(_imageType))
                _image = (ScImage) Activator.CreateInstance(s_imageTypes[_imageType]);
            else
                _image = new ScImage();

            _image.ReadImage(packetID, packetSize, br);
        }

        public override Bitmap Render(RenderingOptions options)
        {
            return Bitmap;
        }

        public override void Write(FileStream input)
        {
            int bytesForPXFormat = 4;
            switch (_imageType)
            {
                case 2:
                case 4:
                case 6:
                    bytesForPXFormat = 2;
                    break;
                case 3:
                    _imageType = 2;
                    bytesForPXFormat = 2;
                    break;
                case 10:
                    bytesForPXFormat = 1;
                    break;
            }


            UInt32 packetSize = (uint) ((_image.GetWidth()) * (_image.GetHeight()) * bytesForPXFormat) + 5;

            if (_offset < 0) // New
            { 
                input.Seek(_scFile.GetEofTexOffset(), SeekOrigin.Begin);
                input.WriteByte(1);
                input.Write(BitConverter.GetBytes(packetSize), 0, 4);
                input.WriteByte(_imageType);

                _image.WriteImage(input);
                _offset = _scFile.GetEofTexOffset();
                _scFile.SetEofTexOffset(input.Position);

                input.Write(new byte[] {0, 0, 0, 0, 0}, 0, 5);
            }
            else // Existing
            {
                input.Seek(_offset, SeekOrigin.Current);

                input.WriteByte(1);
                input.Write(BitConverter.GetBytes(packetSize), 0, 4);
                input.WriteByte(_imageType);
                _image.WriteImage(input);
            }
        }

        public void SetOffset(long position)
        {
            _offset = position;
        }

        #endregion
    }
}
