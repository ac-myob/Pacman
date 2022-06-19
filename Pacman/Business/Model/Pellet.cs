namespace Pacman.Business.Model;

public class Pellet : IEntity, IResetable
{
    public Coordinate Coordinate { get; }
    public char Symbol { get; }
    public bool Eaten { get; set; }

    public Pellet(Coordinate coordinate, char symbol, bool eaten = false)
    {
        Coordinate = coordinate;
        Symbol = symbol;
        Eaten = eaten;
    }

    public void ResetState() => Eaten = false;
}
