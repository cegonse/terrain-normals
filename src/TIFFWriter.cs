/*
 * TerrainNormals - TIFFWriter.cs
 * 
 * - The TIFFWriter class is used to save the heightmap
 *   along with its normals to a new TIFF image.
 *   
 *   The generated file will be stored as a 4 channel RGBA
 *   image, where the first channel (red) contains the height
 *   data and the three following channels (green, blue, alpha)
 *   contain the normal information (X,Y,Z components respectively).
 *   
 *   Behavior under big endian architectures is untested.
 * 
 *  2015, César Gonzàlez Segura (cegonse@gmail.com).
 * 
 *  This file is part of TerrainNormals.
 *
 *  TerrainNormals is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  TerrainNormals is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with TerrainNormals. If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using BitMiracle.LibTiff.Classic;
using System.Windows.Forms;

namespace TerrainNormals
{
    public class TIFFWriter
    {
        public static void ExportTIFF(Bitmap bitmap, string path)
        {
            Tiff tif = Tiff.Open(path, "w");
            
            byte[] raster = getImageRasterBytes(bitmap, PixelFormat.Format32bppArgb);
            tif.SetField(TiffTag.IMAGEWIDTH, bitmap.Width);
            tif.SetField(TiffTag.IMAGELENGTH, bitmap.Height);
            tif.SetField(TiffTag.COMPRESSION, Compression.LZW);
            tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);

            tif.SetField(TiffTag.ROWSPERSTRIP, bitmap.Height);

            tif.SetField(TiffTag.XRESOLUTION, bitmap.HorizontalResolution);
            tif.SetField(TiffTag.YRESOLUTION, bitmap.VerticalResolution);

            tif.SetField(TiffTag.BITSPERSAMPLE, 16);
            tif.SetField(TiffTag.SAMPLESPERPIXEL, 4);

            tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);

            int stride = raster.Length / bitmap.Height;
            convertSamples(raster, bitmap.Width, bitmap.Height);

            for (int i = 0, offset = 0; i < bitmap.Height; i++)
            {
                tif.WriteScanline(raster, offset, i, 0);
                offset += stride;
            }

            tif.Close();
        }

        public static void ExportTIFF(Terrain terr, string path)
        {
            float[,] heights = terr.GetMap();
            float[,] normalsx = terr.GetNormalsX();
            float[,] normalsy = terr.GetNormalsY();
            float[,] normalsz = terr.GetNormalsZ();
            int[] sz = terr.GetSize();
            int width = sz[0];
            int height = sz[1];

            Tiff tif = Tiff.Open(path, "w");

            if (tif == null)
            {
                throw new Exception("Opening TIFF file for write failed.");
            }

            int channelsPerPixel = 4;
            int bytesPerChannel = 2;

            // w * h * 16 bytes * samples per pixel
            byte[] raster = new byte[width * height * bytesPerChannel * channelsPerPixel];

            tif.SetField(TiffTag.IMAGEWIDTH, width);
            tif.SetField(TiffTag.IMAGELENGTH, height);
            tif.SetField(TiffTag.COMPRESSION, Compression.LZW);
            tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);

            tif.SetField(TiffTag.ROWSPERSTRIP, height);

            tif.SetField(TiffTag.XRESOLUTION, 72);
            tif.SetField(TiffTag.YRESOLUTION, 72);
            tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);

            tif.SetField(TiffTag.BITSPERSAMPLE, 8 * bytesPerChannel);
            tif.SetField(TiffTag.SAMPLESPERPIXEL, channelsPerPixel);
            tif.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);

            float ll = (float)Math.Pow(2, 16) - 1f;
            int n = 0;

            // Convert the height and normal data matrix into a raster vector
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    // Fetch the height and normal for this pixel
                    UInt16 pxHeight = (UInt16)(heights[x,y] * ll);
                    UInt16 pxNormalx = (UInt16)(((normalsx[x, y]) * 0.5f + 0.5f) * ll);
                    UInt16 pxNormaly = (UInt16)(((normalsy[x, y]) * 0.5f + 0.5f) * ll);
                    UInt16 pxNormalz = (UInt16)(((normalsz[x, y]) * 0.5f + 0.5f) * ll);

                    if (pxNormalx == 0 && pxNormalz == 0) pxNormaly = (UInt16)(ll);

                    byte[] tempHeight = new byte[2];
                    byte[] tempNormalx = new byte[2];
                    byte[] tempNormaly = new byte[2];
                    byte[] tempNormalz = new byte[2];

                    // Convert them into a 2 byte sequence
                    if (BitConverter.IsLittleEndian)
                    {
                        tempHeight[0] = (byte)(pxHeight & 0x00FF);
                        tempHeight[1] = (byte)((pxHeight & 0xFF00) >> 8);

                        tempNormalx[0] = (byte)(pxNormalx & 0x00FF);
                        tempNormalx[1] = (byte)((pxNormalx & 0xFF00) >> 8);

                        tempNormaly[0] = (byte)(pxNormaly & 0x00FF);
                        tempNormaly[1] = (byte)((pxNormaly & 0xFF00) >> 8);

                        tempNormalz[0] = (byte)(pxNormalz & 0x00FF);
                        tempNormalz[1] = (byte)((pxNormalz & 0xFF00) >> 8);
                    }
                    else
                    {
                        tempHeight[0] = (byte)((pxHeight & 0xFF00) >> 8);
                        tempHeight[1] = (byte)(pxHeight & 0x00FF);

                        tempNormalx[0] = (byte)((pxNormalx & 0xFF00) >> 8);
                        tempNormalx[1] = (byte)(pxNormalx & 0x00FF);

                        tempNormaly[0] = (byte)((pxNormaly & 0xFF00) >> 8);
                        tempNormaly[1] = (byte)(pxNormaly & 0x00FF);

                        tempNormalz[0] = (byte)((pxNormalz & 0xFF00) >> 8);
                        tempNormalz[1] = (byte)(pxNormalz & 0x00FF);
                    }

                    // Fill the raster for this pixel
                    // R channel (height)
                    raster[n++] = tempHeight[0];
                    raster[n++] = tempHeight[1];
                    // G channel (normal X)
                    raster[n++] = tempNormalx[0];
                    raster[n++] = tempNormalx[1];
                    // B channel (normal Y)
                    raster[n++] = tempNormaly[0];
                    raster[n++] = tempNormaly[1];
                    // A channel (normal Z)
                    raster[n++] = tempNormalz[0];
                    raster[n++] = tempNormalz[1];
                }
            }

            // Write out the raster
            int stride = width * bytesPerChannel * channelsPerPixel;

            for (int y = 0; y < height; y++)
            {
                byte[] buffer = new byte[stride];
                Buffer.BlockCopy(raster, stride * y, buffer, 0, buffer.Length);
                tif.WriteScanline(buffer, y);
            }

            tif.Close();
        }

        private static byte[] getImageRasterBytes(Bitmap bitmap, PixelFormat format)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            byte[] bits = null;

            try
            {
                // Lock the managed memory
                BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, format);

                // Declare an array to hold the bytes of the bitmap.
                bits = new byte[bitmapdata.Stride * bitmapdata.Height];

                // Copy the values into the array.
                System.Runtime.InteropServices.Marshal.Copy(bitmapdata.Scan0, bits, 0, bits.Length);

                // Release managed memory
                bitmap.UnlockBits(bitmapdata);
            }
            catch
            {
                return null;
            }

            return bits;
        }

        private static void convertSamples(byte[] data, int width, int height)
        {
            int stride = data.Length / height;
            const int samplesPerPixel = 4;

            for (int y = 0; y < height; y++)
            {
                int offset = stride * y;
                int strideEnd = offset + width * samplesPerPixel;

                for (int i = offset; i < strideEnd; i += samplesPerPixel)
                {
                    byte temp = data[i + 2];
                    data[i + 2] = data[i];
                    data[i] = temp;
                }
            }
        }
    }
}
