using Pacman.Exceptions;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class FileLoader : IWorldLoader
{
    private readonly string _filepath;
    
    public FileLoader(string filepath)
    {
        _filepath = filepath;
    }
    
    public char[,] LoadWorld()
    {
        var fileLines = File.ReadAllLines(_filepath);
        if (!fileLines.Any())
            throw new InvalidFileException("File is empty.");
        
        var length = fileLines.Length;
        var width = fileLines.First().Length;

        if (fileLines.Any(line => line.Length != width))
            throw new InvalidFileException("File width must be uniform.");

        var world = new char[length, width];
        var pacCount = 0;
        
        for (var l = 0; l < length; l++)
            for (var w = 0; w < width; w++)
            {
                var symbol = fileLines[l][w];
                world[l, w] = symbol;
                pacCount += symbol == Constants.PacStart ? 1 : 0;
            }

        return pacCount != 1
            ? throw new InvalidFileException($"Expected exactly 1 Pacman symbol but {pacCount} was found.")
            : world;
    }
}
