using Pacman.Business.Control;

namespace Pacman.Business.Model;

public class GameState
{
    public Size Size { get; }
    public Pac Pac { get; }
    public IEnumerable<Entity> Walls { get; }
    public IDictionary<Coordinate, Pellet> Pellets { get; }
    public IEnumerable<MovableEntity> Ghosts { get; }
    
    public GameState(Size size, Pac pac, IEnumerable<Wall> walls, IDictionary<Coordinate, Pellet> pellets, IEnumerable<MovableEntity> ghosts)
    {
        Size = size;
        Pac = pac;
        Walls = walls;
        Pellets = pellets;
        Ghosts = ghosts;
    }
}
