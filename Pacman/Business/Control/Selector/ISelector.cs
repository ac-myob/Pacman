namespace Pacman.Business.Control.Selector;

public interface ISelector<T>
{
    public T SelectFrom(IEnumerable<T> collection);
}
