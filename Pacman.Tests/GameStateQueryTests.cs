using System.Collections.Generic;
using Pacman.Business.Control;
using Pacman.Business.Model;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests;

public class GameStateQueryTests
{
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesDoNotShareSameCoordinate()
    {
        var ghosts = new[]
        {
            TestHelper.GetGhost() with{Coordinate = new Coordinate(0, 2), Symbol = Constants.RandomGhost},
            TestHelper.GetGhost() with{Coordinate = new Coordinate(2, 0), Symbol = Constants.GreedyGhost},
            TestHelper.GetGhost() with{Coordinate = new Coordinate(2, 2), Symbol = Constants.PathFindingGhost},
        };
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = TestHelper.GetPac() with {Coordinate = new Coordinate(0, 0)},
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
        var ghosts = new[]
        {
            TestHelper.GetGhost() with{Coordinate = new Coordinate(0, 2), Symbol = Constants.RandomGhost},
            TestHelper.GetGhost() with{Coordinate = new Coordinate(2, 0), Symbol = Constants.GreedyGhost},
            TestHelper.GetGhost() with{Coordinate = new Coordinate(2, 2), Symbol = Constants.PathFindingGhost},
        };
        var pellets = new List<Pellet>();
        for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                pellets.Add(new Pellet(new Coordinate(i, j), Constants.Pellet));
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = TestHelper.GetPac() with {Coordinate = new Coordinate(0, 0)},
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
        var gameState = TestHelper.GetGameState() with
        {
            Pac = TestHelper.GetPac() with {Coordinate = pacCoord},
            Ghosts = new[] {TestHelper.GetGhost() with{Coordinate = ghostCoord}}
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
