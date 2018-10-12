using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NVS.Extensions;
using System.ComponentModel;
using System.Drawing.Text;
using System.IO;

namespace NVS.Components
{
    /// <summary>
    /// Represents a custom-designed button.
    /// </summary>
    public class NVSButton : Button
    {

        private PrivateFontCollection _privateFontCollection = new PrivateFontCollection();
        private string _fontFolderPath;
        private string _fontFilePath;
        private Color _primaryBaseColor = ColorTranslator.FromHtml("#212020");
        private Color _currentBaseColor;

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
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this._currentBaseColor = this._primaryBaseColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rect = new RectangleF(0, 0, this.Bounds.Width, this.Bounds.Height);
            GraphicsPath gPath = GetRoundedPath(rect, 25);

            this.Region = new Region(gPath);
            using(Pen p = new Pen(this._currentBaseColor, 1.75f))
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
                Color.Black, 10, ButtonBorderStyle.Inset);    // bottom

            FontFamily fFamily = _privateFontCollection.Families[0];
            Font font = new Font(fFamily.Name, this.FontSize);
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Center;

            e.Graphics.DrawString(this.Text.ToUpper(), font, new SolidBrush(Color.White), this.ClientRectangle, stringFormat);
            
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
