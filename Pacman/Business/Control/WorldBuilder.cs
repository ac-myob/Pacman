using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.WorldLoader;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class WorldBuilder
{
    private readonly char[,] _world;
    private readonly GhostFactory _ghostFactory;

    public WorldBuilder(IWorldLoader worldLoader, GhostFactory ghostFactory)
    {
        _world = worldLoader.LoadWorld();
        _ghostFactory = ghostFactory;
    }

    public IEnumerable<IEntity> GetEntities()
    {
        var width = _world.GetLength(1);
        var length = _world.GetLength(0);
        var worldEntities = new List<IEntity>();

        for (var l = 0; l < length; l++)
            for (var w = 0; w < width; w++)
            {
                var currentCoord = new Coordinate(w, l);
                IEntity? currentEntity = _world[l, w] switch
                {
                    Constants.Wall => new Wall(currentCoord),
                    Constants.Pellet => new Pellet(currentCoord, Constants.Pellet),
                    Constants.MagicPellet => new Pellet(currentCoord, Constants.MagicPellet),
                    Constants.RandomGhost => _ghostFactory.GetGhost(GhostType.Random, currentCoord),
                    Constants.GreedyGhost => _ghostFactory.GetGhost(GhostType.Greedy, currentCoord),
                    Constants.PathFindingGhost => _ghostFactory.GetGhost(GhostType.PathFinding, currentCoord),
                    Constants.PacStart => new Pac(currentCoord, Constants.PacStart, Constants.PacStartingLives),
                    _ => null
                };

                if (currentEntity is not null)
                    worldEntities.Add(currentEntity);
            }

        return worldEntities;
    }
}
