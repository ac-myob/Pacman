using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests;

public class GameStateModifierTests
{
    private readonly GameState _gameState = new(
        It.IsAny<Size>(),
        Constants.PacStartingLives,
        It.IsAny<int>(),
        It.IsAny<Pac>(),
        It.IsAny<IEnumerable<BaseGhost>>(),
        Array.Empty<Wall>(),
        Array.Empty<Pellet>(),
        Array.Empty<MagicPellet>()
    );
    private readonly Pac _pac = new(
        It.IsAny<Coordinate>(), 
        Constants.PacStart, 
        It.IsAny<int>(),
        It.IsAny<IReader>(), 
        It.IsAny<IWriter>()
    );
    
    [Fact]
    public void UpdatePellets_ReturnsGameStateWithPelletsTraversedByPacmanRemoved_WhenGhostNotOnPellet()
    {
        var pacCoord = new Coordinate(0, 0);
        var ghostCoord = new Coordinate(1, 0);
        var gameState = _gameState with
        {
            Pac = _pac with { Coordinate = pacCoord },
            Ghosts = new BaseGhost[] { new GreedyGhost(ghostCoord, It.IsAny<int>()) },
            Pellets = new Pellet[] {new(pacCoord), new(ghostCoord)}
        };

        var actualGameState = gameState.UpdatePellets();
        
        Assert.Equal(new Pellet[] {new(ghostCoord)}, actualGameState.Pellets);
    }
    
    [Fact]
    public void UpdatePellets_ReturnsOriginalGameState_WhenTraversingPelletOccupiedByAGhost()
    {
        var coord = new Coordinate(0, 0);
        var gameState = _gameState with
        {
            Pac = _pac with { Coordinate = coord },
            Ghosts = new BaseGhost[] { new GreedyGhost(coord, It.IsAny<int>()) },
            Pellets = new Pellet[] {new(coord)}
        };

        var actualGameState = gameState.UpdatePellets();
        
        Assert.Equal(new Pellet[] {new(coord)}, actualGameState.Pellets);
    }
    
    [Fact]
    public void UpdateGhostCoordinate_ReturnsGameStateWithUpdatedGhostCoordinate_GivenIdAndNewCoordinate()
    {
        var newCoord = new Coordinate(4, 4);
        var ghosts = new BaseGhost[]
        {
            new RandomGhost(new Coordinate(0, 0), 0, It.IsAny<ISelector<Coordinate>>()),
            new GreedyGhost(new Coordinate(0, 3), 1),
            new PathFindingGhost(new Coordinate(2, 3), 2)
        };
        var gameState = _gameState with {Ghosts = ghosts};
        var expectedCoords = new[] {newCoord, new(0, 3), new(2, 3)};

        var actualGameState = gameState.UpdateGhostCoordinate(0, newCoord);
        var actualCoords = actualGameState.Ghosts.Select(g => g.Coordinate);
        
        Assert.Equal(expectedCoords, actualCoords);
    }
}