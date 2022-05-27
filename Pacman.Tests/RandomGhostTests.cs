using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Xunit;
using Capture = Moq.Capture;

namespace Pacman.Tests;

public class RandomGhostTests
{
    [Fact]
    public void Move_MovesGhostToRandomValidPosition_GivenNoObstacles()
    {
        var mapSize = new Size(3, 3);
        var mockSelector = new Mock<ISelector<Coordinate>>();
        var randomGhost = new RandomGhost(new Coordinate(1, 1), mockSelector.Object);
        var expectedPosCoords = new[]
            {new Coordinate(1, 0), new Coordinate(1, 2), new Coordinate(0, 1), new Coordinate(2, 1)};
        var actualPosCoords = new List<IEnumerable<Coordinate>>();

        mockSelector.Setup(_ => _.Select(Capture.In(actualPosCoords)));
        randomGhost.Move(mapSize, Array.Empty<Entity>());

        Assert.Equal(expectedPosCoords, actualPosCoords.First());
    }
}