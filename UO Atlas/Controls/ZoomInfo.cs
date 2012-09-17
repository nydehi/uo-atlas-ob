using System;
using System.Collections.Generic;
using System.Text;

namespace UO_Atlas.Controls
{
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
        /// <param name="fileName"></param>
        /// <param name="realZoom">the zoom in relation to the map size</param>
        /// <param name="drawZoom">the zoom in relation to the map-image</param>
        /// <param name="zoomLevel"></param>
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
}
