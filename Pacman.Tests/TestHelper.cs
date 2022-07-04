using System;
using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Model;
using Pacman.Business.Model.Ghosts;
using Pacman.Variables;

namespace Pacman.Tests;

public static class TestHelper
{ 
    public static GameState GetGameState()
    {
        return new GameState(
            It.IsAny<Size>(),
            Constants.StartRound,
            It.IsAny<GameStatus>(),
            It.IsAny<Pac>(),
            Array.Empty<Ghost>(),
            new Dictionary<Coordinate, Wall>(),
            Array.Empty<Pellet>());
    }
}
