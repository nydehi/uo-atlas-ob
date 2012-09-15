using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using UO_Atlas.Properties;


namespace UO_Atlas
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Atlas.EnsureLabelDatabase();

            LoadLabelCategories();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }




        private static void LoadLabelCategories()
        {

            string labelCategoryNamesFilePath = Path.Combine(Application.StartupPath, "Icons.txt");
            if(!File.Exists(labelCategoryNamesFilePath))
            {
                using (FileStream outStream = new FileStream(labelCategoryNamesFilePath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(outStream))
                    {
                        writer.Write(Resources.MapLabelCategories);
                    }
                }
            }

            string labelCategoryIconsFilePath = Path.Combine(Application.StartupPath, "Icons.png");
            if (!File.Exists(labelCategoryIconsFilePath))
            {
                
                using (FileStream outStream = new FileStream(labelCategoryIconsFilePath, FileMode.Create))
                {
                    Resources.LabelCategoryIcons.Save(outStream, ImageFormat.Png);
                }
            }


            using (StreamReader labelReader = new StreamReader(labelCategoryNamesFilePath))
            {
                using (Image icons = Image.FromFile(labelCategoryIconsFilePath))
                {
                    int iconX = 0;
                    int iconY = 0;

                    string labelName;
                    do
                    {
                        labelName = labelReader.ReadLine();
                        if (string.IsNullOrEmpty(labelName))
                        {
                            continue;
                        }

                        Bitmap labelIcon = new Bitmap(31, 31);
                        using (Graphics g = Graphics.FromImage(labelIcon))
                        {
                            g.DrawImage(icons, new Rectangle(0, 0, 31, 31), iconX, iconY, 31, 31, GraphicsUnit.Pixel);
                        }

                        iconX += 32;
                        if (iconX >= icons.Width)
                        {
                            iconY += 32;
                            iconX = 0;
                        }

                        LabelCategory category = new LabelCategory();
                        category.Name = labelName;
                        category.Icon = labelIcon;
                        Atlas.LabelCategories[labelName.ToUpper()] = category;

                    } while (labelName != null);
                }
            }
        }
    }
}