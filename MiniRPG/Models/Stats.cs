using System;

namespace MiniRPG.Models
{
    // Classe che contiene tutte le statistiche di un'entità
    // (Player o Enemy): vita, danno e velocità.
    public class Stats
    {
        // Vita massima dell'entità
        public int MaxHealth { get; private set; }

        // Vita attuale
        public int CurrentHealth { get; private set; }

        // Danno inflitto negli attacchi
        public int Damage { get; private set; }

        // Velocità di movimento
        public int Speed { get; private set; }

        // Costruttore: inizializza tutte le statistiche.
        // La vita corrente parte sempre dalla vita massima.
        public Stats(int maxHealth, int damage, int speed)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            Damage = damage;
            Speed = speed;
        }

        // Riduce la vita quando l'entità subisce danno.
        // La vita non può scendere sotto 0.
        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;

            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        // Cura l'entità aumentando la vita.
        // Non può superare la vita massima.
        public void Heal(int amount)
        {
            CurrentHealth += amount;

            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }

        // Proprietà che indica se l'entità è morta.
        public bool IsDead => CurrentHealth <= 0;

        // Aumenta il danno dell'entità (usato per upgrade).
        public void IncreaseDamage(int amount)
        {
            Damage += amount;
        }

        // Aumenta la vita massima e riporta la vita corrente al massimo.
        public void IncreaseMaxHealth(int amount)
        {
            MaxHealth += amount;
            CurrentHealth = MaxHealth;
        }

        // Aumenta la velocità di movimento.
        public void IncreaseSpeed(int amount)
        {
            Speed += amount;
        }
    }
}