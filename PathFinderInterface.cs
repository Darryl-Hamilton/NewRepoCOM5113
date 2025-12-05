
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderAssessment
{
    using Grid = int[,];

  
    internal interface PathFinderInterface
    {
        
        public bool FindPath(Grid map, Coord start, Coord goal, ref LinkedList<Coord> path);

        
    }
}
