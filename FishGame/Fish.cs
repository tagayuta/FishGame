using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame
{
    internal class Fish
    {
        private string name;
        private int point;

        public Fish(string name, int point)
        {
            this.name = name;
            this.point = point;
        }
        
        public string Name { get { return name; } }
        public int Point { get { return point; } }
    }
}
