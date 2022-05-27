using Pacman.Business.Control;

namespace Pacman.Business.Model;

public class GameState
{
    public Size Size { get; }
    public Pac Pac { get; }
    
    public IEnumerable<Entity> Walls { get; }
    public IEnumerable<Entity> Ghosts { get; }
    
    public GameState(Size size, Pac pac, IEnumerable<Wall> walls, IEnumerable<Entity> ghosts)
    {
        Size = size;
        Pac = pac;
        Walls = walls;
        Ghosts = ghosts;
    }
}