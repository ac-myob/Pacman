using Pacman.Variables;

namespace Pacman.Business.Control.Sequence;

public class GhostTypeSequence : ISequence<GhostType>
{
    private readonly GhostType[] _ghostTypes = Enum.GetValues(typeof(GhostType)).Cast<GhostType>().ToArray();
    private int _currentGhostIndex;

    public GhostType GetNext()
    {
        var nextGhostType = _ghostTypes[_currentGhostIndex];
        _currentGhostIndex = Utilities.Mod(_currentGhostIndex + 1, _ghostTypes.Length);

        return nextGhostType;
    }
}