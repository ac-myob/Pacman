using Pacman.Business.Control;
using Pacman.Business.View;

var consoleReader = new ConsoleReader();
var consoleWriter = new ConsoleWriter();
var gameService = new GameService();
var application = new Application(consoleReader, consoleWriter, gameService);

application.Run();
