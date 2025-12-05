
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderAssessment
{
    enum Algorithm { BreadthFirst, DepthFirst, HillClimbing }

    internal class PathFinderFactory
    {
        public static PathFinderInterface NewPathFinder(Algorithm algorithm)
        {
            PathFinderInterface pathFinder;

            switch (algorithm)
            {
                case Algorithm.DepthFirst:
                    pathFinder = new DepthFirst(); 
                    break;
                
                case Algorithm.HillClimbing:
                    pathFinder = new HillClimbing();
                    break;

                default:
                    pathFinder = new BreadthFirst();
                    break;
            }
            return pathFinder;
        }
    }
}
