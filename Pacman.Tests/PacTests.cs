using Moq;
using Xunit;

namespace Pacman.Tests;

public class PacTests
{
    private readonly Mock<IReader> _reader = new();
    private readonly Mock<IWriter> _writer = new();

    [Fact]
    public void Move_ShouldMovePacUp_WhenUpKeyPressed()
    {
        var pac = new Pac(new Coordinate(1, 1), _reader.Object, _writer.Object);
        var expectedCoordinate = new Coordinate(1, 0);
        _reader.Setup(_ => _.ReadKey()).Returns("UpArrow");

        pac.Move();
        
        Assert.Equal(expectedCoordinate, pac.Coordinate);
    }
    
    [Fact]
    public void Move_ShouldMovePacDown_WhenDownKeyPressed()
    {
        var pac = new Pac(new Coordinate(1, 1), _reader.Object, _writer.Object);
        var expectedCoordinate = new Coordinate(1, 2);
        _reader.Setup(_ => _.ReadKey()).Returns("DownArrow");

        pac.Move();
        
        Assert.Equal(expectedCoordinate, pac.Coordinate);
    }
    
    [Fact]
    public void Move_ShouldMovePacLeft_WhenLeftKeyPressed()
    {
        var pac = new Pac(new Coordinate(1, 1), _reader.Object, _writer.Object);
        var expectedCoordinate = new Coordinate(0, 1);
        _reader.Setup(_ => _.ReadKey()).Returns("LeftArrow");

        pac.Move();
        
        Assert.Equal(expectedCoordinate, pac.Coordinate);
    }
    
    [Fact]
    public void Move_ShouldMovePacUp_WhenRightKeyPressed()
    {
        var pac = new Pac(new Coordinate(1, 1), _reader.Object, _writer.Object);
        var expectedCoordinate = new Coordinate(2, 1);
        _reader.Setup(_ => _.ReadKey()).Returns("RightArrow");

        pac.Move();
        
        Assert.Equal(expectedCoordinate, pac.Coordinate);
    }
}