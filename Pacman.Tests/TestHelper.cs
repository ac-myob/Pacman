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
    public static Pac GetPac()
    {
        return new Pac(It.IsAny<Coordinate>(),
            Constants.PacStart,
            It.IsAny<IMoveStrategy>());
    }

    public static Ghost GetGhost()
    {
        return new Ghost(It.IsAny<Coordinate>(),
            It.IsAny<char>(),
            It.IsAny<int>(),
            It.IsAny<IMoveStrategy>());
    }
    
    public static GameState GetGameState()
    {
        return new GameState(It.IsAny<Size>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<Pac>(),
            Array.Empty<Ghost>(),
            Array.Empty<Wall>(),
            Array.Empty<Pellet>());
    }
}
