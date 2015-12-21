/*
 * TerrainNormals - TIFFReader.cs
 * 
 * - This file contains the TIFFReader class, used to read
 *   the heightmap on a TIFF file and create a Terrain object
 *   from it.
 *   
 *   The TIFF file is assumed to be a RGB encoded file, with one
 *   or more channels and having the height data in the first
 *   channel (red channel). 8 bit and 16 bit TIFF files are
 *   supported.
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
using BitMiracle.LibTiff.Classic;
using System.IO;

namespace TerrainNormals
{
    public class TIFFReader
    {
        public static Terrain ReadTerrainFromTIFF(string path, MainForm mf)
        {
            FieldValue[] value;
            int width = 0, height = 0, bpp = 0, bytespp = 0, channels = 0;

            Tiff tiff = Tiff.Open(path, "r");

            if (tiff == null)
            {
                throw new FileNotFoundException();
            }

            value = tiff.GetField(TiffTag.IMAGEWIDTH);
            width = value[0].ToInt();

            value = tiff.GetField(TiffTag.IMAGELENGTH);
            height = value[0].ToInt();

            value = tiff.GetField(TiffTag.BITSPERSAMPLE);
            bpp = value[0].ToInt();
            bytespp = (int)Math.Floor((double)bpp / 8.0);

            value = tiff.GetField(TiffTag.SAMPLESPERPIXEL);
            channels = value[0].ToInt();


            Terrain t = new Terrain(width, height);
            float[,] h = t.GetMap();
            float[,] g = t.GetColorY();
            byte[] buffer = new byte[width * bytespp * channels];

            float ll = (float)Math.Pow(2,bpp);

            // Cycle through all the rows
            for (int row = 0; row < height; row++)
            {
                tiff.ReadScanline(buffer, row);

                // Always use the component in the first channel as the height data
                for (int col = 0, c = 0; col < width * bytespp * channels; col += channels * bytespp, c++)
                {
                    if (bpp == 8)  
                    {
                        h[c, row] = (float)buffer[col] / ll;
                        g[c, row] = (float)buffer[col] / ll;
                    }
                    else if (bpp == 16)
                    {
                        UInt16 hv = 0;
                        hv |= (UInt16)(buffer[col] & 0x00FF);
                        hv |= (UInt16)((buffer[col + 1] << 8) & 0xFF00);

                        h[c, row] = (float)hv / ll;
                        g[c, row] = (float)hv / ll;
                    }
                }
            }

            return t;
        }
    }
}
