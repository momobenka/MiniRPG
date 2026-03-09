using System.Windows;
using MiniRPG.Views;
using MiniRPG.ViewModels;

namespace MiniRPG
{
    // Finestra principale dell'app WPF
    public partial class MainWindow : Window
    {
        private GameView? _gameView; // Controllo per il gioco attuale

        public MainWindow()
        {
            InitializeComponent();
        }

        // --- Bottone "Start" ---
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGame();
        }

        // Mostra la schermata di gioco
        private void ShowGame()
        {
            // Nasconde HomeScreen e mostra il ContentHost (contenitore per UserControl)
            HomeScreen.Visibility = Visibility.Collapsed;
            ContentHost.Visibility = Visibility.Visible;

            // Crea il GameView e lo inserisce nel ContentHost
            _gameView = new GameView();
            ContentHost.Content = _gameView;

            // Collega gli eventi principali dal ViewModel del gioco
            var vm = _gameView.GetViewModel();
            vm.LevelCompleted += OnLevelCompleted;
            vm.PlayerDied += OnPlayerDied;
            vm.GameCompleted += OnGameCompleted;
        }

        // --- Livello completato ---
        private void OnLevelCompleted()
        {
            Dispatcher.Invoke(() =>
            {
                if (_gameView == null) return;

                var player = _gameView.GetViewModel().Player;

                // Mostra la schermata di Upgrade passando il player
                var upgradeView = new UpgradeView(player);
                upgradeView.UpgradesFinished += OnUpgradesFinished;

                ContentHost.Content = upgradeView;
            });
        }

        // --- L'utente ha finito gli upgrade ---
        private void OnUpgradesFinished()
        {
            Dispatcher.Invoke(() =>
            {
                if (_gameView == null) return;

                // Torna alla schermata di gioco e riprende il livello successivo
                ContentHost.Content = _gameView;
                _gameView.GetViewModel().ResumeNextLevel();
            });
        }

        // --- Player morto ---
        private void OnPlayerDied()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Sei morto! Torni alla Home.");
                ReturnToHome();
            });
        }

        // --- Gioco completato ---
        private void OnGameCompleted()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Complimenti! Hai completato il gioco!");
                ReturnToHome();
            });
        }

        // Torna alla HomeScreen
        public void ReturnToHome()
        {
            ContentHost.Visibility = Visibility.Collapsed;
            HomeScreen.Visibility = Visibility.Visible;
        }
    }
}