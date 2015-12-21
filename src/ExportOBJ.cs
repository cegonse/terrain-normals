﻿/*
 * TerrainNormals - ExportOBJ.cs
 * 
 * - This file contains the ExportOBJ class with the definitions to
 *   export the heightmap along with its normals as a Wavefront
 *   OBJ mesh.
 *   
 *   This functionality is unfinished and not working properly.
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

namespace TerrainNormals
{
    class ExportOBJ
    {
        public static string SerializeTerrain(Terrain t, int lod = 1)
        {
            StringBuilder data = new StringBuilder();

            float[,] map = t.GetMap();
            float[,] nx = t.GetNormalsX();
            float[,] ny = t.GetNormalsY();
            float[,] nz = t.GetNormalsZ();

            data.Append("# Generated by TerrainNormals\n");
            data.Append("# https://github.com/cegonse/terrain-normals \n");
            data.Append("# " + System.DateTime.Now.ToLongDateString() + "\n");

            int sx = t.GetSize()[0];
            int sz = t.GetSize()[1];

            float dx = 1f / (float)(sx);
            float dz = 1f / (float)(sz);

            float vx = -((float)sx / 2f) / (float)sx;
            float vz = -((float)sz / 2f) / (float)sz;
            int tc = 0;

            for (int i = 0; i < sx; i++)
            {
                if (i % lod == 0)
                {
                    for (int j = 0; j < sz; j++)
                    {
                        if (j % lod == 0)
                        {
                            data.Append(string.Format("v {0} {1} {2}\n", vx, map[i, j], vz));
                            tc++;

                            if (tc == 3)
                            {
                                data.Append("f -3 -2 -1\n");
                                tc = 0;
                            }
                        }

                        vz += dz;
                    }
                }

                vz = -((float)sx / 2f) / (float)sx;
                vx += dx;
            }

            data.Append("\n");

            for (int i = 0; i < sx; i++)
            {
                if (i % lod == 0)
                {
                    for (int j = 0; j < sz; j++)
                    {
                        if (j % lod == 0)
                        {
                            data.Append(string.Format("vn {0} {1} {2}\n", nx[i, j], ny[i, j], nz[i, j]));
                        }
                    }
                }
            }

            data.Append("\n");

            float uvx = 0f;
            float uvy = 0f;

            for (int i = 0; i < sx; i++)
            {
                if (i % lod == 0)
                {
                    for (int j = 0; j < sz; j++)
                    {
                        if (j % lod == 0)
                        {
                            data.Append(string.Format("vt {0} {1}\n", uvx, uvy));
                        }

                        uvy += dz;
                    }
                }

                uvy = 0;
                uvx += dx;
            }

            data.Append("\n");
            return data.ToString();
        }
    }
}
