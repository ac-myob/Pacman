namespace Pacman.Business.Model;

public abstract record MovableEntity(Coordinate Coordinate, char Symbol, int Id) : Entity(Coordinate, Symbol)
{
    public int Id { get; } = Id;
    public abstract GameState Move(GameState gameState);
}
