using MiniRPG.Models;
using MiniRPG.Services;
using System.Collections.ObjectModel;

namespace MiniRPG.ViewModels
{
    // ViewModel principale del gioco, collega GameLoop, Player e la UI
    public class GameViewModel : BaseViewModel
    {
        private readonly GameLoop _gameLoop;       // logica principale del gioco
        private readonly LevelManager _levelManager; // gestisce livelli e progressione

        // Riferimento al player dal GameLoop
        public Player Player => _gameLoop.Player!;

        // ViewModel per gestire gli upgrade del player
        public UpgradeViewModel UpgradeVM { get; private set; }

        // Lista di nemici visibile dalla UI (ObservableCollection aggiorna la UI automaticamente)
        public ObservableCollection<Enemy> Enemies { get; } = new();

        // Proprietà usate per la UI
        public int PlayerCoins => Player.Coins;       // valuta attuale del player
        public int CurrentLevel => _levelManager.CurrentLevel; // livello corrente

        // Eventi della UI: notificano cambiamenti importanti
        public event Action? GameCompleted;    // gioco terminato
        public event Action? LevelCompleted;   // livello completato
        public event Action? PlayerDied;       // player morto

        // Costruttore: inizializza GameLoop, eventi e avvia il gioco
        public GameViewModel()
        {
            _levelManager = new LevelManager();

            // servizi necessari al GameLoop
            var inputService = new InputService();
            var enemySpawner = new EnemySpawner(800, 600); // dimensioni mappa

            _gameLoop = new GameLoop(inputService, enemySpawner, _levelManager);

            // Colleghiamo gli eventi del GameLoop ai metodi locali
            _gameLoop.LevelCompleted += HandleLevelCompleted;
            _gameLoop.PlayerDied += HandlePlayerDied;
            _gameLoop.GameCompleted += HandleGameCompleted;

            // Avvia un nuovo gioco (player + nemici)
            _gameLoop.StartNewGame();

            // Creiamo UpgradeViewModel una sola volta: prezzi persistono tra i livelli
            UpgradeVM = new UpgradeViewModel(Player);

            // Popoliamo la lista di nemici visibile dalla UI
            foreach (var enemy in _gameLoop.Enemies)
                Enemies.Add(enemy);
        }

        // --- METODI DI GESTIONE EVENTI ---

        private void HandleLevelCompleted()
        {
            // Notifica la UI che il livello è stato completato
            LevelCompleted?.Invoke();
        }

        private void HandlePlayerDied()
        {
            // Notifica la UI che il player è morto
            PlayerDied?.Invoke();
        }

        private void HandleGameCompleted()
        {
            // Notifica la UI che il gioco è terminato
            GameCompleted?.Invoke();
        }

        // --- METODI PUBBLICI PER LA UI ---

        public void ResumeNextLevel()
        {
            // Carica il livello successivo
            _gameLoop.ResumeNextLevel();

            // Aggiorna lista nemici visibile dalla UI
            Enemies.Clear();
            foreach (var enemy in _gameLoop.Enemies)
                Enemies.Add(enemy);

            // Notifica cambiamenti alla UI
            OnPropertyChanged(nameof(CurrentLevel));
            OnPropertyChanged(nameof(PlayerCoins));

            // ⚠️ NON ricreare UpgradeVM: i prezzi NON si resettano tra i livelli
        }

        public void StopGame()
        {
            // Ferma il GameLoop
            _gameLoop.Stop();
        }
    }
}