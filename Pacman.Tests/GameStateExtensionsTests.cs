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

public class GameStateExtensionsTests
{
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesDoNotShareSameCoordinate()
    {
        var pac = new Pac(new Coordinate(0, 0), It.IsAny<IReader>(), It.IsAny<IWriter>());
        var gameState = new GameState(
            new Size(3, 3),
            pac,
            new Wall[] { new(new Coordinate(1, 1))},
            new Dictionary<Coordinate, Pellet>(),
            new MovableEntity[]
            {
                new RandomGhost(new Coordinate(0, 2), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(2, 0), pac),
                new PathFindingGhost(new Coordinate(2, 2), pac)
            }
        );
        var expectedString = $"{Constants.PacStart}{Constants.Blank}{Constants.GreedyGhost}\n" +
                             $"{Constants.Blank}{Constants.Wall}{Constants.Blank}\n" +
                             $"{Constants.RandomGhost}{Constants.Blank}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }
    
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesShareSameCoordinate()
    {
        var pellets = new Dictionary<Coordinate, Pellet>();
        for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
            {
                var coord = new Coordinate(i, j);
                pellets.Add(coord, new Pellet(coord));
            }
        
        var pac = new Pac(new Coordinate(0, 0), It.IsAny<IReader>(), It.IsAny<IWriter>());
        var gameState = new GameState(
            new Size(3, 3),
            pac,
            new Wall[] { new(new Coordinate(1, 1))},
            pellets,
            new MovableEntity[]
            {
                new RandomGhost(new Coordinate(0, 2), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(2, 0), pac),
                new PathFindingGhost(new Coordinate(2, 2), pac)
            }
        );
        var expectedString = $"{Constants.PacStart}{Constants.Pellet}{Constants.GreedyGhost}\n" +
                             $"{Constants.Pellet}{Constants.Wall}{Constants.Pellet}\n" +
                             $"{Constants.RandomGhost}{Constants.Pellet}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }

    [Fact]
    public void GetMovableEntities_ReturnsEnumerableOfEntitiesInCorrectOrder()
    {
        var gameState = new GameState(
            It.IsAny<Size>(),
            new Pac(new Coordinate(), It.IsAny<IReader>(), It.IsAny<IWriter>()),
            It.IsAny<IEnumerable<Wall>>(),
            It.IsAny<IDictionary<Coordinate, Pellet>>(),
            new MovableEntity[]
            {
                new RandomGhost(new Coordinate(), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(), It.IsAny<Pac>()),
                new PathFindingGhost(new Coordinate(), It.IsAny<Pac>())
            });
        var expectedTypes = new[] {typeof(Pac), typeof(RandomGhost), typeof(GreedyGhost), typeof(PathFindingGhost)};

        var actualTypes = gameState.GetMovableEntities().Select(g => g.GetType());
        
        Assert.Equal(expectedTypes, actualTypes);
    }

    [Theory]
    [InlineData(GhostType.Random, typeof(RandomGhost))]
    [InlineData(GhostType.Greedy, typeof(GreedyGhost))]
    [InlineData(GhostType.PathFinding, typeof(PathFindingGhost))]
    public void AddGhost_AddsGhostToGhostEnumerable_WhenGivenGhostType(GhostType ghostType, Type typeOfGhost)
    {
        var gameState = new GameState(
            It.IsAny<Size>(),
            new Pac(new Coordinate(), It.IsAny<IReader>(), It.IsAny<IWriter>()),
            It.IsAny<IEnumerable<Wall>>(),
            It.IsAny<IDictionary<Coordinate, Pellet>>(),
            new List<MovableEntity>
            {
                new RandomGhost(new Coordinate(), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(), It.IsAny<Pac>()),
                new PathFindingGhost(new Coordinate(), It.IsAny<Pac>())
            });
        var expectedTypes = gameState.GetMovableEntities().Select(e => e.GetType()).Append(typeOfGhost).ToArray();
        
        gameState.AddGhost(ghostType, new Coordinate(), It.IsAny<IReader>(), It.IsAny<IWriter>());
        var actualTypes = gameState.GetMovableEntities().Select(e => e.GetType()).ToArray();

        Assert.Equal(expectedTypes, actualTypes);
    }
}
