using System;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.MoveStrategies;
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
            It.IsAny<Pac>(),
            Array.Empty<Ghost>(),
            Array.Empty<Wall>(),
            Array.Empty<Pellet>());
    }
}
