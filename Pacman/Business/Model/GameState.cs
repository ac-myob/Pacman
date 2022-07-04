using Pacman.Business.Control;
using Pacman.Business.Model.Ghosts;
using Pacman.Variables;

namespace Pacman.Business.Model;

public record GameState(Size Size,
    int Round,
    GameStatus GameStatus,
    Pac Pac,
    IEnumerable<Ghost> Ghosts,
    IReadOnlyDictionary<Coordinate, Wall> Walls,
    IEnumerable<Pellet> Pellets);
