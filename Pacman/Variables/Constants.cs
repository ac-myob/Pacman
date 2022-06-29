namespace Pacman.Variables;

public static class Constants
{
    public const string UpKey = "UpArrow";
    public const string DownKey = "DownArrow";
    public const string LeftKey = "LeftArrow";
    public const string RightKey = "RightArrow";
    public const string ValidKeysRegex = $"^({UpKey}|{DownKey}|{LeftKey}|{RightKey})$";
    public const char WallVert = '║';
    public const char WallHorz = '═';
    public const char WallTopLeft = '╔';
    public const char WallTopRight = '╗';
    public const char WallBottomLeft = '╚';
    public const char WallBottomRight = '╝';
    public const char WallTUp = '╦';
    public const char WallTDown = '╩';
    public const char WallTLeft = '╣';
    public const char WallTRight = '╠';
    public const char Pellet = '·';
    public const char MagicPellet = '✪';
    public const char GreedyGhost = 'ᗣ';
    public const char RandomGhost = 'ᓆ';
    public const char PathFindingGhost = 'ᔬ';
    public const char FleeGhost = 'ᐵ';
    public const char PacStart = '◯';
    public const char Heart = '♥';
    public const char Blank = ' ';
    public const string GameFilepath = "../../../../Pacman/Games/GameMap.txt";
    public const char PacUp = 'V';
    public const char PacDown = '^';
    public const char PacLeft = '>';
    public const char PacRight = '<';
    public const char PacVert = '|';
    public const char PacHorz = '—';
    public const int PacStartingLives = 6;
    public const int StartRound = 1;
    public const int MaxRounds = 4;
    public const int PowerUpTurns = 10;
}
