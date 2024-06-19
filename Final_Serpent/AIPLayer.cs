using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Final_Serpent
{
    internal class AIPlayer : Player
    {
        private Food currentFoodTarget; // The current food target the AI is moving towards
        private Food nextFoodTarget;    // The next food target after reaching currentFoodTarget

        public AIPlayer(PointF spawnPos, int initLen, float stepsize, Directions startDirection, Brush snakeBrush)
            : base(spawnPos, initLen, stepsize, startDirection, snakeBrush)
        {
            currentFoodTarget = null;
            nextFoodTarget = null;
        }

        public void UpdateDirection(List<Food> foodList, int logicalCellSize, List<Player> playerList)
        {
            if (foodList.Count == 0) return;

            // If current food target is null or has been reached, find the next food targets
            if (currentFoodTarget == null || !IsTargetFoodValid(currentFoodTarget, foodList))
            {
                currentFoodTarget = FindClosestFood(foodList);
                nextFoodTarget = FindNextFoodTarget(currentFoodTarget, foodList);
            }

            // Calculate paths to the current and next food targets
            List<PointF> pathToCurrent = FindPathToFood(currentFoodTarget.Position, logicalCellSize, playerList);
            List<PointF> pathToNext = FindPathToFood(nextFoodTarget.Position, logicalCellSize, playerList);

            // Decide the next step based on the paths
            PointF nextStep = DetermineNextStep(pathToCurrent, pathToNext);

            // Update direction if a valid next step is determined
            if (nextStep != PointF.Empty)
            {
                UpdateDirectionBasedOnNextStep(nextStep, logicalCellSize);
            }
        }

        private Food FindClosestFood(List<Food> foodList)
        {
            Food closestFood = foodList[0];
            float closestDistance = ManhattanDistance(Head, closestFood.Position);

            foreach (Food food in foodList)
            {
                float distance = ManhattanDistance(Head, food.Position);
                if (distance < closestDistance)
                {
                    closestFood = food;
                    closestDistance = distance;
                }
            }

            return closestFood;
        }

        private Food FindNextFoodTarget(Food currentFood, List<Food> foodList)
        {
            if (foodList.Count == 1) return currentFood; // If there's only one food, return the same food

            Food nextFood = foodList[0];
            float nextFoodDistance = float.MaxValue;

            foreach (Food food in foodList)
            {
                if (food != currentFood)
                {
                    float distance = ManhattanDistance(currentFood.Position, food.Position);
                    if (distance < nextFoodDistance)
                    {
                        nextFood = food;
                        nextFoodDistance = distance;
                    }
                }
            }

            return nextFood;
        }

        private float ManhattanDistance(PointF p1, PointF p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        private List<PointF> FindPathToFood(Point target, int logicalCellSize, List<Player> playerList)
        {
            Point start = new Point((int)Head.X, (int)Head.Y);
            HashSet<Point> closedSet = new HashSet<Point>();
            HashSet<Point> openSet = new HashSet<Point> { start };
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
            Dictionary<Point, float> gScore = new Dictionary<Point, float> { [start] = 0 };
            Dictionary<Point, float> fScore = new Dictionary<Point, float> { [start] = ManhattanDistance(Head, target) };

            while (openSet.Count > 0)
            {
                Point current = openSet.OrderBy(n => fScore.ContainsKey(n) ? fScore[n] : float.MaxValue).First();

                if (current == target)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Point neighbor in GetNeighbors(current, logicalCellSize, playerList))
                {
                    if (closedSet.Contains(neighbor)) continue;

                    float tentativeGScore = gScore[current] + 1;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                    else if (tentativeGScore >= gScore[neighbor]) continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + ManhattanDistance(neighbor, target);
                }
            }

            return null; // No path found
        }

        private List<Point> GetNeighbors(Point current, int logicalCellSize, List<Player> playerList)
        {
            List<Point> neighbors = new List<Point>
            {
                new Point(current.X - 1, current.Y),
                new Point(current.X + 1, current.Y),
                new Point(current.X, current.Y - 1),
                new Point(current.X, current.Y + 1)
            };

            return neighbors.Where(n => IsWalkable(n, logicalCellSize, playerList)).ToList();
        }

        private bool IsWalkable(Point point, int logicalCellSize, List<Player> playerList)
        {
            if (point.X < 0 || point.X >= logicalCellSize || point.Y < 0 || point.Y >= logicalCellSize) return false;

            foreach (Player player in playerList)
            {
                if (player.Positions.Any(p => (int)p.X == point.X && (int)p.Y == point.Y)) return false;
            }

            return true;
        }

        private List<PointF> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            List<PointF> totalPath = new List<PointF> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Insert(0, current);
            }

            return totalPath.Select(p => new PointF(p.X, p.Y)).ToList();
        }

        private PointF DetermineNextStep(List<PointF> pathToCurrent, List<PointF> pathToNext)
        {
            // Check if path to current food is valid and has more than one point
            if (pathToCurrent != null && pathToCurrent.Count > 1)
            {
                // If path to next food is also valid and shorter, consider the next food
                if (pathToNext != null && pathToNext.Count > 1 && pathToNext.Count < pathToCurrent.Count)
                {
                    return pathToNext[1]; // Return the second point in the path to next food
                }

                return pathToCurrent[1]; // Return the second point in the path to current food
            }

            return PointF.Empty; // No valid path found
        }

        private void UpdateDirectionBasedOnNextStep(PointF nextStep, int logicalCellSize)
        {
            Directions nextDirection = DetermineDirectionTowards(nextStep, logicalCellSize);

            // Only change direction if the head is aligned with the grid and the direction is valid
            if (IsAlignedWithGrid(Head, logicalCellSize) && !IsOppositeDirection(nextDirection))
            {
                TempDirection = nextDirection;
            }
        }

        private Directions DetermineDirectionTowards(PointF target, int logicalCellSize)
        {
            if (Head.X < target.X)
                return Directions.Right;
            else if (Head.X > target.X)
                return Directions.Left;
            else if (Head.Y < target.Y)
                return Directions.Down;
            else
                return Directions.Up;
        }

        private bool IsAlignedWithGrid(PointF position, int logicalCellSize)
        {
            return (position.X * logicalCellSize) % logicalCellSize == 0 && (position.Y * logicalCellSize) % logicalCellSize == 0;
        }

        private bool IsOppositeDirection(Directions newDirection)
        {
            return (Direction == Directions.Left && newDirection == Directions.Right) ||
                   (Direction == Directions.Right && newDirection == Directions.Left) ||
                   (Direction == Directions.Up && newDirection == Directions.Down) ||
                   (Direction == Directions.Down && newDirection == Directions.Up);
        }

        private bool IsTargetFoodValid(Food targetFood, List<Food> foodList)
        {
            return foodList.Contains(targetFood);
        }
    }
}