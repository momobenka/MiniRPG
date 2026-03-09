using MiniRPG.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MiniRPG.ViewModels
{
    // ViewModel per gestire gli upgrade del player
    public class UpgradeViewModel : INotifyPropertyChanged
    {
        private readonly Player _player; // riferimento al player

        // Livelli attuali di upgrade
        public int DamageLevel { get; private set; } = 1;
        public int HealthLevel { get; private set; } = 1;
        public int SpeedLevel { get; private set; } = 1;

        // Costi correnti per gli upgrade (persistenti)
        public int DamageCost { get; private set; } = 5;
        public int HealthCost { get; private set; } = 5;
        public int SpeedCost { get; private set; } = 5;

        // Testi mostrati nella UI
        public string DamageLevelText => $"Danno – Livello {DamageLevel} ({_player.Stats.Damage} DMG)";
        public string HealthLevelText => $"Vita – Livello {HealthLevel} ({_player.Stats.MaxHealth} HP)";
        public string SpeedLevelText => $"Velocità – Livello {SpeedLevel} ({_player.Stats.Speed} SPD)";

        public int PlayerCoins => _player.Coins; // valuta corrente del player

        public event Action? UpgradesFinished; // evento chiamato quando l'utente termina gli upgrade

        // Comandi della UI
        public ICommand UpgradeDamageCommand { get; }
        public ICommand UpgradeHealthCommand { get; }
        public ICommand UpgradeSpeedCommand { get; }
        public ICommand FinishUpgradesCommand { get; }

        public UpgradeViewModel(Player player)
        {
            _player = player;

            UpgradeDamageCommand = new RelayCommand(_ => UpgradeDamage());
            UpgradeHealthCommand = new RelayCommand(_ => UpgradeHealth());
            UpgradeSpeedCommand = new RelayCommand(_ => UpgradeSpeed());
            FinishUpgradesCommand = new RelayCommand(_ => FinishUpgrades());
        }

        private void UpgradeDamage()
        {
            if (_player.SpendCoins(DamageCost))
            {
                _player.UpgradeDamage(DamageCost, 3); // aumenta danno
                DamageLevel++;                        // aumenta livello
                DamageCost += 3;                      // incremento costo
                NotifyAll();
            }
        }

        private void UpgradeHealth()
        {
            if (_player.SpendCoins(HealthCost))
            {
                _player.UpgradeHealth(HealthCost, 10);
                HealthLevel++;
                HealthCost += 3;
                NotifyAll();
            }
        }

        private void UpgradeSpeed()
        {
            if (_player.SpendCoins(SpeedCost))
            {
                _player.UpgradeSpeed(SpeedCost, 5);
                SpeedLevel++;
                SpeedCost += 3;
                NotifyAll();
            }
        }

        // Chiamato quando l'utente termina gli upgrade
        public void FinishUpgrades() => UpgradesFinished?.Invoke();

        // Notifica la UI di tutti i cambiamenti
        private void NotifyAll()
        {
            OnPropertyChanged(nameof(PlayerCoins));
            OnPropertyChanged(nameof(DamageCost));
            OnPropertyChanged(nameof(HealthCost));
            OnPropertyChanged(nameof(SpeedCost));
            OnPropertyChanged(nameof(DamageLevelText));
            OnPropertyChanged(nameof(HealthLevelText));
            OnPropertyChanged(nameof(SpeedLevelText));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}