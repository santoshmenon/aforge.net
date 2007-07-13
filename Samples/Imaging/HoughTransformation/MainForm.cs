// AForge Framework
// Hough line transformation demo
//
// Copyright � Andrew Kirillov, 2007
// andrew.kirillov@gmail.com
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AForge.Imaging;
using AForge.Imaging.Filters;

namespace HoughTransform
{
    public partial class MainForm : Form
    {
        private Bitmap sourceImage;
        private Bitmap houghLineImage;

        // binarization filtering sequence
        private FiltersSequence filter = new FiltersSequence(
            new GrayscaleBT709( ),
            new Threshold( )
        );

        HoughLineTransformation lineTransform = new HoughLineTransformation( );


        // Construct MainForm
        public MainForm( )
        {
            InitializeComponent( );
        }

        // Exit from application
        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit( );
        }

        // Open image file
        private void openToolStripMenuItem_Click( object sender, EventArgs e )
        {
            try
            {
                // show file open dialog
                if ( openFileDialog.ShowDialog( ) == DialogResult.OK )
                {
                    // load image
                    Bitmap image = (Bitmap) Bitmap.FromFile( openFileDialog.FileName );
                    // format image
                    AForge.Imaging.Image.FormatImage( ref image );
                    // binarize the image
                    sourceImage = filter.Apply( image );
                    
                    // apply Hough line transofrm
                    lineTransform.ProcessImage( sourceImage );
                    houghLineImage = lineTransform.ToBitmap( );
                    // get lines using relative intensity
                    HoughLine[] lines = lineTransform.GetLinesByRelativeIntensity( 0.5 );

                    foreach ( HoughLine line in lines )
                    {
                        string s = string.Format( "Theta = {0}, R = {1}, I = {2} ({3})", line.Theta, line.Radius, line.Intensity, line.RelativeIntensity );
                        System.Diagnostics.Debug.WriteLine( s );
                    }

                    System.Diagnostics.Debug.WriteLine( "Found lines: " + lineTransform.LinesCount );
                    System.Diagnostics.Debug.WriteLine( "Max intensity: " + lineTransform.MaxIntensity );

                    // show images
                    sourcePictureBox.Image = sourceImage;
                    houghLinePictureBox.Image = houghLineImage;
                }
            }
            catch
            {
                MessageBox.Show( "Failed loading the image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }
    }
}