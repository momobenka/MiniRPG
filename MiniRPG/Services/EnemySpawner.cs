using System.Collections.Generic;
using System.Windows;
using MiniRPG.Models;

namespace MiniRPG.Services
{
    // Classe responsabile della generazione dei nemici nella mappa
    public class EnemySpawner
    {
        // Dimensioni della mappa di gioco
        private readonly double _mapWidth;
        private readonly double _mapHeight;

        public EnemySpawner(double mapWidth, double mapHeight)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
        }

        // Crea e restituisce una lista di nemici
        public List<Enemy> SpawnEnemies(int goblinCount, int vampireCount)
        {
            var enemies = new List<Enemy>();

            // Margine per evitare spawn troppo vicino ai bordi
            double margin = 50;

            // Spawn dei goblin
            for (int i = 0; i < goblinCount; i++)
            {
                var pos = new Point(
                    margin + (i * 40) % (_mapWidth - 2 * margin),
                    margin + ((i * 80) % (_mapHeight - 2 * margin))
                );

                enemies.Add(new Goblin(pos));
            }

            // Spawn dei vampiri
            for (int i = 0; i < vampireCount; i++)
            {
                var pos = new Point(
                    _mapWidth - margin - (i * 60) % (_mapWidth - 2 * margin),
                    _mapHeight - margin - ((i * 90) % (_mapHeight - 2 * margin))
                );

                enemies.Add(new Vampire(pos));
            }

            return enemies;
        }
    }
}