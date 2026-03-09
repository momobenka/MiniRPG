using System.Windows;

namespace MiniRPG.Models
{
    // Classe concreta di nemico: Goblin.
    // Eredita da Enemy e definisce semplicemente le sue statistiche base.
    public class Goblin : Enemy
    {
        // Costruttore del Goblin.
        // Riceve la posizione iniziale e passa tutti i parametri al costruttore della classe Enemy.
        public Goblin(Point startPosition)
            : base(
                  startPosition,
                  new Stats(maxHealth: 75, damage: 5, speed: 50), // Statistiche del goblin: vita, danno e velocità
                  width: 64,  // larghezza del collider/sprite
                  height: 64, // altezza del collider/sprite
                  attackRange: 10, // distanza dalla quale può attaccare
                  attackCooldownSeconds: 2.0) // tempo minimo tra due attacchi
        {
        }
    }
}