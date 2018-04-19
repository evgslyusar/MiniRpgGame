using System;
using System.Collections.Generic;
using MiniRpgGame.Domain;
using MiniRpgGame.Domain.Commands;
using MiniRpgGame.Domain.Events;
using MiniRpgGame.Domain.Platform;

namespace MiniRpgGame.Infrastructure
{
    internal sealed class ConsoleGame : IGame
    {
        private readonly IReadOnlyDictionary<ConsoleKey, Action> _availableActions;
        private readonly Gameplay _gameplay;
        private readonly IEventBus _eventBus;
        private readonly IConsoleGameIO _gameIO;
        
        private bool _isStarted;

        public ConsoleGame(Gameplay gameplay, IConsoleGameIO gameIO, IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _gameplay = gameplay ?? throw new ArgumentNullException(nameof(gameplay));
            _gameIO = gameIO ?? throw new ArgumentNullException(nameof(gameIO));

            _availableActions = new Dictionary<ConsoleKey, Action>
            {
                {ConsoleKey.W, () => _gameplay.Handle(new FightMonsterCommand()) },
                {ConsoleKey.A, () => _gameplay.Handle(new BuyWeaponsCommand()) },
                {ConsoleKey.D, () => _gameplay.Handle(new BuyArmorCommand()) }, 
                {ConsoleKey.S, () => _gameplay.Handle(new HealCommand()) }
            };

            WireUpEvents();
        }

        public void StartGame()
        {
            _isStarted = true;
            _gameplay.Handle(new StartGameCommand());
            GameLoop();
        }
        
        private void GameLoop()
        {
            while (_isStarted)
            {
                var key = _gameIO.ReadKey("Press any of (w - attack | a - weapon | d - armor | s - heal | e - auto | ESC - exit) keys: ");

                if (key == ConsoleKey.Escape)
                {
                    QuitGame();
                }
                else
                {
                    MakeMove(key);
                }
            }
        }

        private void MakeMove(ConsoleKey moveType)
        {
            if (!_availableActions.TryGetValue(moveType, out var action))
            {
                _gameIO.DisplayError("Unknow command! Try again.");
            }
            else
            {
                action.Invoke();
            }
        }
        
        private void PlayAgain()
        {
            var key = _gameIO.ReadKey("Repeat? (y/n) :");
            if (key == ConsoleKey.Y)
            {
                StartGame();
            }
        }
        
        private void QuitGame()
        {
            var key = _gameIO.ReadKey("Are you sure? (y/n) :");
            if (key == ConsoleKey.Y)
            {
                _isStarted = false;
            }
        }
        
        private void WireUpEvents()
        {
            _eventBus.Subscribe<GameStartedEvent>(OnGameStarted);
            _eventBus.Subscribe<GameFinishedEvent>(OnGameFinished);
            _eventBus.Subscribe<MoveCompleteEvent>(OnMoveComplete);
        }
        
        private void OnMoveComplete(MoveCompleteEvent @event)
        {
            if (!string.IsNullOrEmpty(@event.Message))
            {
                if (!@event.Successful)
                {
                    _gameIO.DisplayError(@event.Message);
                }
                else
                {
                    _gameIO.DisplayMessage(@event.Message);
                }
            }
            
            _gameIO.DisplayLine();
            _gameIO.Display(_gameplay.Player);
        }

        private void OnGameStarted(GameStartedEvent @event)
        {
            _gameIO.DisplayMessage("A new game has begun!");
            _gameIO.DisplayLine();
            _gameIO.Display(_gameplay.Player);
        }

        private void OnGameFinished(GameFinishedEvent @event)
        {
            _gameIO.DisplayMessage("Game Over!");
            _gameIO.DisplayLine();
            _isStarted = false;
            PlayAgain();
        }
    }
}