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
            string categoryString = currentLine.Substring(0, categoryEndIndex);

            LabelCategory category;
            LabelCategory.Cache.TryGetValue(categoryString.ToUpper(), out category);

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
        public static readonly Dictionary<string, LabelCategory> Cache = new Dictionary<string,LabelCategory>();
        public static void LoadCache()
        {
            using(StreamReader labelReader = new StreamReader("Icons.txt"))
            {
                using(Image icons = Image.FromFile("Icons.bmp"))
                {
                    int iconX = 0;
                    int iconY = 0;

                    string labelName;
                    do
                    {
                        labelName = labelReader.ReadLine();
                        if(string.IsNullOrEmpty(labelName))
                        {
                            continue;
                        }

                        Bitmap labelIcon = new Bitmap(31, 31);
                        using(Graphics g = Graphics.FromImage(labelIcon))
                        {
                            g.DrawImage(icons, new Rectangle(0, 0, 31, 31), iconX, iconY, 31, 31, GraphicsUnit.Pixel);
                        }

                        iconX += 32;
                        if(iconX >= icons.Width)
                        {
                            iconY += 32;
                            iconX = 0;
                        }
                        
                        LabelCategory category = new LabelCategory();
                        category.Name = labelName;
                        category.Icon = labelIcon;
                        Cache[labelName.ToUpper()] = category;

                    } while (labelName != null);
                }
            }
        }

        public string Name { get; set; }

        public Image Icon { get; set; }
    }
}