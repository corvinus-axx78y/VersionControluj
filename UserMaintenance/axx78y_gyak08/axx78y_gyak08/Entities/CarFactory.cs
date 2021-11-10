using axx78y_gyak08.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axx78y_gyak08.Entities
{
    public class CarFactory : IToyFactory
    {
        public Toy CreateNew()
        {
            return new Car();
        }
    }
}
