using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public static class CoordinateExtensions
{
    public static int GetRank(this Coordinate coordinate, int width)
    {
        var (x, y) = coordinate;
        return x + y * width;
    }

    public static double GetDistance(this Coordinate coord1, Coordinate coord2)
    {
        var (x1, y1) = coord1;
        var (x2, y2) = coord2;
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }

    public static Coordinate Shift(this Coordinate coordinate, Direction direction, Size size)
    {
        var (width, length) = size;
        return direction switch
        {
            Direction.North => coordinate with {Y = Utilities.Mod(coordinate.Y - 1, length)},
            Direction.South => coordinate with {Y = Utilities.Mod(coordinate.Y + 1, length)},
            Direction.East => coordinate with {X = Utilities.Mod(coordinate.X + 1, width)},
            Direction.West => coordinate with {X = Utilities.Mod(coordinate.X - 1, width)},
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}
