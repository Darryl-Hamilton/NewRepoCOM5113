
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderAssessment
{
    internal class BreadthFirst : PathFinderInterface
    {
        public bool FindPath(int[,] map, Coord start, Coord goal, ref LinkedList<Coord> path)
        {
            var open = new Queue<SearchNode>();
            var closed = new Queue<SearchNode>();

            // Enqueue start node
            open.Enqueue(new SearchNode(start));
            SearchNode current;

            while (!open.IsEmpty())
            {
                // Dequeue first item (FIFO)
                current = open.Dequeue();

                // Goal test
                if (current.Position.Row == goal.Row &&
                    current.Position.Col == goal.Col)
                {
                    path = SearchUtilities.buildPathList(current);
                    return true;
                }

                // Add to closed list
                closed.Enqueue(current);

                int r = current.Position.Row;
                int c = current.Position.Col;

                // Neighbours in order: N, E, S, W
                Coord[] neighbours =
                {
                    new Coord(r - 1, c),
                    new Coord(r, c + 1),
                    new Coord(r + 1, c),
                    new Coord(r, c - 1)
                };

                foreach (var n in neighbours)
                {
                    // Bounds check
                    if (n.Row < 0 || n.Col < 0 ||
                        n.Row >= map.GetLength(0) ||
                        n.Col >= map.GetLength(1))
                        continue;

                    // Wall check
                    if (map[n.Row, n.Col] == 0)
                        continue;

                    // Closed list check
                    bool inClosed = false;
                    Queue<SearchNode> temp = new Queue<SearchNode>();

                    while (!closed.IsEmpty())
                    {
                        var x = closed.Dequeue();
                        if (x.Position.Row == n.Row &&
                            x.Position.Col == n.Col)
                            inClosed = true;

                        temp.Enqueue(x);
                    }

                    while (!temp.IsEmpty())
                        closed.Enqueue(temp.Dequeue());

                    if (inClosed)
                        continue;

                    // Enqueue unexplored neighbour
                    open.Enqueue(
                        new SearchNode(n, 0, 0, current)
                    );
                }
            }

            return false; // no path found
        }
    }
}


        