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

public class GameStateTests
{
    [Fact]
    public void MovableEntities_ReturnsEnumerableOfEntitiesInCorrectOrder()
    {
        var gameState = new GameState(
            It.IsAny<Size>(),
            new Pac(new Coordinate(), It.IsAny<IReader>(), It.IsAny<IWriter>()),
            It.IsAny<IEnumerable<Wall>>(),
            new List<Pellet>(),
            new MovableEntity[]
            {
                new RandomGhost(new Coordinate(), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(), It.IsAny<Pac>()),
                new PathFindingGhost(new Coordinate(), It.IsAny<Pac>())
            });
        var expectedTypes = new[] {typeof(Pac), typeof(RandomGhost), typeof(GreedyGhost), typeof(PathFindingGhost)};

        var actualTypes = gameState.MovableEntities.Select(g => g.GetType());
        
        Assert.Equal(expectedTypes, actualTypes);
    }
    
    [Theory]
    [InlineData(1, typeof(RandomGhost))]
    [InlineData(2, typeof(GreedyGhost))]
    [InlineData(3, typeof(PathFindingGhost))]
    public void AddGhost_AddsGhostToGhostEnumerable_WhenGivenRoundNumber(int round, Type typeOfGhost)
    {
        var expectedStartCoord = new Coordinate(14, 11);
        var mockSelector = new Mock<ISelector<Coordinate>>();
        mockSelector.Setup(_ => _.Select(It.IsAny<IEnumerable<Coordinate>>())).Returns(expectedStartCoord);
        var gameState = new GameState(
            It.IsAny<Size>(),
            new Pac(new Coordinate(), It.IsAny<IReader>(), It.IsAny<IWriter>()),
            It.IsAny<IEnumerable<Wall>>(),
            new List<Pellet>(),
            new List<MovableEntity>
            {
                new RandomGhost(new Coordinate(), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(), It.IsAny<Pac>()),
                new PathFindingGhost(new Coordinate(), It.IsAny<Pac>())
            });
        var expectedTypes = gameState.MovableEntities.Select(e => e.GetType()).Append(typeOfGhost).ToArray();
        for (var i = 0; i < round - 1; i++) gameState.IncreaseRound();

        gameState.AddGhost(mockSelector.Object);
        var actualTypes = gameState.MovableEntities.Select(e => e.GetType()).ToArray();

        Assert.Equal(expectedStartCoord, gameState.Ghosts.Last().Coordinate);
        Assert.Equal(expectedTypes, actualTypes);
    }
}