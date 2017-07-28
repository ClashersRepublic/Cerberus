using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using MathNet.Numerics.LinearAlgebra;
using CR.Assets.Editor.Helpers;

namespace CR.Assets.Editor.ScOld
{
    public class Shape : ScObject
    {
        #region Constructors

        public Shape(ScFile scFile)
        {
            _scFile = scFile;
            _chunks = new List<ScObject>();
        }

        public Shape(Shape shape)
        {
            _scFile = shape.GetStorageObject();
            _chunks = new List<ScObject>();

            SetOffset(-Math.Abs(shape.GetOffset()));

            //Duplicate Shape
            using (FileStream input = new FileStream(_scFile.GetInfoFileName(), FileMode.Open))
            {
                input.Seek(Math.Abs(shape.GetOffset()), SeekOrigin.Begin);
                using (var br = new BinaryReader(input))
                {
                    var packetId = br.ReadByte().ToString("X2");
                    var packetSize = br.ReadUInt32();
                    this.Read(br, packetId);
                }
            }

            foreach (ShapeChunk chunk in _chunks)
            {
                chunk.SetOffset(-Math.Abs(chunk.GetOffset()));
            }
        }

        #endregion

        #region Fields & Properties


        private bool _disposed;
        private ushort _shapeId;
        private int _length;
        private List<ScObject> _chunks;
        private ScFile _scFile;
        private long _offset;


        internal string _shapeType;
        public override ushort Id => _shapeId;
        public override List<ScObject> Children => _chunks;

        #endregion

        #region Methods

        public override int GetDataType()
        {
            return 0;
        }

        public override string GetDataTypeName()
        {
            return "Shapes";
        }

        public override string GetName()
        {
            return "Shape " + Id.ToString();
        }

        public List<ScObject> GetChunks()
        {
            return _chunks;
        }

        public override string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("/!\\ Experimental Rendering");
            sb.AppendLine("");
            sb.AppendLine("ShapeId: " + _shapeId);
            sb.AppendLine("Polygons: " + _chunks.Count);
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

        public override bool IsImage()
        {
            return true;
        }

        public sealed override void Read(BinaryReader br, string id)
        {
            /*
            StringBuilder hex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
                hex.AppendFormat("{0:x2}", b);
            Console.WriteLine(hex.ToString());
            */
            _shapeId = br.ReadUInt16(); // 0000
            br.ReadUInt16(); // 0100
            if (id == "12")
                br.ReadUInt16(); // 0400 if datatype 18

            while (true)
            {
                byte chunkType;
                while (true)
                {
                    chunkType = br.ReadByte(); // 11
                    _length = br.ReadInt32(); // 32000000
                    if (chunkType == 17 || chunkType == 22)
                    {
                        ShapeChunk chunk = new ShapeChunk(_scFile);
                        chunk.SetChunkId((ushort) _chunks.Count);
                        chunk.SetShapeId(_shapeId);
                        chunk.SetChunkType(chunkType);
                        chunk.Read(br, id);

                        _chunks.Add(chunk);
                    }
                    else
                    {
                        break;
                    }
                }
                if (chunkType == 0)
                    break;
                Console.WriteLine("Unmanaged chunk type " + chunkType);
                br.ReadBytes(_length);
            }
        }

        public override Bitmap Render(RenderingOptions options)
        {
            /*
            Console.WriteLine("XY:");
            foreach(ShapeChunk chunk in m_vChunks)
            {
                foreach(var p in chunk.GetPointsXY())
                {
                    Console.WriteLine("x: " + p.X + ", y: " + p.Y);
                }
                Console.WriteLine("");
            }
        
            foreach (ShapeChunk chunk in m_vChunks)
            {
                foreach (var p in chunk.GetPointsUV())
                {
                    Console.WriteLine("u: " + p.X + ", u: " + p.Y);
                }
                Console.WriteLine("");
            }
            */

            Console.WriteLine("Rendering image of " + _chunks.Count + " polygons");


            // Calculate et initialize the final shape size
            PointF[] pointsXY = _chunks.SelectMany(chunk => ((ShapeChunk) chunk).XY).ToArray();
            using (var xyPath = new GraphicsPath())
            {
                xyPath.AddPolygon(pointsXY.ToArray());

                var xyBound = Rectangle.Round(xyPath.GetBounds());

                var width = xyBound.Width;
                width = width > 0 ? width : 1;

                var height =  xyBound.Height;
                height = height > 0 ? height : 1;

                var x = xyBound.X;
                var y = xyBound.Y;

                var finalShape = new Bitmap(width, height);
                Console.WriteLine($"Rendering shape: W:{finalShape.Width} H:{finalShape.Height}\n");

                // Assemble shape chunks
                foreach (ShapeChunk chunk in _chunks)
                {
                    var texture = (Texture) _scFile.GetTextures()[chunk.GetTextureId()];
                    Bitmap bitmap = texture.Bitmap;

                    using (var gpuv = new GraphicsPath())
                    {
                        gpuv.AddPolygon(chunk.UV.ToArray());

                        var gxyBound = Rectangle.Round(gpuv.GetBounds());

                        int gpuvWidth = gxyBound.Width;
                        gpuvWidth = gpuvWidth > 0 ? gpuvWidth : 1;

                        int gpuvHeight = gxyBound.Height;
                        gpuvHeight = gpuvHeight > 0 ? gpuvHeight : 1;

                        var shapeChunk = new Bitmap(gpuvWidth, gpuvHeight);

                        var chunkX = gxyBound.X;
                        var chunkY = gxyBound.Y;
                        
                        using (var g = Graphics.FromImage(shapeChunk))
                        {
                            gpuv.Transform(new Matrix(1, 0, 0, 1, -chunkX, -chunkY));
                            g.SetClip(gpuv);
                            g.DrawImage(bitmap, -chunkX, -chunkY);
                        }

                        GraphicsPath gp = new GraphicsPath();
                        gp.AddPolygon(new[] {new Point(0, 0), new Point(gpuvWidth, 0), new Point(0, gpuvHeight)});

                        //Calculate transformation Matrix of UV
                        //double[,] matrixArrayUV = { { polygonUV[0].X, polygonUV[1].X, polygonUV[2].X }, { polygonUV[0].Y, polygonUV[1].Y, polygonUV[2].Y }, { 1, 1, 1 } };
                        double[,] matrixArrayUV =
                        {
                            {
                                gpuv.PathPoints[0].X, gpuv.PathPoints[1].X, gpuv.PathPoints[2].X
                            },
                            {
                                gpuv.PathPoints[0].Y, gpuv.PathPoints[1].Y, gpuv.PathPoints[2].Y
                            },
                            {
                                1, 1, 1
                            }
                        };
                        double[,] matrixArrayXY =
                        {
                            {
                                chunk.XY[0].X, chunk.XY[1].X, chunk.XY[2].X
                            },
                            {
                                chunk.XY[0].Y, chunk.XY[1].Y, chunk.XY[2].Y
                            },
                            {
                                1, 1, 1
                            }
                        };

                        var matrixUV = Matrix<double>.Build.DenseOfArray(matrixArrayUV);
                        var matrixXY = Matrix<double>.Build.DenseOfArray(matrixArrayXY);
                        var inverseMatrixUV = matrixUV.Inverse();
                        var transformMatrix = matrixXY * inverseMatrixUV;
                        var m = new Matrix((float) transformMatrix[0, 0], (float) transformMatrix[1, 0], (float) transformMatrix[0, 1], (float) transformMatrix[1, 1], (float) transformMatrix[0, 2], (float) transformMatrix[1, 2]);
                        //m = new Matrix((float)transformMatrix[0, 0], (float)transformMatrix[1, 0], (float)transformMatrix[0, 1], (float)transformMatrix[1, 1], (float)Math.Round(transformMatrix[0, 2]), (float)Math.Round(transformMatrix[1, 2]));

                        //Perform transformations
                        gp.Transform(m);

                        using (Graphics g = Graphics.FromImage(finalShape))
                        {
                            //Set origin
                            Matrix originTransform = new Matrix();
                            originTransform.Translate(-x, -y);
                            g.Transform = originTransform;

                            g.DrawImage(shapeChunk, gp.PathPoints, gpuv.GetBounds(), GraphicsUnit.Pixel);

                            if (options.ViewPolygons)
                            {
                                gpuv.Transform(m);
                                g.DrawPath(new Pen(Color.DeepSkyBlue, 1), gpuv);
                            }
                            g.Flush();
                        }
                    }

                }
                return finalShape;
                    
            }
        }

        public override void Write(FileStream input)
        {
            if (_offset < 0) //new
            {
                using (FileStream readInput = new FileStream(_scFile.GetInfoFileName(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //Positionnement des curseurs
                    readInput.Seek(Math.Abs(_offset), SeekOrigin.Begin);
                    input.Seek(_scFile.GetEofOffset(), SeekOrigin.Begin);

                    //type and length
                    byte dataType = (byte) readInput.ReadByte();
                    byte[] dataLength = new byte[4];
                    readInput.Read(dataLength, 0, 4);

                    input.WriteByte(dataType);
                    input.Write(dataLength, 0, 4);

                    //shape
                    readInput.Seek(2, SeekOrigin.Current);
                    input.Write(BitConverter.GetBytes(_shapeId), 0, 2);

                    byte[] unknown1 = new byte[2];
                    readInput.Read(unknown1, 0, 2); //0100
                    input.Write(unknown1, 0, 2);

                    if (dataType == 18)
                    {
                        byte[] unknown2 = new byte[2];
                        readInput.Read(unknown2, 0, 2); //0400
                        input.Write(unknown2, 0, 2);
                    }

                    int chunkCounter = 0;
                    while (true)
                    {
                        byte shapeType;
                        byte[] length = new byte[4];
                        while (true)
                        {
                            shapeType = (byte) readInput.ReadByte(); //11
                            input.WriteByte(shapeType);

                            readInput.Read(length, 0, 4); //32000000
                            input.Write(length, 0, 4);

                            if (shapeType == 17 || shapeType == 22)
                            {
                                Console.WriteLine("Managed shape type " + shapeType);
                                _chunks[chunkCounter].Write(input);
                                chunkCounter++;
                                readInput.Seek(BitConverter.ToInt32(length, 0), SeekOrigin.Current);
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (shapeType == 0)
                        {
                            break;
                        }
                        Console.WriteLine("Unmanaged shape type " + shapeType);
                        for (int i = 0; i < BitConverter.ToInt32(length, 0); i++)
                        {
                            input.WriteByte((byte) readInput.ReadByte());
                        }
                    }
                }
                _offset = _scFile.GetEofOffset();
                _scFile.SetEofOffset(input.Position);
                input.Write(new byte[] {0, 0, 0, 0, 0}, 0, 5);
            }
        }
        public override void Dispose()
        {
            if (_disposed)
                return;

            foreach (var chunk in _chunks)
            {
                chunk.Dispose();
            }

            _disposed = true;
        }
        public void SetId(ushort id)
        {
            _shapeId = id;
        }

        public void SetOffset(long offset)
        {
            _offset = offset;
        }

        #endregion
    }
}