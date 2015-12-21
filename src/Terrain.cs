/*
 * TerrainNormals - Terrain.cs
 * 
 * - This file contains the definitions for the Terrain class,
 *   including the code to create the normal map from the
 *   heightmap. Makes use of the OpenTK vector algebra libraries.
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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;

namespace TerrainNormals
{
    public class Terrain
    {
        private int _width;
        private int _height;

        private float[,] _map;

        private float[,] _normalx;
        private float[,] _normaly;
        private float[,] _normalz;
        private float[,] _colorx;
        private float[,] _colory;
        private float[,] _colorz;

        private bool _normals = false;

        public Terrain(int w, int h)
        {
            _width = w;
            _height = h;

            _map = new float[w, h];
            _normalx = new float[w, h];
            _normaly = new float[w, h];
            _normalz = new float[w, h];
            _colorx = new float[w, h];
            _colory = new float[w, h];
            _colorz = new float[w, h];
        }

        public bool HasNormals()
        {
            return _normals;
        }

        public void GenerateNormalMap(MainForm mf)
        {
            int w = _width, h = _height;

            for (int i = 1; i < _height - 1; i++)
            {
                for (int j = 1; j < _width - 1; j++)
                {
                    Vector3 q0, q1, T, B, N;

                    q0 = new Vector3((float)i / (float)h - 0.5f, _map[i, j - 1], (float)(j-1) / (float)w - 0.5f);
                    q1 = new Vector3((float)i / (float)h - 0.5f, _map[i, j + 1], (float)(j + 1) / (float)w - 0.5f);
                    T = q1 - q0;

                    q0 = new Vector3((float)(i - 1) / (float)h - 0.5f, _map[i - 1, j], (float)j / (float)w - 0.5f);
                    q1 = new Vector3((float)(i + 1) / (float)h - 0.5f, _map[i + 1, j], (float)j / (float)w - 0.5f);
                    B = q1 - q0;

                    N = Vector3.Cross(Vector3.Normalize(T),Vector3.Normalize(B));
                    N = Vector3.Normalize(N);

                    _normalx[i, j] = N.X;
                    _normaly[i, j] = N.Y;
                    _normalz[i, j] = N.Z;
                }
            }

            // j = 0
            for (int i = 1; i < _height - 1; i++)
            {
                Vector3 nc, nd, nu, nr, q0, q1, q2, N;

                N = new Vector3();
                nc = new Vector3((float)i / (float)h - 0.5f, _map[i, 0], -0.5f);
                nd = new Vector3((float)(i - 1) / (float)h - 0.5f, _map[i - 1, 0], -0.5f);
                nu = new Vector3((float)(i + 1) / (float)h - 0.5f, _map[i + 1, 0], -0.5f);
                nr = new Vector3((float)i / (float)h - 0.5f, _map[i, 1], -0.5f + 1.0f / (float)w);

                q0 = nu - nc;
                q1 = nd - nc;
                q2 = nr - nc;

                N += Vector3.Cross(q1, q2);
                N += Vector3.Cross(q2, q0);
                N /= 2;
                N = N.Normalized();

                _normalx[i, 0] = N.X;
                _normaly[i, 0] = N.Y;
                _normalz[i, 0] = N.Z;
            }

            // j = width-1
            for (int i = 1; i < _height - 1; i++)
            {
                Vector3 nc, nd, nu, nr, q0, q1, q2, N;

                N = new Vector3();
                nc = new Vector3((float)i / (float)h - 0.5f, _map[i, _width - 1], (float)(_width - 1) / (float)w - 0.5f);
                nd = new Vector3((float)(i - 1) / (float)h - 0.5f, _map[i - 1, _width - 1], (float)(_width - 1) / (float)w - 0.5f);
                nu = new Vector3((float)(i + 1) / (float)h - 0.5f, _map[i + 1, _width - 1], (float)(_width - 1) / (float)w - 0.5f);
                nr = new Vector3((float)i / (float)h - 0.5f, _map[i, _width - 2], (float)(_width - 2) / (float)w - 0.5f);

                q0 = nu - nc;
                q1 = nd - nc;
                q2 = nr - nc;

                N += Vector3.Cross(q0, q2);
                N += Vector3.Cross(q2, q1);
                N /= 2;
                N = N.Normalized();

                _normalx[i, _width - 1] = N.X;
                _normaly[i, _width - 1] = N.Y;
                _normalz[i, _width - 1] = N.Z;
            }

            // i = 0
            for (int j = 1; j < _width - 1; j++)
            {
                Vector3 nc, nl, nu, nr, q0, q1, q2, N;

                N = new Vector3();
                nc = new Vector3(-0.5f, _map[0, j], (float)j / (float)w - 0.5f);
                nl = new Vector3(-0.5f, _map[0, j - 1], (float)(j - 1) / (float)w - 0.5f);
                nu = new Vector3(-0.5f + 1.0f / (float)h, _map[1, j], (float)j / (float)w - 0.5f);
                nr = new Vector3(-0.5f, _map[0, j + 1], (float)(j + 1) / (float)w - 0.5f);

                q0 = nl - nc;
                q1 = nu - nc;
                q2 = nr - nc;

                N += Vector3.Cross(q2, q1);
                N += Vector3.Cross(q1, q0);
                N /= 2;
                N = N.Normalized();

                _normalx[0, j] = N.X;
                _normaly[0, j] = N.Y;
                _normalz[0, j] = N.Z;
            }

            // i = height - 1
            for (int j = 1; j < _width - 1; j++)
            {
                Vector3 nc, nl, nu, nr, q0, q1, q2, N;

                N = new Vector3();
                nc = new Vector3((float)(_height - 1) / (float)h - 0.5f, _map[_height - 1, j], (float)j / (float)w - 0.5f);
                nl = new Vector3((float)(_height - 1) / (float)h - 0.5f, _map[_height - 1, j - 1], (float)(j - 1) / (float)w - 0.5f);
                nu = new Vector3((float)(_height - 2) / (float)h - 0.5f, _map[_height - 2, j], (float)j / (float)w - 0.5f);
                nr = new Vector3((float)(_height - 1) / (float)h - 0.5f, _map[_height - 1, j + 1], (float)(j + 1) / (float)w - 0.5f);

                q0 = nl - nc;
                q1 = nu - nc;
                q2 = nr - nc;

                N += Vector3.Cross(q0, q1);
                N += Vector3.Cross(q1, q2);
                N /= 2;
                N = N.Normalized();

                _normalx[_height - 1, j] = N.X;
                _normaly[_height - 1, j] = N.Y;
                _normalz[_height - 1, j] = N.Z;
            }

            // i = 0, j = 0
            {
                Vector3 N = new Vector3();

                N += new Vector3(_normalx[1, 0], _normaly[1, 0], _normalz[1, 0]);
                N += new Vector3(_normalx[0, 1], _normaly[0, 1], _normalz[0, 1]);
                N += new Vector3(_normalx[1, 1], _normaly[1, 1], _normalz[1, 1]);

                N /= 3.0f;
                N = N.Normalized();

                _normalx[0, 0] = N.X;
                _normaly[0, 0] = N.Y;
                _normalz[0, 0] = N.Z;
            }

            // i = height, j = width
            {
                Vector3 N = new Vector3();

                N += new Vector3(_normalx[_height - 2, _width - 1],
                    _normaly[_height - 2, _width - 1], _normalz[_height - 2, _width - 1]);

                N += new Vector3(_normalx[_height - 1, _width - 2],
                    _normaly[_height - 1, _width - 2], _normalz[_height - 1, _width - 2]);

                N += new Vector3(_normalx[_height - 2, _width - 2],
                    _normaly[_height - 2, _width - 2], _normalz[_height - 2, _width - 2]);

                N /= 3.0f;
                N = N.Normalized();

                _normalx[_height - 1, _width - 1] = N.X;
                _normaly[_height - 1, _width - 1] = N.Y;
                _normalz[_height - 1, _width - 1] = N.Z;
            }

            // i = 0, j = width
            {
                Vector3 N = new Vector3();

                N += new Vector3(_normalx[1, _width - 1], _normaly[1, _width - 1], _normalz[1, _width - 1]);
                N += new Vector3(_normalx[0, _width - 2], _normaly[0, _width - 2], _normalz[0, _width - 2]);
                N += new Vector3(_normalx[1, _width - 2], _normaly[1, _width - 2], _normalz[1, _width - 2]);

                N /= 3.0f;
                N = N.Normalized();

                _normalx[0, _width - 1] = N.X;
                _normaly[0, _width - 1] = N.Y;
                _normalz[0, _width - 1] = N.Z;
            }

            // i = height, j = 0
            {
                Vector3 N = new Vector3();

                N += new Vector3(_normalx[_height - 2, 0], _normaly[_height - 2, 0], _normalz[_height - 2, 0]);
                N += new Vector3(_normalx[_height - 1, 1], _normaly[_height - 1, 1], _normalz[_height - 1, 1]);
                N += new Vector3(_normalx[_height - 2, 1], _normaly[_height - 2, 1], _normalz[_height - 2, 1]);

                N /= 3.0f;
                N = N.Normalized();

                _normalx[_height - 1, 0] = N.X;
                _normaly[_height - 1, 0] = N.Y;
                _normalz[_height - 1, 0] = N.Z;
            }

            _normals = true;
        }

        public float[,] GetColorX()
        {
            return _colorx;
        }

        public float[,] GetColorY()
        {
            return _colory;
        }

        public float[,] GetColorZ()
        {
            return _colorz;
        }

        public int[] GetSize()
        {
            int[] sz = new int[2];
            sz[0] = _width;
            sz[1] = _height;

            return sz;
        }

        public float[,] GetMap()
        {
            return _map;
        }

        public float[,] GetNormalsX()
        {
            return _normalx;
        }

        public float[,] GetNormalsY()
        {
            return _normaly;
        }

        public float[,] GetNormalsZ()
        {
            return _normalz;
        }
    }
}
