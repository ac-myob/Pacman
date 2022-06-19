namespace Pacman.Variables;

public enum Direction
{
    North,
    South,
    East,
    West
}

public enum GhostType
{
    Random,
    Greedy,
    PathFinding
}

public enum Colour
{
    Red,
    Blue,
    Yellow,
    Green
}

public enum GameStatus
{
    Running,
    RoundComplete,
    Collided,
    GameComplete
}
