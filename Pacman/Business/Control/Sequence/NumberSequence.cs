namespace Pacman.Business.Control.Sequence;

public class NumberSequence : ISequence<int>
{
    private int _start;

    public NumberSequence(int start)
    {
        _start = start;
    }

    public int GetNext()
    {
        var next = _start;
        ++_start;
        
        return next;
    }
}