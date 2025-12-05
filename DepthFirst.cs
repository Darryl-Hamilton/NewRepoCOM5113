using System;
using System.Collections.Generic;

namespace PathFinderAssessment
{
    internal class DepthFirst : PathFinderInterface
    {
        public bool FindPath(int[,] map, Coord start, Coord goal, ref LinkedList<Coord> path)
        {
            var open = new Stack<SearchNode>();
            var closed = new Queue<SearchNode>();

            // Push start node
            open.Push(new SearchNode(start));
            SearchNode current;

            while (!open.IsEmpty())
            {
                // Pop top item 
                current = open.Pop();

                // Goal test
                if (current.Position.Row == goal.Row &&
                    current.Position.Col == goal.Col)
                {
                    path = SearchUtilities.buildPathList(current);
                    return true;
                }

                // Add to closed
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

                    // Push neighbour (backtracking via stack)
                    open.Push(
                        new SearchNode(n, 0, 0, current)
                    );
                }
            }

            return false; // no path found
        }
    }
}
