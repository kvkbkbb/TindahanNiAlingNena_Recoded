using TindahanNiAlingNena_Recoded.Services;
using TindahanNiAlingNena_Recoded.UI;

//ApplicationConfiguration.Initialize();

GameSession session = new GameSession();
GameScreen screen = new GameScreen();

// player clicks -> game logic
screen.DenominationClicked += session.AddChangePiece;
screen.GiveChangeClicked += session.GiveChange;
screen.ClearClicked += session.ClearChange;
screen.AskForMoreClicked += session.AskForMore;

// game changes -> screen display
session.StateChanged += screen.ShowState;
session.MessageSent += screen.ShowMessage;
session.CustomerStarted += () => screen.ShowCustomer(CustomerState.Normal);

session.Start();
Application.Run(screen);