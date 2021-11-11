using axx78y_gyak08.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axx78y_gyak08.Entities
{
    public class Present : Toy
    {
        public SolidBrush ribbon { get; private set; }
        public SolidBrush box { get; private set; }

        public Present(Color szalag, Color doboz)
        {
            ribbon = new SolidBrush(szalag);
            box = new SolidBrush(doboz);
        }

        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(box, 0, 0, Width, Height);
            g.FillRectangle(ribbon, 0, Height / 5*2, Width, Height / 5);
            g.FillRectangle(ribbon, Width / 5*2, 0, Width / 5, Height);
        }
    }
}
