using Moq;
using Pacman.Business.Control;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Exceptions;
using Xunit;

namespace Pacman.Tests;

public class GameServiceTests
{
    private readonly Mock<IReader> _mockReader = new();
    private readonly Mock<IWriter> _mockWriter = new();
    private readonly GameService _gameService;
    
    public GameServiceTests()
    {
        _gameService = new GameService(_mockReader.Object, _mockWriter.Object);
    }

    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectSize_WhenGivenTxtFile()
    {
        var actualGameState = _gameService.GetNewGameState("../../../../Pacman/Games/TestGame1.txt");
        
        Assert.Equal(new Size(3, 4), actualGameState.Size);
    }
    
    [Fact]
    public void GetNewGameState_ThrowsInvalidFileException_WhenFileWidthIsNotUniform()
    {
        Assert.Throws<InvalidFileException>(
            () => _gameService.GetNewGameState("../../../../Pacman/Games/TestGame2.txt"));
    }
}