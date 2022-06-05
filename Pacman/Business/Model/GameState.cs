using Pacman.Business.Control;

namespace Pacman.Business.Model;

public record GameState(
    Size Size, 
    int Round,
    Pac Pac,
    IEnumerable<MovableEntity> Ghosts,
    IEnumerable<Wall> Walls,
    IEnumerable<Pellet> Pellets); 