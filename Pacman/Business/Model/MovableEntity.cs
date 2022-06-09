using Pacman.Business.Control.MoveStrategies;

namespace Pacman.Business.Model;

public abstract record MovableEntity(Coordinate Coordinate, char Symbol, int Id, IMoveStrategy MoveStrategy) : 
    Entity(Coordinate, Symbol)
{ 
    public abstract GameState PlayTurn(GameState gameState);
}
