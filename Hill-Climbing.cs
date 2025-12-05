using System;
using System.Collections.Generic;
using System.Text;

namespace PathFinderAssessment
{
    internal class HillClimbing : PathFinderInterface
    {
        // Depth limit to prevent infinite descent
        private const int MAX_DEPTH = 50;

        public bool FindPath(int[,] map, Coord start, Coord goal, ref LinkedList<Coord> path)
        {
            // Start recursive depth-limited hill climb
            SearchNode startNode = new SearchNode(
                start,
                0,
                SearchUtilities.ManhattanDistance(start, goal),
                null
            );

            bool success = HillClimbRecursive(map, startNode, goal, 0);

            if (success)
            {
                path = SearchUtilities.buildPathList(_goalNode);
                return true;
            }

            return false;
        }

        // Stores final goal node once found
        private SearchNode? _goalNode = null;

        // ================================
        // DEPTH-LIMITED HILL CLIMB WITH BACKTRACKING
        // ================================
        private bool HillClimbRecursive(int[,] map,
                                        SearchNode current,
                                        Coord goal,
                                        int depth)
        {
            // Depth limit reached → fail
            if (depth > MAX_DEPTH)
                return false;

            // Goal test
            if (current.Position.Row == goal.Row &&
                current.Position.Col == goal.Col)
            {
                _goalNode = current;
                return true;
            }

            int r = current.Position.Row;
            int c = current.Position.Col;

            // Generate neighbours (N, E, S, W)
            SearchNode[] successors = new SearchNode[4];
            int count = 0;

            Coord[] neighbours =
            {
                new Coord(r - 1, c), // North
                new Coord(r, c + 1), // East
                new Coord(r + 1, c), // South
                new Coord(r, c - 1)  // West
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

                int h = SearchUtilities.ManhattanDistance(n, goal);

                successors[count++] = new SearchNode(
                    n,
                    0,
                    h,
                    current
                );
            }

            // Sort successors by heuristic (lowest first)
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (successors[j].Score < successors[i].Score)
                    {
                        var temp = successors[i];
                        successors[i] = successors[j];
                        successors[j] = temp;
                    }
                }
            }

            // Try each neighbour in heuristic order (BACKTRACKING)
            for (int i = 0; i < count; i++)
            {
                // Only move if heuristic improves or stays equal
                if (successors[i].Score <= current.Score)
                {
                    bool found = HillClimbRecursive(
                        map,
                        successors[i],
                        goal,
                        depth + 1
                    );

                    if (found)
                        return true; // propagate success up the call stack
                }
            }

            // All options exhausted → BACKTRACK
            return false;
        }
    }
}
