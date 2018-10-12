using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NVS.Components
{
    public class NVSThumbnail : RoundedControl
    {
        private readonly Color _primaryBaseColor = ColorTranslator.FromHtml("#212020");
        private readonly Color _onHighlightColor = ColorTranslator.FromHtml("#02bcf2");

        private Color _currentBaseColor;
        private Color _currentHighlightColor;

        private readonly PictureBox pictureBox;
        
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this._currentBaseColor = ColorTranslator.FromHtml("#2D2D2D");
            this._currentHighlightColor = this._onHighlightColor;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this._currentBaseColor = this._primaryBaseColor;
            this._currentHighlightColor = Color.Black;
            Invalidate();
        }

        public void LoadImageThumbnail(String filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            this.pictureBox.Image = Image.FromFile(filePath);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            this.BackColor = this._currentBaseColor;

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                this._currentHighlightColor, 8, ButtonBorderStyle.Solid, // left
                this._currentHighlightColor, 8, ButtonBorderStyle.Solid, // top
                this._currentHighlightColor, 8, ButtonBorderStyle.Solid, // right
                this._currentHighlightColor, 20, ButtonBorderStyle.Solid);    // bottom
            
        }

        public NVSThumbnail()
        {
            this._currentHighlightColor = Color.Black;
            this._currentBaseColor = this._primaryBaseColor;

            this.pictureBox = new PictureBox()
            {
                Name = "thumbNailBox",
                Size = new Size(this.ClientRectangle.Size.Width - 16, this.ClientRectangle.Size.Height - 28),
                Location = new Point(8, 8)
            };

            this.pictureBox.MouseEnter += pictureBoxMouseEnter;
            this.pictureBox.MouseLeave += pictureBoxMouseLeave;
            
            this.Controls.Add(this.pictureBox);
        }

        private void pictureBoxMouseLeave(object sender, EventArgs e)
        {
            this._currentBaseColor = this._primaryBaseColor;
            this._currentHighlightColor = Color.Black;
            Invalidate();
        }

        private void pictureBoxMouseEnter(object sender, EventArgs e)
        {
            this._currentBaseColor = ColorTranslator.FromHtml("#2D2D2D");
            this._currentHighlightColor = this._onHighlightColor;
            Invalidate();
        }
    }
}
