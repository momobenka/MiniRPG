using System.Collections.Generic;
using System.Windows;
using MiniRPG.Models;

namespace MiniRPG.Services
{
    // Servizio statico per gestire le collisioni tra entità
    public static class CollisionService
    {
        // Controlla se due entità si sovrappongono
        public static bool CheckCollision(Entity a, Entity b)
        {
            return a.GetBounds().IntersectsWith(b.GetBounds());
        }

        // Restituisce tutti i nemici che stanno collidendo con il player
        public static IEnumerable<Enemy> GetEnemiesCollidingWithPlayer(Player player, IEnumerable<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                if (CheckCollision(player, enemy))
                {
                    // yield return restituisce un elemento alla volta
                    // senza creare una lista completa in memoria
                    yield return enemy;
                }
            }
        }
    }
}