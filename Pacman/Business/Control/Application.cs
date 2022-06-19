using Pacman.Business.View;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class Application
{
    private readonly Query _query;
    private readonly IReader _reader;
    private readonly IWriter _writer;
    private readonly IGameService _gameService;
    private readonly IDictionary<char, Colour> _entityColours = new Dictionary<char, Colour> 
    {
        {Constants.RandomGhost, Colour.Blue},
        {Constants.GreedyGhost, Colour.Green},
        {Constants.PathFindingGhost, Colour.Red},
        {Constants.PacStart, Colour.Yellow},
        {Constants.PacUp, Colour.Yellow},
        {Constants.PacDown, Colour.Yellow},
        {Constants.PacLeft, Colour.Yellow},
        {Constants.PacRight, Colour.Yellow}
    };
    private readonly IDictionary<char, Colour> _turnInfoColours = new Dictionary<char, Colour>
    {
        {Constants.Heart, Colour.Red}
    };

    public Application(IReader reader, IWriter writer, IGameService gameService)
    {
        _reader = reader;
        _writer = writer;
        _query = new Query(reader, writer);
        _gameService = gameService;
    }

    public void Run()
    {
        DisplayGame();
        
        do
        {
            switch (_gameService.GameState.GameStatus)
            {
                case GameStatus.Collided:
                    _writer.Write(Messages.GhostCollision);
                    _reader.ReadKey();
                    _gameService.ResetRound();
                    DisplayGame();
                    break;
                case GameStatus.RoundComplete:
                    _writer.Write(Messages.RoundComplete);
                    _reader.ReadKey();
                    _gameService.IncreaseRound();
                    DisplayGame();
                    break;
                case  GameStatus.Running:
                {
                    var userInput = GetKeyPress();
                    _gameService.PlayRound(userInput);
                    DisplayGame();
                    break;
                }
                case GameStatus.GameComplete:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        } while(_gameService.GameState.GameStatus != GameStatus.GameComplete);
        
        _writer.Write(Messages.GetGameOutcome(_gameService.GameState));
    }

    private void DisplayGame()
    {
        _writer.Clear();
        _writer.Write(Messages.GetTurnInfo(_gameService.GameState), _turnInfoColours);
        _writer.Write(_gameService.GameState.GetString(), _entityColours);
    }

    private Direction GetKeyPress()
    {
        Direction direction;
        bool isDirectionValid;
        
        do
        {
            var keyPress = _query.GetKeyPress(Constants.ValidKeysRegex, Messages.InvalidKeyPress);

            direction = keyPress switch
            {
                Constants.UpKey => Direction.North,
                Constants.DownKey => Direction.South,
                Constants.LeftKey => Direction.West,
                Constants.RightKey => Direction.East,
                _ => throw new ArgumentOutOfRangeException()
            };

            isDirectionValid = _gameService.GameState.IsDirectionValid(direction);

            if (!isDirectionValid)
                _writer.Write(Messages.WallObstruction);

        } while (!isDirectionValid);

        return direction;
    }
}
