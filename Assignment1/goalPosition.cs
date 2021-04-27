using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class goalPosition
    {
        private List<string> directions;
        private Tuple<int, int> position;

        public goalPosition(List<string> d, Tuple<int, int> p)
        {
            this.directions = d;
            this.position = p;
        }
    }
}
