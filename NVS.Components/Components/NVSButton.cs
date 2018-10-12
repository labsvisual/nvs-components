using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NVS.Extensions;
using System.ComponentModel;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace NVS.Components
{

    /// <summary>Bevel border style.</summary>
    public enum BevelStyle
    {
        /// <summary>Lowered border.</summary>
        Lowered,
        /// <summary>Raised border.</summary>
        Raised,
        /// <summary>No border.</summary>
        Flat
    }

    /// <summary>
    /// Represents a custom-designed button.
    /// </summary>
    public class NVSButton : Button
    {

        private PrivateFontCollection _privateFontCollection = new PrivateFontCollection();
        private string _fontFolderPath;
        private string _fontFilePath;
        private Color _primaryBaseColor = ColorTranslator.FromHtml("#212020");
        private Color _depressedButtonColor = ColorTranslator.FromHtml("#02bcf2");
        private bool _isDepressed = false;

        private Color _currentBaseColor;
        private Color _currentTextColor = Color.White;

        private const Border3DSide DefaultShape = Border3DSide.Bottom;
        private const BevelStyle DefaultStyle = BevelStyle.Lowered;

        private Border3DSide shape;
        /// <summary>
        /// Gets or sets the shape of the bevel.
        /// </summary>
        [DefaultValue(DefaultShape)]
        public Border3DSide Shape
        {
            get
            {
                return shape;
            }
            set
            {
                shape = value;
                Invalidate();
            }
        }

        private BevelStyle style;
        /// <summary>
        /// Gets or sets the style of the bevel.
        /// </summary>
        [DefaultValue(DefaultStyle)]
        public BevelStyle Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The drop effect of the glow.
        /// </summary>
        [Description("The drop effect of the glow.")]
        [Category("Appearance")]
        public int DropFactor { get; set; } = 5;

        /// <summary>
        /// The radius of the button.
        /// </summary>
        [Description("Gets or sets the radius of the button")]
        [Category("Appearance")]
        public int BorderRadius { get; set; } = 10;

        /// <summary>
        /// The font size of the button.
        /// </summary>
        [Description("The font size of the text on the button")]
        [Category("Appearance")]
        public float FontSize { get; set; } = 14.0f;

        private GraphicsPath GetRoundedPath(RectangleF rect, int radius)
        {
            GraphicsPath gPath = new GraphicsPath();

            float fixedRadius = radius / 2;

            gPath.StartFigure();

            // Top left to top right
            gPath.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            gPath.AddLine(rect.X + fixedRadius, rect.Y, rect.Width - fixedRadius, rect.Y);

            // Top right to bottom right
            gPath.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            gPath.AddLine(rect.Width, rect.Y + fixedRadius, rect.Width, rect.Height - fixedRadius);

            // Bottom right to bottom left
            gPath.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            gPath.AddLine(rect.Width - fixedRadius, rect.Height, rect.X + fixedRadius, rect.Height);

            // Bottom left to top left
            gPath.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            gPath.AddLine(rect.X, rect.Height - fixedRadius, rect.X, rect.Y + fixedRadius);

            gPath.CloseFigure();

            return gPath;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this._currentBaseColor = ColorTranslator.FromHtml("#2D2D2D");
            this._currentTextColor = this._depressedButtonColor;

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this._currentBaseColor = this._primaryBaseColor;
            this._currentTextColor = Color.White;

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this._isDepressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            this._isDepressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rect = new RectangleF(0, 0, this.Bounds.Width, this.Bounds.Height);
            GraphicsPath gPath = GetRoundedPath(rect, 25);

            this.Region = new Region(gPath);
            using (Pen p = new Pen(this._currentBaseColor, 1.75f))
            {
                p.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(p, gPath);
            }

            SolidBrush brush = new SolidBrush(this._currentBaseColor);
            e.Graphics.FillRoundedRectangle(brush, 0, 0, this.Bounds.Width, this.Bounds.Height, this.BorderRadius);
            
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Transparent, 0, ButtonBorderStyle.None, // left
                Color.Transparent, 0, ButtonBorderStyle.None, // top
                Color.Transparent, 0, ButtonBorderStyle.None, // right
                Color.Black, 10, ButtonBorderStyle.Solid);    // bottom

            //Rectangle rectR = new Rectangle(this.ClientRectangle.X + (int)(this.ClientRectangle.Width * 0.25), this.ClientRectangle.Y, (int)(this.Bounds.Width - (this.Bounds.Width * 0.5)), this.Bounds.Height);
            ////Rectangle rectR = new Rectangle(this.ClientRectangle.X, this.ClientRectangle.Y, this.Bounds.Width, this.Bounds.Height);
            //ControlPaint.DrawBorder(e.Graphics, rectR,
            //    Color.Transparent, 0, ButtonBorderStyle.None, // left
            //    this._currentTextColor, 4, ButtonBorderStyle.Solid, // top
            //    Color.Transparent, 0, ButtonBorderStyle.None, // right
            //    Color.Transparent, 0, ButtonBorderStyle.None);    // bottom

            FontFamily fFamily = _privateFontCollection.Families[0];
            Font font = new Font(fFamily.Name, this.FontSize);
            StringFormat stringFormat = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            
            if (!this._isDepressed)
            {
                e.Graphics.DrawString(this.Text.ToUpper(), font, new SolidBrush(this._currentTextColor), this.ClientRectangle, stringFormat);
            }
            else
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Black, 10, ButtonBorderStyle.Solid, // left
                Color.Black, 10, ButtonBorderStyle.Solid, // top
                Color.Black, 10, ButtonBorderStyle.Solid, // right
                Color.Black, 10, ButtonBorderStyle.Solid);    // bottom

                e.Graphics.DrawString(this.Text.ToUpper(), font, new SolidBrush(this._currentTextColor), this.ClientRectangle, stringFormat);

                //Bitmap bmp = new Bitmap(this.ClientSize.Width / this.DropFactor, this.ClientSize.Height / this.DropFactor);
                //GraphicsPath graphicsPath = new GraphicsPath();

                //graphicsPath.AddString(this.Text.ToUpper(), fFamily, (int)FontStyle.Bold, this.FontSize, this.ClientRectangle, stringFormat);

                //Graphics gF = Graphics.FromImage(bmp);
                //Matrix matrixTransform = new Matrix(1.0f / this.DropFactor, 0, 0, 1.0f / this.DropFactor, -(1.0f / this.DropFactor * 2), -(1.0f / this.DropFactor * 2));

                //gF.SmoothingMode = SmoothingMode.AntiAlias;
                //gF.Transform = matrixTransform;

                //Pen p = new Pen(this._currentTextColor);
                //gF.DrawPath(p, graphicsPath);
                //gF.FillPath(new SolidBrush(this._currentTextColor), graphicsPath);
                //gF.Dispose();

                //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //e.Graphics.DrawImage(bmp, ClientRectangle, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel);
                //e.Graphics.FillPath(new SolidBrush(Color.White), graphicsPath);
                //graphicsPath.Dispose();
            }

        }

        private void LoadFonts()
        {
            this._fontFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NVS");
            this._fontFilePath = Path.Combine(this._fontFolderPath, "DroidSans.ttf");

            if (!Directory.Exists(this._fontFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(this._fontFolderPath);
                } catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (!File.Exists(this._fontFilePath))
            {
                File.WriteAllBytes(this._fontFilePath, Fonts.DroidSans_Bold);
            }

            _privateFontCollection.AddFontFile(this._fontFilePath);
         
        }

        public NVSButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            this._currentBaseColor = this._primaryBaseColor;

            LoadFonts();
        }
    }
}
