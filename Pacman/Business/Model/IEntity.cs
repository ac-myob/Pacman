namespace Pacman.Business.Model;

public interface IEntity
{
    Coordinate Coordinate { get; }
    char Symbol { get; }
}