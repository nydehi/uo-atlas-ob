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
using System.Text;

namespace UO_Atlas
{
    /// <summary>
    /// An enum of all UO maps
    /// </summary>
    public enum MapName
    {
        Felucca = 0,
        Trammel,
        Ilshenar,
        Malas,
        Tokuno
    }

    public class Map
    {
        private static Dictionary<MapName, Map> m_MapList = new Dictionary<MapName, Map>();

        static Map()
        {
            m_MapList.Add(MapName.Felucca, new Map(0, Ultima.Map.Felucca));
            m_MapList.Add(MapName.Trammel, new Map(1, Ultima.Map.Trammel));
            m_MapList.Add(MapName.Ilshenar, new Map(2, Ultima.Map.Ilshenar));
            m_MapList.Add(MapName.Malas, new Map(3, Ultima.Map.Malas));
            m_MapList.Add(MapName.Tokuno, new Map(4, Ultima.Map.Tokuno));
        }

        /// <summary>
        /// Get a map by it's name.
        /// </summary>
        /// <param name="mapName">Name of the map</param>
        /// <returns>Map of the name</returns>
        public static Map Get(MapName mapName)
        {
            return m_MapList[mapName];
        }

        private Ultima.Map m_MapObject;
        private int m_Index;

        private Map(int index, Ultima.Map mapObject)
        {
            m_Index = index;
            m_MapObject = mapObject;
        }

        /// <summary>
        /// The index of the map
        /// </summary>
        public int Index { get { return m_Index; } }

        /// <summary>
        /// The width of the map
        /// </summary>
        public int Width { get { return m_MapObject.Width; } }

        /// <summary>
        /// The height of the map
        /// </summary>
        public int Height { get { return m_MapObject.Height; } }

        /// <summary>
        /// Ensures that the location is within the bounds of the current map
        /// </summary>
        /// <param name="location">the location to check</param>
        public void EnsureLocationWithinBounds(ref System.Drawing.Point location)
        {
            //check the x-Coordinate
            if (location.X > m_MapObject.Width)
                location.X = location.X % m_MapObject.Width;
            else if (location.X < 0)
                location.X = (location.X % m_MapObject.Width) + m_MapObject.Width;

            //check the y-Coordinate
            if (location.Y > m_MapObject.Height)
                location.Y = location.Y % m_MapObject.Height;
            else if (location.Y < 0)
                location.Y = (location.Y % m_MapObject.Height) + m_MapObject.Height;
        }

        /// <summary>
        /// Creates an image of the map.
        /// </summary>
        /// <param name="statics">Indicitaes whether statics should be drawn, too</param>
        /// <returns>An image of map with the same size</returns>
        public System.Drawing.Image ToImage(bool statics)
        {
            return m_MapObject.GetImage(0, 0, m_MapObject.Width >> 3, m_MapObject.Height >> 3, statics); // the size is measured in blocks!
        }
    }
}
