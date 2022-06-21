using System;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Control.WorldLoader;
using Pacman.Business.Model;
using Xunit;

namespace Pacman.Tests;

public class WorldBuilderTests
{
    private readonly Mock<IWorldLoader> _worldLoader = new();

    [Fact]
    public void GetEntities_ReturnsEnumerableOfEntitiesWithCorrectCoordinates()
    {
        var expectedEntityCoordinates = new Coordinate[]
        {
            new(0, 0), new(1, 0), new(2, 0),
            new(2, 1),
            new(0, 2), new(1, 2), new(2, 2),
            new(0, 3), new(1, 3)
        };
        _worldLoader.Setup(_ => _.LoadWorld()).Returns(
            new[,] {
                {'·', '·', '·'},
                {'X', 'X', '◯'},
                {'ᗣ', 'ᓆ', '▩'},
                {'▩', 'ᔬ', 'X'}
            });

        var worldBuilder = new WorldBuilder(_worldLoader.Object, new GhostFactory(new RandomSelector<Coordinate>()));
        var actualEntityCoordinates = worldBuilder.GetEntities().Select(e => e.Coordinate);
        
        Assert.Equal(expectedEntityCoordinates, actualEntityCoordinates);
    }

    [Fact]
    public void GetEntities_ReturnsEnumerableOfEntitiesWithCorrectTypes()
    {
        var expectedEntityTypes= new[]
        {
            typeof(Pellet), typeof(Pellet), typeof(Pellet),
            typeof(Pac),
            typeof(Ghost), typeof(Ghost), typeof(Wall),
            typeof(Wall), typeof(Ghost)
        };
        _worldLoader.Setup(_ => _.LoadWorld()).Returns(
            new[,] {
                {'·', '·', '·'},
                {'X', 'X', '◯'},
                {'ᗣ', 'ᓆ', '▩'},
                {'▩', 'ᔬ', 'X'}
            });
        
        var worldBuilder = new WorldBuilder(_worldLoader.Object, new GhostFactory(new RandomSelector<Coordinate>()));
        var actualEntityTypes = worldBuilder.GetEntities().Select(e => e.GetType());
        
        Assert.Equal(expectedEntityTypes, actualEntityTypes);
    }
}