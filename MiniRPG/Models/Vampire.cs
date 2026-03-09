using System;
using System.Collections.Generic;
using System.Windows;

namespace MiniRPG.Models
{
    // Nemico speciale che esplode quando muore causando danno ad area.
    public class Vampire : Enemy
    {
        // Raggio dell'esplosione
        public double ExplosionRadius { get; private set; }

        // Danno inflitto dall'esplosione
        public int ExplosionDamage { get; private set; }

        // Evento lanciato quando il vampiro esplode
        public event Action<Vampire>? Exploded;

        // Costruttore che definisce le statistiche del vampiro
        public Vampire(Point startPosition)
            : base(
                  startPosition,
                  new Stats(maxHealth: 100, damage: 10, speed: 100), // vampiro più veloce dei goblin
                  width: 64,
                  height: 64,
                  attackRange: 10,
                  attackCooldownSeconds: 2.0)
        {
            // Parametri dell'esplosione
            ExplosionRadius = 60;
            ExplosionDamage = 15;
        }

        // Metodo chiamato quando il nemico muore
        // Override per aggiungere il comportamento di esplosione
        protected override void OnDied()
        {
            base.OnDied();          // mantiene il comportamento base (evento morte)
            Exploded?.Invoke(this); // notifica che il vampiro è esploso
        }

        // Applica il danno dell'esplosione agli oggetti vicini
        public void ApplyExplosion(Player player, IEnumerable<Enemy> enemies)
        {
            // Distanza tra vampiro e player
            double distPlayer = (player.Position - Position).Length;

            // Se il player è nel raggio dell'esplosione subisce danno
            if (distPlayer <= ExplosionRadius)
            {
                player.TakeDamage(ExplosionDamage);
            }

            // Controlla tutti i nemici
            foreach (var enemy in enemies)
            {
                if (enemy == this) continue;   // ignora se stesso
                if (enemy is Vampire) continue; // i vampiri non si danneggiano tra loro

                // Distanza dal punto dell'esplosione
                double dist = (enemy.Position - Position).Length;

                // Se è nel raggio subisce danno
                if (dist <= ExplosionRadius)
                {
                    enemy.TakeDamage(ExplosionDamage);
                }
            }
        }
    }
}