using System;
using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests;

public class GameStateQueryTests
{
    private readonly GameState _gameState = new(
        It.IsAny<Size>(),
        It.IsAny<int>(),
        It.IsAny<Pac>(),
        It.IsAny<IEnumerable<BaseGhost>>(),
        Array.Empty<Wall>(),
        Array.Empty<Pellet>()
    );
    private readonly Pac _pac = new(
        It.IsAny<Coordinate>(), 
        Constants.PacStart, 
        It.IsAny<int>(), 
        It.IsAny<int>(),
        It.IsAny<IReader>(), 
        It.IsAny<IWriter>()
    );
    
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesDoNotShareSameCoordinate()
    {
        var ghosts = new BaseGhost[] 
        {
            new RandomGhost(new Coordinate(0, 2), It.IsAny<int>(), It.IsAny<ISelector<Coordinate>>()),
            new GreedyGhost(new Coordinate(2, 0), It.IsAny<int>()),
            new PathFindingGhost(new Coordinate(2, 2), It.IsAny<int>())
        };
        var gameState = _gameState with
        {
            Size = new Size(3, 3),
            Pac = _pac with {Coordinate = new Coordinate(0, 0)},
            Ghosts = ghosts,
            Walls = new Wall[] {new(new Coordinate(1, 1))}
        };

        var expectedString = $"{Constants.PacStart}{Constants.Blank}{Constants.GreedyGhost}\n" +
                             $"{Constants.Blank}{Constants.Wall}{Constants.Blank}\n" +
                             $"{Constants.RandomGhost}{Constants.Blank}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }
    
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesShareSameCoordinate()
    {
        var ghosts = new BaseGhost[] 
        {
            new RandomGhost(new Coordinate(0, 2), It.IsAny<int>(), It.IsAny<ISelector<Coordinate>>()),
            new GreedyGhost(new Coordinate(2, 0), It.IsAny<int>()),
            new PathFindingGhost(new Coordinate(2, 2), It.IsAny<int>())
        };
        var pellets = new List<Pellet>();
        for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                pellets.Add(new Pellet(new Coordinate(i, j)));
        var gameState = _gameState with
        {
            Size = new Size(3, 3),
            Pac = _pac with {Coordinate = new Coordinate(0, 0)},
            Ghosts = ghosts,
            Walls = new Wall[] {new(new Coordinate(1, 1))},
            Pellets = pellets
        };

        var expectedString = $"{Constants.PacStart}{Constants.Pellet}{Constants.GreedyGhost}\n" +
                             $"{Constants.Pellet}{Constants.Wall}{Constants.Pellet}\n" +
                             $"{Constants.RandomGhost}{Constants.Pellet}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }

    [Theory]
    [MemberData(nameof(IsPacOnGhostTestData))]
    public void IsPacOnGhost_ReturnsTrueIfPacHasSameCoordinateAsAGhost(
        Coordinate pacCoord, Coordinate ghostCoord, bool expectedBool)
    {
        var gameState = _gameState with
        {
            Pac = _pac with {Coordinate = pacCoord},
            Ghosts = new BaseGhost[] {new GreedyGhost(ghostCoord, It.IsAny<int>())}
        };

        var actualBool = gameState.IsPacOnGhost();

        Assert.Equal(expectedBool, actualBool);
    }

    private static IEnumerable<object[]> IsPacOnGhostTestData()
    {
        yield return new object[]
        {
            new Coordinate(0, 0),
            new Coordinate(0, 0),
            true
        };
        
        yield return new object[]
        {
            new Coordinate(1, 0),
            new Coordinate(0, 0),
            false
        };
    }
}
