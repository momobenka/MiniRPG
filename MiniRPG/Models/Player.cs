using MiniRPG.Services;
using MiniRPG.Utils;
using System.Windows;

namespace MiniRPG.Models
{
    // Classe Player. Rappresenta il personaggio controllato dal giocatore.
    // Eredita da Entity e aggiunge logiche di movimento, attacco, monete e potenziamenti.
    public class Player : Entity
    {
        // Numero di monete possedute dal giocatore
        public int Coins { get; private set; }

        // Servizio che gestisce le animazioni del player
        public AnimationService? Animation { get; set; }

        // Velocità corrente del player (direzione + intensità del movimento)
        private Vector _velocity = new Vector(0, 0);

        // Accelerazione quando il giocatore preme un tasto di movimento
        private const double Acceleration = 600;

        // Attrito che rallenta il player quando non si muove
        private const double Friction = 500;

        // Rigenerazione della vita (HP per secondo)
        private double _regenRate = 5;
        private double _regenTimer = 0;

        // Indica se il player sta eseguendo un attacco
        public bool IsAttacking { get; private set; }

        // Timer che determina quanto dura l'attacco
        private double _attackTimer;

        // Durata dell'attacco
        private double _attackDuration = 0.5;

        // Costruttore del player
        public Player(Point startPosition, Stats stats, double width, double height)
            : base(startPosition, stats, width, height)
        {
            Coins = 0;
        }

        // Metodo chiamato ogni frame per aggiornare stato del player
        public void Update(double deltaTime)
        {
            // Gestione durata attacco
            if (IsAttacking)
            {
                _attackTimer -= deltaTime;

                if (_attackTimer <= 0)
                    IsAttacking = false;
            }

            // Rigenerazione automatica della vita
            if (Stats.CurrentHealth < Stats.MaxHealth)
            {
                _regenTimer += deltaTime;

                // Ogni secondo recupera HP
                if (_regenTimer >= 1.0)
                {
                    Stats.Heal((int)_regenRate);
                    _regenTimer = 0;
                }
            }
        }

        // Gestisce il movimento del player
        public void Move(Vector input, double deltaTime)
        {
            if (!IsAlive) return;
            if (IsAttacking) return; // durante l’attacco non ti muovi

            if (input.Length > 0)
            {
                input.Normalize();
                _velocity += input * Acceleration * deltaTime;
            }
            else
            {
                if (_velocity.Length > 0)
                {
                    Vector friction = -_velocity;
                    friction.Normalize();
                    friction *= Friction * deltaTime;

                    if (friction.Length > _velocity.Length)
                        _velocity = new Vector(0, 0);
                    else
                        _velocity += friction;
                }
            }

            if (_velocity.Length > Stats.Speed)
            {
                _velocity.Normalize();
                _velocity *= Stats.Speed;
            }

            // Aggiorna posizione
            Position = new Point(
                Position.X + _velocity.X * deltaTime,
                Position.Y + _velocity.Y * deltaTime
            );

            // --- LIMITE MAPPA ---
            // usa GameSettings per bloccare il player dentro la mappa
            Position = new Point(
                Math.Max(0, Math.Min(Position.X, GameSettings.MapWidth - Width)),
                Math.Max(0, Math.Min(Position.Y, GameSettings.MapHeight - Height))
            );
        }

        // Avvia l'attacco del player
        public void StartAttack()
        {
            if (!IsAlive) return;
            if (IsAttacking) return;

            IsAttacking = true;
            _attackTimer = _attackDuration;

            // Avvia animazione di attacco
            Animation?.SetState(AnimationState.Attack);
        }

        // Aggiunge monete al giocatore
        public void AddCoins(int amount)
        {
            Coins += amount;
        }

        // Spende monete se il giocatore ne ha abbastanza
        public bool SpendCoins(int amount)
        {
            if (Coins < amount) return false;

            Coins -= amount;
            return true;
        }

        // Potenziamento del danno
        public void UpgradeDamage(int cost, int damageIncrease)
        {
            if (SpendCoins(cost))
                Stats.IncreaseDamage(damageIncrease);
        }

        // Potenziamento della vita massima
        public void UpgradeHealth(int cost, int healthIncrease)
        {
            if (SpendCoins(cost))
                Stats.IncreaseMaxHealth(healthIncrease);
        }

        // Potenziamento della velocità
        public void UpgradeSpeed(int cost, int speedIncrease)
        {
            if (SpendCoins(cost))
                Stats.IncreaseSpeed(speedIncrease);
        }
    }
}