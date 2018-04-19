using System;
using MiniRpgGame.Domain.Commands;
using MiniRpgGame.Domain.Events;
using MiniRpgGame.Domain.Platform;
using MiniRpgGame.Settings;

namespace MiniRpgGame.Domain
{
    public sealed class Gameplay :
        ICommandHandler<FightMonsterCommand>,
        ICommandHandler<BuyWeaponsCommand>,
        ICommandHandler<BuyArmorCommand>,
        ICommandHandler<HealCommand>,
        ICommandHandler<StartGameCommand>
    {
        private readonly Seller _seller;
        private readonly Battle _battle;
        private readonly Doctor _doctor;
        private readonly IGameplaySettings _settings;
        private readonly IEventBus _eventBus;

        public Player Player { get; private set; }

        public bool IsStarted { get; private set; }
        
        public Gameplay(Seller seller, Battle battle, Doctor doctor, IEventBus eventBus, IGameplaySettings settings)
        {
            _seller = seller ?? throw new ArgumentNullException(nameof(seller));
            _battle = battle ?? throw new ArgumentNullException(nameof(battle));
            _doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            WireUpEvents();
        }

        public void Handle(FightMonsterCommand command)
        {
            ThrowExceptionIfGameIsNotStarted();
            _battle.Do(Player);
        }

        public void Handle(BuyWeaponsCommand command)
        {
            ThrowExceptionIfGameIsNotStarted();
            _seller.SellWeapons(Player); 
        }

        public void Handle(BuyArmorCommand command)
        {
            ThrowExceptionIfGameIsNotStarted();
            _seller.SellArmor(Player);
        }

        public void Handle(HealCommand command)
        {
            ThrowExceptionIfGameIsNotStarted();
            _doctor.Heal(Player);
        }

        public void Handle(StartGameCommand command)
        {
            if (IsStarted)
            {
                throw new InvalidOperationException("The game is start.");
            }
            
            Player = new Player(
                _settings.InitialHealth,
                _settings.InitialMaxHealth,
                _settings.InitialPower,
                _settings.InitialCoins,
                _eventBus);
            
            IsStarted = true;
            
            _eventBus.Publish(new GameStartedEvent());
        }

        private void ThrowExceptionIfGameIsNotStarted()
        {
            if (!IsStarted)
            {
                throw new InvalidOperationException("The game is not start.");
            }
        }

        private void WireUpEvents()
        {
            _eventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
        }

        private void OnPlayerDied(PlayerDiedEvent @event)
        {
            ThrowExceptionIfGameIsNotStarted();
            IsStarted = false;
            _eventBus.Publish(new GameFinishedEvent());
        }
    }
}