using System;
using System.Collections.Generic;
using MiniRPG.Models;
using MiniRPG.Utils;
using Timer = MiniRPG.Utils.Timer;

namespace MiniRPG.Services
{
    // Classe principale che gestisce il ciclo del gioco (game loop)
    // Aggiorna player, nemici, livelli e controlla condizioni di fine partita.
    public class GameLoop
    {
        // Timer che esegue l'update del gioco a intervalli regolari
        private readonly Timer _timer;

        // Servizi utilizzati dal gioco
        private readonly InputService _inputService;
        private readonly EnemySpawner _enemySpawner;
        private readonly LevelManager _levelManager;

        // Riferimento al player
        public Player? Player { get; private set; }

        // Lista dei nemici attivi nel livello
        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();

        // Eventi per comunicare con l'interfaccia o altri sistemi
        public event Action? LevelCompleted;
        public event Action? PlayerDied;
        public event Action? GameCompleted;

        public GameLoop(InputService inputService,
                        EnemySpawner enemySpawner,
                        LevelManager levelManager)
        {
            _inputService = inputService;
            _enemySpawner = enemySpawner;
            _levelManager = levelManager;

            // Timer con intervallo ~16ms (circa 60 FPS)
            _timer = new Timer(16);
            _timer.Tick += OnTick; // ogni tick chiama OnTick
        }

        // Avvia una nuova partita
        public void StartNewGame()
        {
            _levelManager.Reset();

            // Creazione del player
            Player = new Player(
                new System.Windows.Point(200, 200),
                new Stats(100, 30, 120),
                64,
                64
            );

            // Imposta animazioni del player
            Player.Animation = new AnimationService(
                new SpriteSheet("Assets/Sprites/Warrior/idle.png", 12, 4, 48),
                new SpriteSheet("Assets/Sprites/Warrior/walk.png", 6, 4, 24),
                new SpriteSheet("Assets/Sprites/Warrior/attack.png", 8, 4, 32),
                new SpriteSheet("Assets/Sprites/Warrior/hurt.png", 5, 4, 20),
                new SpriteSheet("Assets/Sprites/Warrior/death.png", 7, 4, 28)
            );

            LoadLevel();
            _timer.Start();
        }

        // Carica i nemici del livello corrente
        private void LoadLevel()
        {
            Enemies.Clear();

            var (goblins, vampires) = _levelManager.GetEnemyCountsForCurrentLevel();

            // Genera i nemici
            Enemies = _enemySpawner.SpawnEnemies(goblins, vampires);

            foreach (var enemy in Enemies)
            {
                if (enemy is Goblin)
                {
                    // Animazioni goblin
                    enemy.Animation = new AnimationService(
                        new SpriteSheet("Assets/Sprites/Goblin/idle.png", 4, 4, 16),
                        new SpriteSheet("Assets/Sprites/Goblin/walk.png", 6, 4, 24),
                        new SpriteSheet("Assets/Sprites/Goblin/attack.png", 8, 4, 32),
                        new SpriteSheet("Assets/Sprites/Goblin/hurt.png", 6, 4, 24),
                        new SpriteSheet("Assets/Sprites/Goblin/death.png", 8, 4, 32)
                    );
                }
                else if (enemy is Vampire vampire)
                {
                    // Animazioni vampiro
                    enemy.Animation = new AnimationService(
                        new SpriteSheet("Assets/Sprites/Vampire/idle.png", 4, 4, 16),
                        new SpriteSheet("Assets/Sprites/Vampire/walk.png", 6, 4, 24),
                        new SpriteSheet("Assets/Sprites/Vampire/attack.png", 12, 4, 48),
                        new SpriteSheet("Assets/Sprites/Vampire/hurt.png", 4, 4, 16),
                        new SpriteSheet("Assets/Sprites/Vampire/death.png", 11, 4, 44)
                    );

                    // Evento esplosione vampiro
                    vampire.Exploded += OnVampireExploded;
                }
            }
        }

        // Quando un vampiro muore ed esplode
        private void OnVampireExploded(Vampire vampire)
        {
            if (Player != null)
                vampire.ApplyExplosion(Player, Enemies);
        }

        // Metodo principale eseguito ogni frame del gioco
        private void OnTick(double deltaTime)
        {
            if (Player == null) return;

            // Controlla morte del player
            if (!Player.IsAlive)
            {
                _timer.Stop();
                PlayerDied?.Invoke();
                return;
            }

            // Aggiorna stato player (timer attacco, rigenerazione ecc.)
            Player.Update(deltaTime);

            // Legge input di movimento
            var dir = _inputService.GetMovementDirection();

            // Se non sta attaccando può muoversi o iniziare un attacco
            if (!Player.IsAttacking)
            {
                Player.Move(dir, deltaTime);

                if (_inputService.IsAttackPressed())
                {
                    Player.StartAttack();

                    // Danno ai nemici in collisione
                    foreach (var enemy in Enemies)
                    {
                        if (enemy.IsAlive && CollisionService.CheckCollision(Player, enemy))
                            enemy.TakeDamage(Player.Stats.Damage);
                    }
                }
            }

            // Gestione animazione del player
            if (!Player.IsAlive)
            {
                Player.Animation?.SetState(AnimationState.Death);
            }
            else if (Player.IsAttacking)
            {
                Player.Animation?.SetState(AnimationState.Attack);
            }
            else if (dir.Length > 0)
            {
                Player.Animation?.SetState(AnimationState.Walk);
            }
            else
            {
                Player.Animation?.SetState(AnimationState.Idle);
            }

            // Aggiorna tutti i nemici
            foreach (var enemy in Enemies)
            {
                if (enemy.IsAlive)
                    enemy.Update(deltaTime, Player, Enemies);
            }

            // Rimuove i nemici morti dalla lista
            Enemies.RemoveAll(e => !e.IsAlive);

            // Se non ci sono più nemici il livello è completato
            if (Enemies.Count == 0)
            {
                Player.AddCoins(_levelManager.GetCoinsForCurrentLevel());

                _levelManager.AdvanceLevel();
                _timer.Stop();

                if (_levelManager.IsGameCompleted)
                    GameCompleted?.Invoke();
                else
                    LevelCompleted?.Invoke();
            }
        }

        // Riprende il gioco caricando il livello successivo
        public void ResumeNextLevel()
        {
            if (_levelManager.IsGameCompleted)
                return;

            LoadLevel();
            _timer.Start();
        }

        // Ferma completamente il game loop
        public void Stop()
        {
            _timer.Stop();
        }
    }
}