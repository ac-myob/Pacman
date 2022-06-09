using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;

namespace Pacman.Business.Model;

public record GameState(
    Size Size, 
    int Lives,
    int PowerUpRemaining,
    int Round,
    Pac Pac,
    IEnumerable<Ghost> Ghosts,
    IEnumerable<Wall> Walls,
    IEnumerable<Pellet> Pellets); 
    