using Pacman.Variables;

namespace Pacman.Business.View;

public interface IWriter
{
    public void Write(string message);
    
    public void Write(string message, IDictionary<char, Colour> colourMapping);

    public void Clear();
}
