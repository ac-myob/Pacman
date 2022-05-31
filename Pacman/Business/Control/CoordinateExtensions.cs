using Pacman.Business.Model;

namespace Pacman.Business.Control;

public static class CoordinateExtensions
{
    public static int GetRank(this Coordinate coordinate, int width)
    {
        var (x, y) = coordinate;
        return x + y * width;
    }
}