using System;
using System.Collections.Generic;

namespace MiniRPG.Models
{
    // Gestisce la progressione dei livelli del gioco:
    // livello attuale, ricompense e numero di nemici da generare.
    public class LevelManager
    {
        // Livello corrente del gioco. Può essere modificato solo dalla classe stessa.
        public int CurrentLevel { get; private set; } = 1;

        // Numero massimo di livelli previsti dal gioco.
        public int MaxLevel { get; } = 10;

        // Indica se il gioco è completato (quando si supera l'ultimo livello).
        public bool IsGameCompleted => CurrentLevel > 10;

        // Numero di monete guadagnate al livello 1.
        public int BaseCoinsLevel1 { get; } = 5;

        // Incremento delle monete per ogni livello successivo.
        public int CoinsIncrementPerLevel { get; } = 10;

        // Calcola quante monete riceve il giocatore al completamento del livello corrente.
        public int GetCoinsForCurrentLevel()
        {
            // Livello 1: 5
            // Livello 2: 15
            // Livello 3: 25 ...
            return BaseCoinsLevel1 + (CurrentLevel - 1) * CoinsIncrementPerLevel;
        }

        // Passa al livello successivo.
        public void AdvanceLevel()
        {
            CurrentLevel++;
        }

        // Riporta il gioco al livello iniziale.
        public void Reset()
        {
            CurrentLevel = 1;
        }

        // Restituisce quanti nemici generare nel livello corrente.
        // Usa una tuple (goblinCount, vampireCount) per restituire due valori.
        public (int goblinCount, int vampireCount) GetEnemyCountsForCurrentLevel()
        {
            // Livelli 1-5: solo goblin con numero crescente
            // Livelli 6-10: goblin + vampiri

            if (CurrentLevel <= 5)
            {
                // Numero di goblin aumenta di 2 ogni livello
                int goblins = 3 + (CurrentLevel - 1) * 2; // 3,5,7,9,11
                return (goblins, 0);
            }
            else
            {
                // Dal livello 6 iniziano ad apparire i vampiri
                int levelIndex = CurrentLevel - 5; // trasforma livelli 6..10 in 1..5

                int goblins = 10 + levelIndex * 2; // goblin continuano ad aumentare
                int vampires = levelIndex; // numero di vampiri cresce gradualmente

                return (goblins, vampires);
            }
        }
    }
}