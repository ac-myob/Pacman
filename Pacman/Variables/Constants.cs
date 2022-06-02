namespace Pacman.Variables;

public static class Constants
{
    public const string UpKey = "UpArrow";
    public const string DownKey = "DownArrow";
    public const string LeftKey = "LeftArrow";
    public const string RightKey = "RightArrow";
    public const string ValidKeysRegex = $"^({UpKey}|{DownKey}|{LeftKey}|{RightKey})$";
    public const char Wall = '▩';
    public const char Pellet = '•';
    public const char GreedyGhost = 'ᗣ';
    public const char RandomGhost = 'ᓆ';
    public const char PathFindingGhost = 'ᔬ';
    public const char PacStart = '◯';
    public const char Blank = ' ';
    public const string GameFilepath = "../../../../Pacman/Games/GameMap.txt";
    public const char PacUp = 'V';
    public const char PacDown = '^';
    public const char PacLeft = '>';
    public const char PacRight = '<';
    public const int PacStartingLives = 3;
}
