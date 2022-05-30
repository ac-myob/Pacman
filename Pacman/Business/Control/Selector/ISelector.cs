namespace Pacman.Business.Control.Selector;

public interface ISelector<T>
{
    public T Select(IEnumerable<T> collection);
}
