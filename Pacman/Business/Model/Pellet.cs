namespace Pacman.Business.Model;

public record Pellet(Coordinate Coordinate, char Symbol) : Entity(Coordinate, Symbol);
