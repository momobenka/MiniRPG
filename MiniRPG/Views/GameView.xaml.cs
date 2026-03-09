using MiniRPG.Models;
using MiniRPG.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiniRPG.Views
{
    // UserControl che gestisce la visualizzazione del gioco (canvas + animazioni)
    public partial class GameView : UserControl
    {
        private readonly GameViewModel _viewModel; // ViewModel collegato alla UI
        private bool _isReady = false;              // Flag per sapere se il controllo è caricato

        public GameView()
        {
            InitializeComponent();

            // Inizializza ViewModel e assegna al DataContext per il binding XAML
            _viewModel = new GameViewModel();
            DataContext = _viewModel;

            // Collega eventi del ViewModel a metodi locali
            _viewModel.LevelCompleted += OnLevelCompleted;
            _viewModel.PlayerDied += OnPlayerDied;
            _viewModel.GameCompleted += OnGameCompleted;

            // Segnala che il controllo è pronto dopo il caricamento
            Loaded += (_, __) => { _isReady = true; };

            // Aggancio al rendering di WPF (chiamato ad ogni frame)
            CompositionTarget.Rendering += OnRendering;
        }

        // --- Metodo chiamato ad ogni frame di rendering WPF ---
        private void OnRendering(object? sender, System.EventArgs e)
        {
            if (!_isReady) return;
            if (_viewModel.Player == null) return;
            if (_viewModel.Player.Animation == null) return;

            DrawScene(); // ridisegna player, nemici e barra vita
        }

        // --- Metodo richiamato quando il gioco è completato ---
        private void OnGameCompleted()
        {
            // Torna alla schermata principale (Home)
            var main = Window.GetWindow(this) as MainWindow;
            main?.ReturnToHome();
        }

        // --- Disegna tutta la scena sul canvas ---
        private void DrawScene()
        {
            GameCanvas.Children.Clear(); // pulisce frame precedente

            double deltaTime = 0.016; // assume 60 FPS (~16ms per frame)

            // --- PLAYER ---
            if (_viewModel.Player != null && _viewModel.Player.Animation != null)
            {
                var frame = _viewModel.Player.Animation.GetCurrentFrame(deltaTime); // ottieni frame animazione corrente

                var img = new Image
                {
                    Source = frame,
                    Width = 96,
                    Height = 96
                };

                // posiziona immagine sul canvas
                Canvas.SetLeft(img, _viewModel.Player.Position.X);
                Canvas.SetTop(img, _viewModel.Player.Position.Y);
                GameCanvas.Children.Add(img);
            }

            // --- NEMICI ---
            foreach (var enemy in _viewModel.Enemies)
            {
                if (!enemy.IsAlive) continue;       // ignora nemici morti
                if (enemy.Animation == null) continue; // ignora nemici senza animazione

                var frame = enemy.Animation.GetCurrentFrame(deltaTime); // frame corrente nemico

                var img = new Image
                {
                    Source = frame,
                    Width = 96,
                    Height = 96
                };

                Canvas.SetLeft(img, enemy.Position.X);
                Canvas.SetTop(img, enemy.Position.Y);
                GameCanvas.Children.Add(img);
            }

            DrawHealthBar(); // disegna barra vita del player
        }

        // --- Disegna la barra della vita del player ---
        private void DrawHealthBar()
        {
            if (_viewModel.Player == null) return;

            double maxWidth = 200; // larghezza massima barra
            double ratio = Math.Max(0, (double)_viewModel.Player.Stats.CurrentHealth / _viewModel.Player.Stats.MaxHealth);
            double barWidth = maxWidth * ratio; // larghezza barra piena proporzionale alla vita

            double posX = 10;
            double posY = 520;

            // barra vuota (sfondo)
            var empty = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/UI/healthbar_empty.png")),
                Width = maxWidth,
                Height = 20
            };
            Canvas.SetLeft(empty, posX);
            Canvas.SetTop(empty, posY);
            GameCanvas.Children.Add(empty);

            // barra piena (vita corrente)
            var full = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/UI/healthbar_full.png")),
                Width = barWidth,
                Height = 20
            };
            Canvas.SetLeft(full, posX);
            Canvas.SetTop(full, posY);
            GameCanvas.Children.Add(full);
        }

        // --- EVENTI NON IMPLEMENTATI (qui puoi mostrare messaggi o animazioni) ---
        private void OnLevelCompleted() { }
        private void OnPlayerDied() { }

        // Metodo utile per test o binding esterno
        public GameViewModel GetViewModel() => _viewModel;
    }
}