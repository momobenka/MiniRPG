using MiniRPG.Services;
using System;
using System.Windows;

namespace MiniRPG.Models
{
    /*
        Classe astratta Enemy.

        "abstract" significa che NON può essere istanziata direttamente
        (non puoi fare new Enemy()).

        Serve come classe base per tutti i tipi di nemici del gioco
        (per esempio: Zombie, Goblin, Skeleton ecc.).

        Questa classe estende Entity, quindi eredita:
        - Position
        - Stats
        - IsAlive
        - TakeDamage()
        - ecc.
    */
    public abstract class Enemy : Entity
    {
        // Distanza dalla quale il nemico può attaccare il player
        public double AttackRange { get; protected set; }

        // Tempo minimo tra un attacco e l'altro (cooldown)
        public double AttackCooldownSeconds { get; protected set; }

        // Servizio che gestisce le animazioni del nemico (walk, attack, death ecc.)
        // Il ? significa che può essere null (nullable reference type)
        public AnimationService? Animation { get; set; }

        /*
            Variabile privata con underscore _attackTimer

            Convenzione comune in C#:
            le variabili private spesso iniziano con _

            Serve per distinguere:
            - variabili interne alla classe
            - proprietà pubbliche
        */
        private double _attackTimer;

        // --- GESTIONE ANIMAZIONE HURT (quando il nemico subisce danno) ---

        // Indica se il nemico è attualmente nello stato "hurt"
        private bool _isHurting;

        // Timer per la durata dell'animazione hurt
        private double _hurtTimer;

        // Durata dell'animazione hurt (in secondi)
        private double _hurtDuration = 0.25;

        /*
            EVENTO Died

            Un event è un sistema di comunicazione tra oggetti.

            Quando il nemico muore:
            -> viene invocato l'evento
            -> altre classi (per esempio GameManager) possono reagire

            Action<Enemy> significa:
            una funzione che riceve un Enemy come parametro
            e non restituisce nulla.

            Il ? significa che l'evento potrebbe essere null
            (cioè nessuno è iscritto all'evento).
        */
        public event Action<Enemy>? Died;

        /*
            COSTRUTTORE DEL NEMICO

            Viene chiamato quando viene creato un nemico.

            Parametri:
            startPosition -> posizione iniziale
            stats -> statistiche del nemico (vita, velocità ecc.)
            width / height -> dimensioni del collider
            attackRange -> distanza attacco
            attackCooldownSeconds -> tempo tra gli attacchi
        */
        protected Enemy(Point startPosition, Stats stats, double width, double height,
                        double attackRange, double attackCooldownSeconds)
            : base(startPosition, stats, width, height) // chiama il costruttore della classe base Entity
        {
            AttackRange = attackRange;
            AttackCooldownSeconds = attackCooldownSeconds;

            // all'inizio il timer attacco è 0 quindi il nemico può attaccare subito
            _attackTimer = 0;
        }

        /*
            Metodo chiamato quando il nemico subisce danno.

            override significa che stiamo modificando il comportamento
            di un metodo definito nella classe base (Entity).
        */
        public override void TakeDamage(int amount)
        {
            // Chiama la logica base (riduzione HP ecc.)
            base.TakeDamage(amount);

            // Se il nemico è morto
            if (!IsAlive)
            {
                // Avvia animazione di morte
                Animation?.SetState(AnimationState.Death);

                // Notifica agli altri sistemi che il nemico è morto
                OnDied();
                return;
            }

            // Attiva animazione Hurt (il nemico è stato colpito)
            _isHurting = true;

            // reset timer
            _hurtTimer = _hurtDuration;

            Animation?.SetState(AnimationState.Hurt);
        }

        /*
            Update viene chiamato ogni frame del gioco.

            deltaTime = tempo passato dall'ultimo frame
            player = riferimento al player
            allEnemies = lista di tutti i nemici
        */
        public virtual void Update(double deltaTime, Player player, List<Enemy> allEnemies)
        {
            if (!IsAlive)
            {
                Animation?.SetState(AnimationState.Death);
                return;
            }

            if (_isHurting)
            {
                _hurtTimer -= deltaTime;
                if (_hurtTimer <= 0) _isHurting = false;
                return;
            }

            // --- MOVIMENTO VERSO IL PLAYER ---
            Vector dir = player.Position - Position;
            if (dir.Length > 0) dir.Normalize();

            // --- SEPARAZIONE DAI NEMICI ---
            Vector separation = new Vector(0, 0);
            double desiredSeparation = 40;
            foreach (var other in allEnemies)
            {
                if (other == this) continue;
                double dist = (other.Position - Position).Length;
                if (dist < desiredSeparation && dist > 0)
                {
                    Vector push = Position - other.Position;
                    push.Normalize();
                    push /= dist;
                    separation += push;
                }
            }

            // --- COMBINAZIONE FORZE ---
            Vector finalDir = dir + separation * 1.5;
            if (finalDir.Length > 0)
            {
                finalDir.Normalize();
                Position += finalDir * Stats.Speed * deltaTime;
                Animation?.SetState(AnimationState.Walk);

                // --- LIMITE MAPPA PER NEMICI ---
                // usa GameSettings per bloccare nemici dentro la mappa
                Position = new Point(
                    Math.Max(0, Math.Min(Position.X, GameSettings.MapWidth - Width)),
                    Math.Max(0, Math.Min(Position.Y, GameSettings.MapHeight - Height))
                );
            }

            // --- SISTEMA DI ATTACCO ---
            _attackTimer -= deltaTime;
            if (CollisionService.CheckCollision(this, player))
            {
                if (_attackTimer <= 0)
                {
                    Animation?.SetState(AnimationState.Attack);
                    player.TakeDamage(Stats.Damage);
                    _attackTimer = AttackCooldownSeconds;
                }
            }
        }

        /*
            Metodo che calcola una forza di separazione
            per tenere distanti i nemici tra loro.

            (Algoritmo simile ai "boids" usati nelle AI di gruppo)
        */
        private Vector GetSeparationForce(List<Enemy> allEnemies)
        {
            double desiredSeparation = 35;
            Vector force = new Vector(0, 0);
            int count = 0;

            foreach (var other in allEnemies)
            {
                if (other == this) continue;

                double dist = (other.Position - this.Position).Length;

                if (dist > 0 && dist < desiredSeparation)
                {
                    Vector diff = this.Position - other.Position;
                    diff.Normalize();
                    diff /= dist;

                    force += diff;
                    count++;
                }
            }

            if (count > 0)
                force /= count;

            return force;
        }

        /*
            Metodo che viene chiamato quando il nemico muore.

            Invoke attiva l'evento Died.
        */
        protected virtual void OnDied()
        {
            Died?.Invoke(this);
        }
    }
}