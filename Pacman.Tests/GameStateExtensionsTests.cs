using System.Collections.Generic;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Model;
using Pacman.Business.Model.Ghosts;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests;

public class GameStateExtensionsTests
{
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesDoNotShareSameCoordinate()
    {
        var ghosts = new[]
        {
            new Ghost(new Coordinate(0, 2), Constants.RandomGhost, It.IsAny<IMoveStrategy>()),
            new Ghost(new Coordinate(2, 0), Constants.GreedyGhost, It.IsAny<IMoveStrategy>()),
            new Ghost(new Coordinate(2, 2), Constants.PathFindingGhost, It.IsAny<IMoveStrategy>())
        };
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = new Pac(new Coordinate(), Constants.PacStart, It.IsAny<int>()),
            Ghosts = ghosts,
            Walls = new Wall[] {new(new Coordinate(1, 1), Constants.WallHorz)}.ToDictionary(k => k.Coordinate, v => v)
        };

        var expectedString = $"{Constants.PacStart}{Constants.Blank}{Constants.GreedyGhost}\n" +
                             $"{Constants.Blank}{Constants.WallHorz}{Constants.Blank}\n" +
                             $"{Constants.RandomGhost}{Constants.Blank}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }
    
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesShareSameCoordinate()
    {
        var ghosts = new[]
        {
            new Ghost(new Coordinate(0, 2), Constants.RandomGhost, It.IsAny<IMoveStrategy>()),
            new Ghost(new Coordinate(2, 0), Constants.GreedyGhost, It.IsAny<IMoveStrategy>()),
            new Ghost(new Coordinate(2, 2), Constants.PathFindingGhost, It.IsAny<IMoveStrategy>())
        };
        var pellets = new List<Pellet>();
        for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                pellets.Add(new Pellet(new Coordinate(i, j), Constants.Pellet));
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = new Pac(new Coordinate(), Constants.PacStart, It.IsAny<int>()),
            Ghosts = ghosts,
            Walls = new Wall[] {new(new Coordinate(1, 1), Constants.WallHorz)}.ToDictionary(k => k.Coordinate, v => v),
            Pellets = pellets
        };

        var expectedString = $"{Constants.PacStart}{Constants.Pellet}{Constants.GreedyGhost}\n" +
                             $"{Constants.Pellet}{Constants.WallHorz}{Constants.Pellet}\n" +
                             $"{Constants.RandomGhost}{Constants.Pellet}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }
}
