/*
 * TerrainNormals - MainForm.cs
 * 
 * - Graphical user interface of TerrainNormals. Allows to see the
 *   loaded heightmap and to visualize the calculated normal vectors
 *   on its surface.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace TerrainNormals
{
    public partial class MainForm : Form
    {
        Terrain _terrain;
        Vector3 _eye;
        bool _loaded = false;

        float _rot = 0f;
        float _zoom = 0.6f;

        public MainForm()
        {
            InitializeComponent();
            _eye = new Vector3(_zoom, _zoom, 0f);

            textBoxScale.Text = _zoom.ToString();
            textBoxRotation.Text = _rot.ToString();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            string path = "";

            try
            {
                path = openFileDialog.FileName;
                _terrain = TIFFReader.ReadTerrainFromTIFF(path, this);
                Draw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading heightmap", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _loaded = true;

            GL.Viewport(glControl.ClientRectangle.X, glControl.ClientRectangle.Y, glControl.ClientRectangle.Width, glControl.ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 pers = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)glControl.Size.Width / (float)glControl.Size.Height, 0.01f, 100.0f);
            GL.LoadMatrix(ref pers);

            Draw();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (!_loaded) return;

            GL.Viewport(glControl.ClientRectangle.X, glControl.ClientRectangle.Y, glControl.ClientRectangle.Width, glControl.ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 pers = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)glControl.Size.Width / (float)glControl.Size.Height, 0.01f, 100.0f);
            GL.LoadMatrix(ref pers);

            Draw();
        }

        private void Draw()
        {
            if (!_loaded) return;

            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_terrain != null)
            {
                int[] sz = _terrain.GetSize();
                int w = sz[0];
                int h = sz[1];

                Matrix4 look = Matrix4.LookAt(_eye, Vector3.Zero, Vector3.UnitY);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref look);

                GL.PointSize(10f*_zoom + 5f);

                // Draw Terrain
                float[,] m = _terrain.GetMap();
                float[,] r = _terrain.GetColorX();
                float[,] g = _terrain.GetColorY();
                float[,] b = _terrain.GetColorZ();

                for (int i = 0; i < h; i++)
                {
                    GL.Begin(PrimitiveType.Points);

                    for (int j = 0; j < w; j++)
                    {
                        float x = (float)i / (float)w - 0.5f;
                        float y = m[i, j] / 2f;
                        float z = (float)j / (float)h - 0.5f;

                        GL.Vertex3(x, y, z);
                        GL.Color3(r[i, j], g[i, j], b[i, j]);
                    }

                    GL.End();
                }

                // Draw Normal Vectors
                if (_terrain.HasNormals())
                {
                    float[,] nx = _terrain.GetNormalsX();
                    float[,] ny = _terrain.GetNormalsY();
                    float[,] nz = _terrain.GetNormalsZ();

                    for (int i = 0; i < h; i++)
                    {
                        for (int j = 0; j < w; j++)
                        {
                            if (i % 8 == 0 && j % 8 == 0)
                            {
                                float x = (float)i / (float)w - 0.5f;
                                float y = _terrain.GetMap()[i, j] / 2f;
                                float z = (float)j / (float)h - 0.5f;

                                GL.Begin(PrimitiveType.Lines);

                                GL.Vertex3(x, y, z);
                                GL.Color3(nx[i, j], ny[i, j], nz[i, j]);

                                GL.Vertex3(x + 0.01f * nx[i, j], y + 0.01f * ny[i, j], z + 0.01f * nz[i, j]);
                                GL.Color3(nx[i, j], ny[i, j], nz[i, j]);

                                GL.End();
                            }
                        }
                    }
                }
            }

            glControl.SwapBuffers();
            glControl.Invalidate();
        }

        public void UpdateProgress(int p)
        {
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (_terrain != null)
            {
                openFileDialog.ShowDialog();
                string path = "";

                try
                {
                    path = openFileDialog.FileName;
                    int[] sz = _terrain.GetSize();

                    Bitmap bmpor = new Bitmap(path);
                    Bitmap bmp = new Bitmap(bmpor, new Size(sz[0], sz[1]));
                    bmpor.Dispose();

                    float[,] r = _terrain.GetColorX();
                    float[,] g = _terrain.GetColorY();
                    float[,] b = _terrain.GetColorZ();

                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            Color c = bmp.GetPixel(i, j);
                            r[i, j] = (float)c.R / 255f;
                            g[i, j] = (float)c.G / 255f;
                            b[i, j] = (float)c.B / 255f;
                        }

                        UpdateProgress(100 * (int)((float)i / (float)bmp.Width));
                    }

                    bmp.Dispose();
                    Draw();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error loading heightmap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonNormals_Click(object sender, EventArgs e)
        {
            if (_terrain != null)
            {
                _terrain.GenerateNormalMap(this);
                Draw();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog.ShowDialog();
                string path = saveFileDialog.FileName;
                TIFFWriter.ExportTIFF(_terrain, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace, "Error exporting heightmap", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxRotation_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxRotation.Text.Contains('.'))
                {
                    toolTip.IsBalloon = true;
                    toolTip.SetToolTip(textBoxRotation, "Use COMMA as the decimal separator.");
                    throw new Exception();
                }

                _rot = float.Parse(textBoxRotation.Text) * 0.0174533f;
                _eye = new Vector3(_zoom, _zoom, 0f);
                _eye = Vector3.Transform(_eye, Matrix4.CreateRotationY(_rot));

                textBoxRotation.BackColor = Color.White;
                Draw();
            }
            catch (Exception ex)
            {
                textBoxRotation.BackColor = Color.Red;
            }
        }

        private void textBoxScale_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxScale.Text.Contains('.'))
                {
                    toolTip.IsBalloon = true;
                    toolTip.SetToolTip(textBoxScale, "Use COMMA as the decimal separator.");
                    throw new Exception();
                }

                _zoom = 1f / float.Parse(textBoxScale.Text);
                _eye = new Vector3(_zoom, _zoom, 0f);
                _eye = Vector3.Transform(_eye, Matrix4.CreateRotationY(_rot));

                textBoxScale.BackColor = Color.White;
                Draw();
            }
            catch (Exception ex)
            {
                textBoxScale.BackColor = Color.Red;
            }
        }

        private void buttonMesh_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog.ShowDialog();
                string path = saveFileDialog.FileName;
                File.WriteAllText(path, ExportOBJ.SerializeTerrain(_terrain, 128));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
