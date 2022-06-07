using Pacman.Business.Model;

namespace Pacman.Business.Control.Ghosts;

public abstract record BaseGhost(Coordinate Coordinate, char Symbol, int Id) : MovableEntity(Coordinate, Symbol, Id);