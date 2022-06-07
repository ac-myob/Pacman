using Pacman.Variables;

namespace Pacman.Business.Model;

public record MagicPellet(Coordinate Coordinate) : Entity(Coordinate, Constants.MagicPellet);
