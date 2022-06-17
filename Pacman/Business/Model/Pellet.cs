namespace Pacman.Business.Model;

public class Pellet : Entity
{
    public bool Eaten { get; set; }

    public Pellet(Coordinate coordinate, char symbol, bool eaten = false) : base(coordinate, symbol)
    {
        Eaten = eaten;
    }
}
