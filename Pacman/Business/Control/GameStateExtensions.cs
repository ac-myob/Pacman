using System.Text;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public static class GameStateExtensions
{
    public static string GetString(this GameState gameState)
    {
        var (width, length) = gameState.Size;
        var res = new StringBuilder(new string(Constants.Blank, length * width));
        var entities = gameState.GetPellets().Cast<IEntity>().
            Concat(gameState.Walls.Values).
            Append(gameState.Pac).
            Concat(gameState.Ghosts).
            OrderBy(o => o.Coordinate.Y).
            ThenBy(o => o.Coordinate.X).
            ToArray();

        foreach (var entity in entities)
            res[entity.Coordinate.GetRank(width)] = entity.Symbol;

        for (var l = 0; l < length; l++)
            res.Insert(width * (l + 1) + l, Environment.NewLine);

        return res.ToString();
    }
    
    public static IEnumerable<Pellet> GetPellets(this GameState gameState) =>
        gameState.Pellets.Where(p => !p.Eaten);
}
