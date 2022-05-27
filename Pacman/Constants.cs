namespace Pacman;

public static class Constants
{
    public const string UpKey = "UpArrow";
    public const string DownKey = "DownArrow";
    public const string LeftKey = "LeftArrow";
    public const string RightKey = "RightArrow";
    public const string ValidKeysRegex = $"^({UpKey}|{DownKey}|{LeftKey}|{RightKey})$";
}
