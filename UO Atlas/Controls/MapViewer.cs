/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;


namespace UO_Atlas
{
    public enum ZoomLevel
    {
        PercentOneSixteenth = 0,
        PercentOneEighth,
        PercentOneQuarter,
        PercentOneHalf,
        PercentOneHundred,
        PercentTwoHundred,
        PercentFourHundred,
        PercentEightHundred,
        PercentSixteenHundred,

        MinimumZoom = PercentOneSixteenth,
        MaximumZoom = PercentSixteenHundred
    }

    /// <summary>
    /// Contains all necessary information to handle different zoom levels
    /// </summary>
    public struct ZoomInfo
    {
        ZoomLevel m_ZoomLevel;
        string m_FileName;
        float m_RealZoom;
        int m_ImageZoom;

        /// <summary>
        /// Creates a new ZoomLevel
        /// </summary>
        /// <param name="realZoom">the zoom in relation to the map size</param>
        /// <param name="drawZoom">the zoom in relation to the map-image</param>
        public ZoomInfo(ZoomLevel zoomLevel, string fileName, float realZoom, int drawZoom)
        {
            m_ZoomLevel = zoomLevel;
            m_FileName = fileName;
            m_RealZoom = realZoom;
            m_ImageZoom = drawZoom;
        }

        /// <summary>
        /// Indicates the zoom in an enum
        /// </summary>
        public ZoomLevel ZoomLevel { get { return m_ZoomLevel; } }
        /// <summary>
        /// The filename of the map-image at the current zoom-level
        /// </summary>
        public string FileName { get { return m_FileName; } }
        /// <summary>
        /// RealZoom indicates the zoom in relation to the map size
        /// </summary>
        public float RealZoom { get { return m_RealZoom; } }
        /// <summary>
        /// ImageZoom indicates the zoom of the loaded image
        /// </summary>
        public int ImageZoom { get { return m_ImageZoom; } }
    }

    /// <summary>
    /// A control to draw the map
    /// </summary>
    public partial class MapViewer : UserControl
    {
        public event EventHandler<MapViewerEventArgs> OnMapChanged;
        public event EventHandler<MapViewerEventArgs> OnCoordinatesChanged;
        public event EventHandler<MapViewerEventArgs> OnZoomLevelChanged;
        public event ErrorEventHandler OnError;

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public MapViewer()
        {
            SetZoomInfo();

            InitializeComponent();

            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
              ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void SetZoomInfo()
        {
            m_ZoomInfos.Add(ZoomLevel.PercentOneSixteenth, new ZoomInfo(ZoomLevel.PercentOneSixteenth, "map{0}-6.25%.png", 0.06250f, 1));
            m_ZoomInfos.Add(ZoomLevel.PercentOneEighth, new ZoomInfo(ZoomLevel.PercentOneEighth, "map{0}-12.5%.png", 0.125f, 1));
            m_ZoomInfos.Add(ZoomLevel.PercentOneQuarter, new ZoomInfo(ZoomLevel.PercentOneQuarter, "map{0}-25%.png", 0.250f, 1));
            m_ZoomInfos.Add(ZoomLevel.PercentOneHalf, new ZoomInfo(ZoomLevel.PercentOneHalf, "map{0}-50%.png", 0.500f, 1));
            m_ZoomInfos.Add(ZoomLevel.PercentOneHundred, new ZoomInfo(ZoomLevel.PercentOneHundred, "map{0}-100%.png", 1.000f, 1));
            m_ZoomInfos.Add(ZoomLevel.PercentTwoHundred, new ZoomInfo(ZoomLevel.PercentTwoHundred, "map{0}-100%.png", 2.0f, 2));
            m_ZoomInfos.Add(ZoomLevel.PercentFourHundred, new ZoomInfo(ZoomLevel.PercentFourHundred, "map{0}-100%.png", 4.0f, 4));

            m_ZoomInfos.Add(ZoomLevel.PercentEightHundred, new ZoomInfo(ZoomLevel.PercentEightHundred, "map{0}-100%.png", 8.0f, 8));
            m_ZoomInfos.Add(ZoomLevel.PercentSixteenHundred, new ZoomInfo(ZoomLevel.PercentSixteenHundred, "map{0}-100%.png", 16.0f, 16));
        }

        private Map m_Map;
        private Image m_MapImage;
        private Size m_CanvasSize = new Size(0, 0);
        private Dictionary<ZoomLevel, ZoomInfo> m_ZoomInfos = new Dictionary<ZoomLevel, ZoomInfo>();
        private ZoomLevel m_ZoomLevel = ZoomLevel.MinimumZoom;
        private Point m_PaintLocation = new Point(0, 0);
        private Point m_CenterLocation = new Point(0, 0);
        private Point m_HoveredLocation = new Point(0, 0);
        private Point m_GrabbedLocation = new Point(0, 0);
        private bool m_ScroolbarUpdate = false;

        /// <summary>
        /// The map that will be painted
        /// </summary>
        public Map Map
        {
            get { return m_Map; }
            set
            {
                if (m_Map == value)
                    return;

                m_Map = value;

                if (m_Map != null)
                {
                    ReloadMapImage();

                    setScrollbarValues();
                    OnMapLocationChanged();
                    Invalidate();
                }

                if (OnMapChanged != null)
                    OnMapChanged(this, new MapViewerEventArgs(this.Map, this.CenterLocation, this.ZoomLevel));
            }
        }

        /// <summary>
        /// The canvas-area we are about to draw in
        /// </summary>
        public Size CanvasSize
        {
            get { return m_CanvasSize; }
        }

        /// <summary>
        /// The level of zoom
        /// </summary>
        public ZoomLevel ZoomLevel
        {
            get { return m_ZoomLevel; }
            set
            {
                if (m_ZoomLevel == value)
                    return;

                m_ZoomLevel = value;

                if (m_Map != null)
                {
                    ReloadMapImage();
                    setScrollbarValues();
                    SetLocation(m_CenterLocation);
                }

                if (OnZoomLevelChanged != null)
                    OnZoomLevelChanged(this, new MapViewerEventArgs(this.Map, this.CenterLocation, this.ZoomLevel));
            }
        }

        /// <summary>
        /// The map coordinates of the top-left corner (for all paint-procedures)
        /// </summary>
        public Point PaintLocation { get { return m_PaintLocation; } }

        /// <summary>
        /// Gets the map coordinates of the centered location
        /// </summary>
        public Point CenterLocation { get { return m_CenterLocation; } }

        /// <summary>
        /// The coordinates at the current mouse-position
        /// </summary>
        public Point HoveredLocation { get { return m_HoveredLocation; } }

        /// <summary>
        /// RealZoom indicates the zoom in relation to the map size
        /// </summary>
        private float RealZoom { get { return m_ZoomInfos[m_ZoomLevel].RealZoom; } }

        /// <summary>
        /// RealZoom indicates the zoom in relation to the current image
        /// </summary>
        private int ImageZoom { get { return m_ZoomInfos[m_ZoomLevel].ImageZoom; } }


        public void Zoom(int steps)
        {
            if (steps == 0)
                return;

            //check for zoom > maximum
            if ((int)ZoomLevel + steps > (int)ZoomLevel.MaximumZoom)
            {
                ZoomLevel = ZoomLevel.MaximumZoom;
                return;
            }

            //check for zoom < minimum
            if ((int)ZoomLevel + steps < (int)ZoomLevel.MinimumZoom)
            {
                ZoomLevel = ZoomLevel.MinimumZoom;
                return;
            }

            ZoomLevel = (ZoomLevel)((int)ZoomLevel + steps);
        }

        protected override void OnResize(EventArgs e)
        {
            // Recalculate canvas size
            m_CanvasSize.Width = this.Width - vScrollBar.Width;
            m_CanvasSize.Height = this.Height - hScrollBar.Height;

            // Readjust scrollbars
            hScrollBar.Width = this.Width;
            vScrollBar.Height = m_CanvasSize.Height;
            hScrollBar.Top = m_CanvasSize.Height;
            vScrollBar.Left = m_CanvasSize.Width;

            setScrollbarValues();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //draw image
            if (m_MapImage != null)
            {
                try
                {

                    // if the image fits into canvas completely
                    if (m_CanvasSize.Width > m_MapImage.Width && m_CanvasSize.Height > m_MapImage.Height)
                    {
                        int x = 0, y = 0;

                        Graphics g = e.Graphics;

                        while (x < m_CanvasSize.Width)
                        {
                            y = 0;

                            while (y < m_CanvasSize.Height)
                            {
                                g.DrawImage(m_MapImage, x, y);
                                y += m_MapImage.Height;
                            }

                            x += m_MapImage.Width;
                        }

                    }
                    // if the image fits NOT into canvas, show a part
                    else
                    {
                        Rectangle sourceRect, destRect;

                        // the coordinates of the top-left corner of the map-image
                        int X = (int) (m_PaintLocation.X * RealZoom / ImageZoom);
                        int Y = (int) (m_PaintLocation.Y * RealZoom / ImageZoom);

                        // width and length of the visible map-rectangle
                        int Width = (int) (m_CanvasSize.Width / ImageZoom);
                        int Height = (int) (m_CanvasSize.Height / ImageZoom);

                        sourceRect = new Rectangle(X, Y, Width, Height); // which part of the image will be painted?
                        destRect = new Rectangle(0, 0, m_CanvasSize.Width, m_CanvasSize.Height); // paint into the canvas

                        Graphics g = e.Graphics;
                        g.DrawImage(m_MapImage, destRect, sourceRect, GraphicsUnit.Pixel);

                        // Calculate whether the source-retangle reaches the border of the map
                        int rightBorder = (int) (X + Width - m_Map.Width * RealZoom / ImageZoom);
                        int bottomBorder = (int) (Y + Height - m_Map.Height * RealZoom / ImageZoom);

                        // the sourceRectangle reaches over the right border of the map
                        if (rightBorder >= 0)
                        {
                            sourceRect = new Rectangle(0, Y, rightBorder, m_CanvasSize.Height * ImageZoom);
                            destRect = new Rectangle(m_CanvasSize.Width - rightBorder, 0, rightBorder, m_CanvasSize.Height);

                            g.DrawImage(m_MapImage, destRect, sourceRect, GraphicsUnit.Pixel);
                        }

                        // the sourceRectangle reaches over the bottom border of the map
                        if (bottomBorder >= 0)
                        {
                            sourceRect = new Rectangle(X, 0, m_CanvasSize.Width * ImageZoom, bottomBorder);
                            destRect = new Rectangle(0, m_CanvasSize.Height - bottomBorder, m_CanvasSize.Width, bottomBorder);

                            g.DrawImage(m_MapImage, destRect, sourceRect, GraphicsUnit.Pixel);
                        }

                        // the sourceRectangle reaches over the right and the bottom border of the map
                        if (rightBorder >= 0 && bottomBorder >= 0)
                        {
                            sourceRect = new Rectangle(0, 0, rightBorder, bottomBorder);
                            destRect = new Rectangle(m_CanvasSize.Width - rightBorder, m_CanvasSize.Height - bottomBorder, rightBorder, bottomBorder);

                            g.DrawImage(m_MapImage, destRect, sourceRect, GraphicsUnit.Pixel);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void ReloadMapImage()
        {
            // RAM cleanup 
            if (m_MapImage != null)
                m_MapImage.Dispose();

            string mapImagePath = Path.Combine(Atlas.MapImagesFolder, String.Format(m_ZoomInfos[m_ZoomLevel].FileName, m_Map.Index));

            if (File.Exists(mapImagePath))
            {
                Rectangle imageBounds;
                PixelFormat pixelFormat;
                int totalBytesUsedByEitherImage;
                byte[] fromFileImageBytes;

                using(Bitmap fromFileImage = new Bitmap(mapImagePath))
                {
                    imageBounds = new Rectangle(0, 0, fromFileImage.Width, fromFileImage.Height);
                    pixelFormat = fromFileImage.PixelFormat;
                    System.Drawing.Imaging.BitmapData fromFileImageData = fromFileImage.LockBits(imageBounds, System.Drawing.Imaging.ImageLockMode.ReadOnly, pixelFormat);

                    totalBytesUsedByEitherImage = Math.Abs(fromFileImageData.Stride) * imageBounds.Height;

                    fromFileImageBytes = new byte[totalBytesUsedByEitherImage];
                    System.Runtime.InteropServices.Marshal.Copy(fromFileImageData.Scan0, fromFileImageBytes, 0, totalBytesUsedByEitherImage);

                    fromFileImage.UnlockBits(fromFileImageData);
                    fromFileImage.Dispose();
                }


                Bitmap inMemoryImage = new Bitmap(imageBounds.Width, imageBounds.Height, pixelFormat);
                System.Drawing.Imaging.BitmapData inMemoryImageData = inMemoryImage.LockBits(imageBounds, System.Drawing.Imaging.ImageLockMode.WriteOnly, pixelFormat);
                System.Runtime.InteropServices.Marshal.Copy(fromFileImageBytes, 0, inMemoryImageData.Scan0, totalBytesUsedByEitherImage);
                inMemoryImage.UnlockBits(inMemoryImageData);

                fromFileImageBytes = null;
                

                m_MapImage = inMemoryImage;
            }
            else
            {
                if (OnError != null)
                    OnError(this, "A map-image is missing. Please generate the map files.");
            }
        }

        private string ZoomLevelToString()
        {
            switch (m_ZoomLevel)
            {
                case ZoomLevel.PercentOneSixteenth:
                    return "1/16x";
                case ZoomLevel.PercentOneEighth:
                    return "1/8x";
                case ZoomLevel.PercentOneQuarter:
                    return "1/4x";
                case ZoomLevel.PercentOneHalf:
                    return "1/2x";
                case ZoomLevel.PercentOneHundred:
                    return "1x";
                case ZoomLevel.PercentTwoHundred:
                    return "2x";
                case ZoomLevel.PercentFourHundred:
                    return "4x";
                case ZoomLevel.PercentEightHundred:
                    return "8x";
                case ZoomLevel.PercentSixteenHundred:
                    return "16x";
                default:
                    return "1x";
            }
        }

        private void setScrollbarValues()
        {
            if (m_Map != null)
            {
                // if the image fits into canvas completely disable scrolling
                hScrollBar.Enabled = vScrollBar.Enabled = !(m_CanvasSize.Width > m_Map.Width && m_CanvasSize.Height > m_Map.Height);

                hScrollBar.Minimum = (int)(m_Map.Width * -0.1);
                vScrollBar.Minimum = (int)(m_Map.Height * -0.1);

                hScrollBar.Maximum = (int)(m_Map.Width * 1.3);
                vScrollBar.Maximum = (int)(m_Map.Height * 1.3);

                hScrollBar.SmallChange = (int)(m_Map.Width * 0.1);
                vScrollBar.SmallChange = (int)(m_Map.Height * 0.1);

                hScrollBar.LargeChange = (int)(m_Map.Width * 0.2);
                vScrollBar.LargeChange = (int)(m_Map.Height * 0.2);
            }
        }

        /// <summary>
        /// Set the coordinates of the centered point
        /// </summary>
        public void SetLocation(int x, int y)
        {
            SetLocation(new Point(x, y));
        }

        /// <summary>
        /// Set the coordinates of the centered point
        /// </summary>
        public void SetLocation(Point location)
        {
            m_Map.EnsureLocationWithinBounds(ref location);

            Point topLeft = new Point();
            topLeft.X = (int)(location.X - m_CanvasSize.Width / RealZoom / 2);
            topLeft.Y = (int)(location.Y - m_CanvasSize.Height / RealZoom / 2);

            m_Map.EnsureLocationWithinBounds(ref topLeft);
            m_PaintLocation = topLeft;

            OnMapLocationChanged();

            if (OnCoordinatesChanged != null)
                OnCoordinatesChanged(this, new MapViewerEventArgs(this.Map, this.CenterLocation, this.ZoomLevel));
        }

        /// <summary>
        /// Adjust scrollbars and repaint image
        /// </summary>
        private void OnMapLocationChanged()
        {
            m_CenterLocation.X = (int)(m_PaintLocation.X + m_CanvasSize.Width / RealZoom / 2);
            m_CenterLocation.Y = (int)(m_PaintLocation.Y + m_CanvasSize.Height / RealZoom / 2);

            m_Map.EnsureLocationWithinBounds(ref m_CenterLocation);

            m_ScroolbarUpdate = true;
            hScrollBar.Value = m_CenterLocation.X;
            vScrollBar.Value = m_CenterLocation.Y;
            hScrollBar.Invalidate();
            vScrollBar.Invalidate();
            m_ScroolbarUpdate = false;

            this.Invalidate();
        }

        private void MapViewer_MouseMove(object sender, MouseEventArgs e)
        {
            m_HoveredLocation.X = (int)(m_PaintLocation.X + e.X / RealZoom) % m_Map.Width;
            m_HoveredLocation.Y = (int)(m_PaintLocation.Y + e.Y / RealZoom) % m_Map.Height;

            // When the map is "dragged" then move it based on the coordinates on MouseDown
            if (e.Button == MouseButtons.Left)
            {
                int offsetX = m_HoveredLocation.X - m_GrabbedLocation.X;
                int offsetY = m_HoveredLocation.Y - m_GrabbedLocation.Y;

                SetLocation(m_CenterLocation.X - offsetX, m_CenterLocation.Y - offsetY);
            }
        }

        private void MapViewer_MouseClick(object sender, MouseEventArgs e)
        {
            // Middle mousebutton -> Center the clicked coordinates
            if (e.Button == MouseButtons.Middle)
            {
                SetLocation(m_HoveredLocation);
            }
        }

        private void MapViewer_MouseDown(object sender, MouseEventArgs e)
        {
            // Memory the currently hovered coordinates
            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.Hand;

                m_GrabbedLocation.X = m_HoveredLocation.X;
                m_GrabbedLocation.Y = m_HoveredLocation.Y;
            }
        }

        private void MapViewer_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (!m_ScroolbarUpdate)
                SetLocation(hScrollBar.Value, vScrollBar.Value);
        }
    }

    public class MapViewerEventArgs : EventArgs
    {
        public Map Map;
        public Point Coordinates;
        public ZoomLevel ZoomLevel;

        public MapViewerEventArgs(Map map, Point coordinates, ZoomLevel zoomlevel)
        {
            Map = map;
            Coordinates = coordinates;
            ZoomLevel = zoomlevel;
        }
    }
}
