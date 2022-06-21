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
    Green,
    Cyan
}

public enum GameStatus
{
    Running,
    RoundComplete,
    Collided,
    GameComplete
}
