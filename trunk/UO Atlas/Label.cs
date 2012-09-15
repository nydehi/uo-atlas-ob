using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace UO_Atlas
{
    internal class Label
    {
        public LabelCategory Category { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public byte Facet { get; set; }
        public string Text { get; set; }

        public static Label LoadFrom(StreamReader reader)
        {
            string currentLine = reader.ReadLine();
            if(string.IsNullOrEmpty(currentLine))
            {
                return null;
            }

            int categoryEndIndex = currentLine.IndexOf(':', 1);
            if(categoryEndIndex < 0 || categoryEndIndex + 8 > currentLine.Length)
            {
                return null;
            }
            string categoryString = currentLine.Substring(1, categoryEndIndex - 1);

            LabelCategory category;
            if(!Atlas.LabelCategories.TryGetValue(categoryString, out category))
            {
               if(!Atlas.LabelCategories.TryGetValue(categoryString.ToUpper(), out category))
               {
                   return null;
               }
            }

            string[] theOtherParts = currentLine.Substring(categoryEndIndex + 2).Split(new char[] {' '}, 4);
            
            short x;
            if(!short.TryParse(theOtherParts[0], out x))
            {
                return null;
            }

            short y;
            if (!short.TryParse(theOtherParts[1], out y))
            {
                return null;
            }

            byte facet;
            if (!byte.TryParse(theOtherParts[2], out facet))
            {
                return null;
            }

            Label l = new Label();
            l.Category = category;
            l.X = x;
            l.Y = y;
            l.Facet = facet;
            l.Text = theOtherParts[3];
            return l;
        }
    }

    
    internal class LabelCategory
    {
        public string Name { get; set; }

        public Image Icon { get; set; }
    }
}