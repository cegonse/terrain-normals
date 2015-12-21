/*
 * TerrainNormals - Program.cs
 * 
 * - Main entry point of the application.
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
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace TerrainNormals
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string[] argv = Environment.GetCommandLineArgs();
            int argc = argv.Length;

            // If no arguments are passed, start the GUI
            if (argc == 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else if (argc >= 2)
            {
                // Convert all the heightmaps in the current folder
                if (argv[1] == "-a")
                {
                    // Get all the files in the folder
                    string[] files = Directory.GetFiles(Application.StartupPath);

                    // Start converting
                    Parallel.ForEach(files, CreateNormalmap);
                }
                else
                {
                    // Batch convert all the files passed as arguments
                    string[] files = new string[argc - 1];
                    argv.CopyTo(files, 1);

                    // Start converting
                    Parallel.ForEach(files, CreateNormalmap);
                }
            }
        }

        static void CreateNormalmap(string filename)
        {
            try
            {
                if (filename.Contains(".tiff") || filename.Contains(".tif"))
                {
                    // Load the heightmap and generate the normal map
                    Terrain t = TIFFReader.ReadTerrainFromTIFF(filename, null);
                    t.GenerateNormalMap(null);

                    // Create the destination path from the command line argument
                    string[] fn = filename.Split('.');
                    string saveName = fn[0] + "_normals.tiff";

                    TIFFWriter.ExportTIFF(t, saveName);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The file " + filename + " couldn't be converted. Skipping.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("The file " + filename + " is not a RGBA image. Skipping", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
