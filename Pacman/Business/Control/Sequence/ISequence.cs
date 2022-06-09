namespace Pacman.Business.Control.Sequence;

public interface ISequence<out T>
{
    public T GetNext();
}