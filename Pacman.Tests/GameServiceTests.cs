using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
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
    private const string TestGame = "../../../../Pacman/Games/TestGame.txt";
    
    public GameServiceTests()
    {
        _gameService = new GameService(_mockReader.Object, _mockWriter.Object);
    }

    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectSize_WhenGivenTxtFile()
    {
        var actualGameState = _gameService.GetNewGameState(TestGame);
        
        Assert.Equal(new Size(3, 4), actualGameState.Size);
    }
    
    [Fact]
    public void GetNewGameState_ThrowsInvalidFileException_WhenFileWidthIsNotUniform()
    {
        Assert.Throws<InvalidFileException>(
            () => _gameService.GetNewGameState("../../../../Pacman/Games/JaggedFile.txt"));
    }
    
    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectPacCoordinate_WhenGivenTxtFile()
    {
        var expectedPacCoord = new Coordinate(2, 1);
        var actualGameState = _gameService.GetNewGameState(TestGame);
        
        Assert.Equal(expectedPacCoord, actualGameState.Pac.Coordinate);
    }
    
    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectWallCoordinates_WhenGivenTxtFile()
    {
        var expectedWallCoords = new[] {new Coordinate(2, 2), new Coordinate(0, 3)};
        var actualGameState = _gameService.GetNewGameState(TestGame);
        
        Assert.Equal(expectedWallCoords, actualGameState.Walls.Select(w => w.Coordinate));
    }
    
    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectRandomGhostCoordinate_WhenGivenTxtFile()
    {
        var expectedRandomGhostCoord = new Coordinate(1, 2);
        var actualGameState = _gameService.GetNewGameState(TestGame);
        var randomGhosts = actualGameState.Ghosts.Where(g => g.GetType() == typeof(RandomGhost));
        
        Assert.Contains(randomGhosts, g => g.Coordinate == expectedRandomGhostCoord);
    }
    
    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectGreedyGhostCoordinate_WhenGivenTxtFile()
    {
        var expectedGreedyGhostCoord = new Coordinate(0, 2);
        var actualGameState = _gameService.GetNewGameState(TestGame);
        var randomGhosts = actualGameState.Ghosts.Where(g => g.GetType() == typeof(GreedyGhost));
        
        Assert.Contains(randomGhosts, g => g.Coordinate == expectedGreedyGhostCoord);
    }
    
    [Fact]
    public void GetNewGameState_ReturnsGameStateWithCorrectPathFindingGhostCoordinate_WhenGivenTxtFile()
    {
        var expectedPathFindingGhostCoord = new Coordinate(1, 3);
        var actualGameState = _gameService.GetNewGameState(TestGame);
        var randomGhosts = actualGameState.Ghosts.Where(g => g.GetType() == typeof(PathFindingGhost));
        
        Assert.Contains(randomGhosts, g => g.Coordinate == expectedPathFindingGhostCoord);
    }
}
