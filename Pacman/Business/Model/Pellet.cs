using Pacman.Variables;

namespace Pacman.Business.Model;

public record Pellet(Coordinate Coordinate) : Entity(Coordinate, Constants.Pellet);
