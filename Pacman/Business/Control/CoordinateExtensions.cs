using Pacman.Business.Model;

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
}