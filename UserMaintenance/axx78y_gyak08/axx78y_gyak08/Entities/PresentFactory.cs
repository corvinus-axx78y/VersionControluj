using axx78y_gyak08.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axx78y_gyak08.Entities
{
    public class PresentFactory : IToyFactory
    {
        public Color ribbon { get; set; }
        public Color box { get; set; }
        public Toy CreateNew()
        {
            return new Present(ribbon, box);
        }
    }
}
