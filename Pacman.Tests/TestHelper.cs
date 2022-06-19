using System;
using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Tests;

public static class TestHelper
{ 
    public static GameState GetGameState()
    {
        return new GameState(
            It.IsAny<Size>(),
            Constants.PacStartingLives,
            Constants.StartRound,
            It.IsAny<int>(),
            It.IsAny<GameStatus>(),
            It.IsAny<Pac>(),
            Array.Empty<Ghost>(),
            new Dictionary<Coordinate, Wall>(),
            Array.Empty<Pellet>());
    }
}
