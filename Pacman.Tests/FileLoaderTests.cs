using Pacman.Business.Control;
using Pacman.Business.Control.WorldLoader;
using Pacman.Exceptions;
using Xunit;

namespace Pacman.Tests;

public class FileLoaderTests
{
    [Fact]
    public void LoadWorld_ThrowsInvalidFileException_WhenFileIsEmpty()
    {
        var fileLoad = new FileLoader("../../../../Pacman/Games/Empty.txt");

        Assert.Throws<InvalidFileException>(() => fileLoad.LoadWorld());
    }

    [Fact]
    public void LoadWorld_ThrowsInvalidFileException_WhenFileWidthIsNotUniform()
    {
        var fileLoad = new FileLoader("../../../../Pacman/Games/JaggedFile.txt");

        Assert.Throws<InvalidFileException>(() => fileLoad.LoadWorld());
    }

    [Theory]
    [InlineData("../../../../Pacman/Games/MultiplePac.txt")]
    [InlineData("../../../../Pacman/Games/NoPac.txt")]
    public void LoadWorld_ThrowsInvalidFileException_WhenFileDoesNotHaveExactlyOnePacSymbol(string filePath)
    {
        var fileLoad = new FileLoader(filePath);

        Assert.Throws<InvalidFileException>(() => fileLoad.LoadWorld());
    }
    
    [Fact]
    public void LoadWorld_Returns2DArrayOfCharactersOfMap_WhenGivenFilepath()
    {
        var fileLoad = new FileLoader("../../../../Pacman/Games/TestMap.txt");
        var expectedArray = new[,]
        {
            {'·', '·', '✪'},
            {'X', '·', '◯'},
            {'ᗣ', 'ᓆ', 'X'},
            {'▩', 'ᔬ', 'X'}
        };

        var actualArray = fileLoad.LoadWorld();
        
        Assert.Equal(expectedArray, actualArray);
    }
}
