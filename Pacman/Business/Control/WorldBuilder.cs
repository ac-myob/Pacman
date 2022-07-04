using Pacman.Business.Control.WorldLoader;
using Pacman.Business.Model;
using Pacman.Business.Model.Ghosts;
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
                    Constants.WallHorz => new Wall(currentCoord, Constants.WallHorz),
                    Constants.WallVert => new Wall(currentCoord, Constants.WallVert),
                    Constants.WallBottomLeft => new Wall(currentCoord, Constants.WallBottomLeft),
                    Constants.WallBottomRight => new Wall(currentCoord, Constants.WallBottomRight),
                    Constants.WallTopLeft => new Wall(currentCoord, Constants.WallTopLeft),
                    Constants.WallTopRight => new Wall(currentCoord, Constants.WallTopRight),
                    Constants.WallTUp => new Wall(currentCoord, Constants.WallTUp),
                    Constants.WallTDown => new Wall(currentCoord, Constants.WallTDown),
                    Constants.WallTLeft => new Wall(currentCoord, Constants.WallTLeft),
                    Constants.WallTRight => new Wall(currentCoord, Constants.WallTRight),
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
