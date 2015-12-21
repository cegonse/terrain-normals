# TerrainNormals

An application to embed normal maps into terrain 
heightmaps.

## Behavior

The application creates the normal map from the
terrain heightmap samples.

For each sample, the normal vector of the surface
composed by the current sample and its four adjacent
samples is obtained. Then, the four vectors are
averaged and normalized to estimate the normal vector
at the sample.

## Usage

TerrainNormals can be launched with a Graphical User
Interface or in batch mode.

* Launching the application without arguments will
launch the GUI mode. From the GUI, a heightmap can be
loaded and the terrain along with its normal vectors
can be visualized and stored.

* If the first argument passed to the application is
"-a", the application will convert all the heightmap
files in the directory containing the executable.

* If more than one argument is passed, the application
will interpret them as target heightmap files to convert.

## Limitations

The terrain heightmap files must be:

* RGB color scheme TIFF files.
* Composed of one or more channels.
* Have a bit depth of 8 or 16 bits per sample.

The height data is assumed to be stored in the first
component of the image (the red channel).

The exported heightmap will preserve the red channel with
the original height data and store the normal vectors for
each sample in the green, blue and alpha channels (X,Y,Z
components respectively).

## Building

Should compile under most .NET platforms supporting Windows
Forms. The GUI can be removed if necessary.

LibTIFF and OpenTK are needed to build the application. These
libraries can be obtained from:

http://bitmiracle.com/libtiff/
http://www.opentk.com/

## Contributing

TerrainNormals is licensed under the Lesser GPL license, version 3.
Feel free to contribute or to use all or any of its components
as needed.