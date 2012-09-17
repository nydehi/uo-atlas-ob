using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace UO_Atlas.Controls
{
    public partial class ImageViewer : UserControl
    {
        private HScrollBar _HScrollBar;
        private readonly VScrollBar _VScrollBar;

        private int _ClientWidth;
        private int _ClientHeight;

        private int _Stride;
        private Bitmap _BufferImage;
        private int _BufferImageWidth;
        private int _BufferImageHeight;
        
        private GCHandle _ImageHandle;
        private MemoryStream _ImageStream;



        public ImageViewer()
        {
            InitializeComponent();

            _ClientWidth = ClientSize.Width;
            _ClientHeight = ClientSize.Height;

            _VScrollBar = new VScrollBar();
            _VScrollBar.Maximum = 0;
            _VScrollBar.Minimum = 0;
            _VScrollBar.Size = new Size(15, _ClientHeight);
            _VScrollBar.Value = 0;
            _VScrollBar.Scroll += OnVerticalScrollBarPositionChanged;
            _VScrollBar.Location = new Point(_ClientWidth - 15, 0);
            Controls.Add(_VScrollBar);

            _BufferImage = new Bitmap(_ClientWidth - 15, _ClientHeight, PixelFormat.Format24bppRgb);
            _BufferImageWidth = _BufferImage.Width;
            _BufferImageHeight = _BufferImage.Height;

            using(Graphics g = Graphics.FromImage(_BufferImage))
            {
                g.Clear(Color.White);
            }
        }



        private void OnVerticalScrollBarPositionChanged(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }



        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _ClientWidth = ClientSize.Width;
            _ClientHeight = ClientSize.Height;

            _VScrollBar.Location = new Point(_ClientWidth - 15, 0);
            _VScrollBar.Size = new Size(15, _ClientHeight);

            if(_BufferImage != null)
            {
                if(_ImageHandle.IsAllocated)
                {
                    _ImageHandle.Free();
                }
                
                _BufferImage.Dispose();
                GC.Collect();

                _BufferImage = new Bitmap(_BufferImageWidth, _ClientHeight, PixelFormat.Format24bppRgb);
                using(Graphics g = Graphics.FromImage(_BufferImage))
                {
                    g.Clear(Color.White);
                }
            }

            Invalidate();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle r = new Rectangle(0, 0, _BufferImageWidth, _BufferImageHeight);

            BitmapData imageData = _BufferImage.LockBits(r, ImageLockMode.ReadWrite, _BufferImage.PixelFormat);

            long cnt = 0;
            long xPos = 0;
            unsafe
            {
                IntPtr imagePointer = imageData.Scan0;
                byte* data = (byte*) imagePointer;

                long a = _VScrollBar.Value;
                long b = _Stride;
                _ImageStream.Position = a * b;

                for (int j = 0; j < _BufferImageWidth * Math.Min(_ClientHeight, _BufferImageHeight); j += 1)
                {
                    byte[] buffer = new byte[3];
                    _ImageStream.Read(buffer, 0, 3);

                    data[cnt + 2] = buffer[2];
                    data[cnt + 1] = buffer[1];
                    data[cnt] = buffer[0];
                    cnt += 3;
                    xPos += 1;

                    if(xPos == _BufferImageWidth)
                    {
                        int stride = _BufferImageWidth * 3;
                        if (stride % 4 != 0)
                        {
                            cnt += (4 - stride % 4);
                            _ImageStream.Read(buffer, 0, (4 - stride % 4));
                        }
                        
                        xPos = 0;
                    }
                }
            }

            _BufferImage.UnlockBits(imageData);

            e.Graphics.DrawImage(_BufferImage, 0, 0);
        }



        public void LoadImage(string fullFilePathToImage)
        {
            UnpackImage(fullFilePathToImage, _ClientHeight, out _BufferImageHeight, out _Stride, out _ImageHandle, out _ImageStream, out _BufferImage);
            _BufferImageWidth = _BufferImage.Width;
        }



        private static void UnpackImage(string file, int height, out int newImageHeight, out int stride, out GCHandle imageDataHandle, out MemoryStream imageStream, out Bitmap resultingImage)
        {
            using(Bitmap temporaryImage = (Bitmap) Image.FromFile(file))
            {
                int imageWidth = temporaryImage.Width;
                newImageHeight = temporaryImage.Height;

                stride = imageWidth * 3;
                if (stride % 4 != 0)
                {
                    stride = (stride + 4 - stride % 4);
                }

                byte[] bigByte = new byte[stride * newImageHeight];
                imageDataHandle = GCHandle.Alloc(bigByte, GCHandleType.Pinned);
                IntPtr pointer = Marshal.UnsafeAddrOfPinnedArrayElement(bigByte, 0);
                imageStream = new MemoryStream(bigByte);

                using (Bitmap tbmp = new Bitmap(imageWidth, newImageHeight, stride, PixelFormat.Format24bppRgb, pointer))
                {
                    using (Graphics gtw = Graphics.FromImage(tbmp))
                    {
                        gtw.DrawImage(temporaryImage, new Rectangle(0, 0, imageWidth, newImageHeight));
                    }
                }

                resultingImage = new Bitmap(imageWidth, height, PixelFormat.Format24bppRgb);

                using(Graphics g = Graphics.FromImage(resultingImage))
                {
                    g.Clear(Color.White);
                }
            }

        }
    }
}
