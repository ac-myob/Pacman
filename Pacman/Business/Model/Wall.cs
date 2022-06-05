using Pacman.Variables;

namespace Pacman.Business.Model;

public record Wall(Coordinate Coordinate) : Entity(Coordinate, Constants.Wall);

