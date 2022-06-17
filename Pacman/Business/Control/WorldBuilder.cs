using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class WorldBuilder
{
    private readonly IWorldLoader _worldLoader;
    private readonly GhostFactory _ghostFactory;

    public WorldBuilder(IWorldLoader worldLoader, GhostFactory ghostFactory)
    {
        _worldLoader = worldLoader;
        _ghostFactory = ghostFactory;
    }

    public (Size, IEnumerable<Entity>) Build()
    {
        var world = _worldLoader.LoadWorld();
        var worldSize = new Size(world.GetLength(1), world.GetLength(0));
        var pac = new Pac(_getPacCoord(world, worldSize), Constants.PacStart);
        var worldEntities = new List<Entity>{pac};

        for (var l = 0; l < worldSize.Length; l++)
            for (var w = 0; w < worldSize.Width; w++)
            {
                var currentCoord = new Coordinate(w, l);
                Entity? currentEntity = world[l, w] switch
                {
                    Constants.Wall => new Wall(currentCoord),
                    Constants.Pellet => new Pellet(currentCoord, Constants.Pellet),
                    Constants.MagicPellet => new Pellet(currentCoord, Constants.MagicPellet),
                    Constants.RandomGhost => _ghostFactory.GetGhost(GhostType.Random, currentCoord),
                    Constants.GreedyGhost => _ghostFactory.GetGhost(GhostType.Greedy, currentCoord),
                    Constants.PathFindingGhost => _ghostFactory.GetGhost(GhostType.PathFinding, currentCoord),
                    _ => null
                };

                if (currentEntity is not null)
                    worldEntities.Add(currentEntity);
            }

        return (worldSize, worldEntities);
    }

    private static Coordinate _getPacCoord(char[,] world, Size size)
    {
        var (width, length) = size;

        for (var l = 0; l < length; l++)
            for (var w = 0; w < width; w++)
                if (world[l, w] == Constants.PacStart)
                    return new Coordinate(w, l);

        return new Coordinate();
    }
}
