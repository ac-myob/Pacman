using Pacman.Business.Control;
using Pacman.Business.Model;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests;

public class GameStateModifierTests
{
    [Fact]
    public void UpdatePellets_ReturnsGameStateWithPelletsTraversedByPacmanRemoved_WhenGhostNotOnPellet()
    {
        var pacCoord = new Coordinate(0, 0);
        var ghostCoord = new Coordinate(1, 0);
        var gameState = TestHelper.GetGameState() with
        {
            Pac = TestHelper.GetPac() with { Coordinate = pacCoord },
            Ghosts = new[] { TestHelper.GetGhost() with{Coordinate = ghostCoord} },
            Pellets = new Pellet[] {new(pacCoord, Constants.Pellet), new(ghostCoord, Constants.Pellet)}
        };

        var actualGameState = gameState.UpdatePellets();
        
        Assert.Equal(new Pellet[] {new(ghostCoord, Constants.Pellet)}, actualGameState.Pellets);
    }
    
    [Fact]
    public void UpdatePellets_ReturnsOriginalGameState_WhenTraversingPelletOccupiedByAGhost()
    {
        var coord = new Coordinate(0, 0);
        var gameState = TestHelper.GetGameState() with
        {
            Pac = TestHelper.GetPac() with { Coordinate = coord },
            Ghosts = new[] { TestHelper.GetGhost() with{Coordinate = coord} },
            Pellets = new Pellet[] {new(coord, Constants.Pellet)}
        };

        var actualGameState = gameState.UpdatePellets();
        
        Assert.Equal(new Pellet[] {new(coord, Constants.Pellet)}, actualGameState.Pellets);
    }

    [Fact]
    public void UpdatePowerUp_DecrementsRemainingPowerUpByOne_WhenRemainingPowerUpGreaterThanZero()
    {
        var gameState = TestHelper.GetGameState() with {PowerUpRemaining = Constants.PowerUpTurns};

        var actualGameState = gameState.UpdatePowerUp();
        
        Assert.Equal(gameState.PowerUpRemaining - 1, actualGameState.PowerUpRemaining);
    }
    
    [Fact]
    public void UpdatePowerUp_SetsPowerUpRemainingToTen_WhenPacTraversesMagicPellet()
    {
        var coordinate = new Coordinate(0, 0);
        var gameState = TestHelper.GetGameState() with
        {
            Pac = TestHelper.GetPac() with{Coordinate = coordinate},
            Pellets = new[] {new Pellet(coordinate, Constants.MagicPellet)}
        };

        var actualGameState = gameState.UpdatePowerUp();
        
        Assert.Equal(Constants.PowerUpTurns, actualGameState.PowerUpRemaining);
    }
}
