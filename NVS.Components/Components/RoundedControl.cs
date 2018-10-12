using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NVS.Components
{
    public class RoundedControl : Control
    {
        private Color _primaryBaseColor = ColorTranslator.FromHtml("#212020");

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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rect = new RectangleF(0, 0, this.Bounds.Width, this.Bounds.Height);
            GraphicsPath gPath = GetRoundedPath(rect, 25);

            this.Region = new Region(gPath);
            using (Pen p = new Pen(this._primaryBaseColor, 1.75f))
            {
                p.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(p, gPath);
            }
        }
    }
}
