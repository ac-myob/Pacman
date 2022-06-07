using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;

namespace Pacman.Business.Model;

public record GameState(
    Size Size, 
    int Round,
    Pac Pac,
    IEnumerable<BaseGhost> Ghosts,
    IEnumerable<Wall> Walls,
    IEnumerable<Pellet> Pellets); 