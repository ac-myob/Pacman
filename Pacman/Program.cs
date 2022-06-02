using Pacman.Business.Control;
using Pacman.Business.View;

var consoleReader = new ConsoleReader();
var consoleWriter = new ConsoleWriter();
var gameEngine = new GameEngine(consoleReader, consoleWriter);

gameEngine.Run();