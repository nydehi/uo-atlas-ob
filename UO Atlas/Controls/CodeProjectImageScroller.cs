using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace UO_Atlas.Controls
{
    public class CodeProjectImageScroller : Form
    {

        int _InputImageWidth, _InputImageHeight, _ClientWidth, _ClientHeight, _OutputImageStride;
        private PictureBox _Picbox;
        private VScrollBar _VScrollBar;
        private Bitmap _BufferImage, _InputImageCopy;
        
        MemoryStream _InputImageStream;
        private byte[] _OuputImageData;
        private readonly byte[] _OuputPixelBuffer = new byte[3];


        public CodeProjectImageScroller()
        {
            InitializeComponent();
        }


        private void ResizeForm_Resize(object sender, EventArgs e)
        {
            _ClientWidth = ClientSize.Width;
            _ClientHeight = ClientSize.Height;

            _VScrollBar.Location = new Point(_ClientWidth - 15, 0);
            _VScrollBar.Size = new Size(15, _ClientHeight);
            _Picbox.Size = new Size(_ClientWidth - 15, _ClientHeight);

            if (_BufferImage != null)
            {
                _BufferImage.Dispose();
                GC.Collect();
                _BufferImage = new Bitmap(_InputImageWidth, _ClientHeight, PixelFormat.Format24bppRgb);
                Graphics gtw = Graphics.FromImage(_BufferImage);
                gtw.Clear(Color.White);
                gtw.Dispose();
            }
            Invalidate();
        }
        private void InitializeComponent()
        {
            SuspendLayout();
            ClientSize = new Size(600, 500);
            MinimumSize = new Size(300, 300);
            _ClientWidth = ClientSize.Width;
            _ClientHeight = ClientSize.Height;

            Text = "Fast Image Scroll";
            Resize += ResizeForm_Resize;
            Paint += Form_Paint;

            _VScrollBar = new VScrollBar();
            _VScrollBar.Maximum = 0;
            _VScrollBar.Minimum = 0;
            _VScrollBar.Name = "small scroll bar";
            _VScrollBar.Size = new Size(15, _ClientHeight);
            _VScrollBar.Value = 0;
            _VScrollBar.Scroll += SbarScroll;
            _VScrollBar.Location = new Point(_ClientWidth - 15, 0);

            _Picbox = new PictureBox();
            _Picbox.BackColor = Color.White;  //Set White backgroud of Picture Box


            _BufferImage = new Bitmap(_ClientWidth - 15, _ClientHeight, PixelFormat.Format24bppRgb);
            _InputImageHeight = _BufferImage.Height; _InputImageWidth = _BufferImage.Width;
            Graphics gtw = Graphics.FromImage(_BufferImage);
            gtw.Clear(Color.White);
            gtw.Dispose();
            _Picbox.Image = _BufferImage;
            _Picbox.Name = "_Picbox";
            _Picbox.Location = new Point(0, 0);
            _Picbox.Size = new Size(_ClientWidth - 15, _ClientHeight);
            _Picbox.SizeMode = PictureBoxSizeMode.StretchImage;// .Normal;
            Controls.AddRange(new Control[] { _Picbox, _VScrollBar });

            OpenFileDialog fileOpen = new OpenFileDialog();
            fileOpen.InitialDirectory = ".\\";
            fileOpen.Filter = "All file (*.*)| *.*";
            fileOpen.FilterIndex = 1; fileOpen.ShowHelp = true;
            fileOpen.RestoreDirectory = false; //true;
            if (fileOpen.ShowDialog() != DialogResult.Cancel)
            {
                string inputfile = fileOpen.FileName;
                UnpackImage(inputfile); ResumeLayout(false);
            }
            else Close(); // if cancel is pressed, close the window
        }



        private void SbarScroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }



        private void Form_Paint(object sender, PaintEventArgs e)
        {

            Rectangle displayBounds = new Rectangle(0, 0, _BufferImage.Width, _BufferImage.Height);
            BitmapData bitmapdata = _BufferImage.LockBits(displayBounds, ImageLockMode.ReadWrite, _BufferImage.PixelFormat);

            long cnt = 0;
            unsafe
            {
                byte* data = (byte*)bitmapdata.Scan0;

                int a = _VScrollBar.Value;
                long b = _OutputImageStride;
                _InputImageStream.Position = a * b;
                long xpos = 0;
                for (int j = 0; j < (_InputImageWidth * (Math.Min(_ClientHeight, _InputImageHeight))); j++)
                {
                    _InputImageStream.Read(_OuputPixelBuffer, 0, 3);
                    data[cnt + 2] = _OuputPixelBuffer[2];  //B component
                    data[cnt + 1] = _OuputPixelBuffer[1];  //G component
                    data[cnt] = _OuputPixelBuffer[0];  //R component
                    cnt += 3; xpos++;
                    if (xpos == _InputImageWidth)
                    {
                        if ((_InputImageWidth * 3) % 4 != 0)
                        {
                            cnt += (4 - (_InputImageWidth * 3) % 4);
                            _InputImageStream.Read(_OuputPixelBuffer, 0, (4 - (_InputImageWidth * 3) % 4));
                        } xpos = 0;
                    }
                }

            }
            _BufferImage.UnlockBits(bitmapdata);
            _Picbox.Image = _BufferImage;
            _Picbox.Update();
        }

        public void UnpackImage(string file)
        {
            Bitmap timp;
            try { timp = (Bitmap)Image.FromFile(file); }
            catch { MessageBox.Show("Unknown Image format type."); return; }
            _InputImageWidth = timp.Width; _InputImageHeight = timp.Height;
            if ((_InputImageWidth * 3) % 4 == 0) _OutputImageStride = (3 * _InputImageWidth);
            else _OutputImageStride = ((_InputImageWidth * 3) + 4 - (_InputImageWidth * 3) % 4);

            _OuputImageData = new byte[_OutputImageStride * _InputImageHeight];
            GCHandle handle = GCHandle.Alloc(_OuputImageData, GCHandleType.Pinned);
            IntPtr pointer = Marshal.UnsafeAddrOfPinnedArrayElement(_OuputImageData, 0);
            _InputImageCopy = new Bitmap(_InputImageWidth, _InputImageHeight, _OutputImageStride, PixelFormat.Format24bppRgb, pointer);
            _InputImageStream = new MemoryStream(_OuputImageData);

            Graphics gtw = Graphics.FromImage(_InputImageCopy);
            gtw.DrawImage(timp, new Rectangle(0, 0, _InputImageWidth, _InputImageHeight));
            gtw.Dispose();
            timp.Dispose();

            _VScrollBar.Value = 0;
            _VScrollBar.Maximum = Math.Max(_InputImageHeight - _ClientHeight, 0);
            _BufferImage = new Bitmap(_InputImageWidth, _ClientHeight, PixelFormat.Format24bppRgb);
            gtw = Graphics.FromImage(_BufferImage);
            gtw.Clear(Color.White);
            gtw.Dispose();
            _Picbox.Image = _BufferImage;
        }


    }
}
