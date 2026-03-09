using System.Windows;

namespace MiniRPG.Models
{
    // Classe base astratta per tutte le entità del gioco (Player, Enemy, ecc.)
    // Contiene proprietà comuni come posizione, statistiche e dimensioni.
    public abstract class Entity
    {
        // Posizione dell'entità nella mappa (coordinate X e Y)
        public Point Position { get; set; }

        // Oggetto che contiene le statistiche dell'entità (HP, danno, velocità ecc.)
        public Stats Stats { get; protected set; }

        // Dimensioni dell'entità, usate soprattutto per il sistema di collisioni
        public double Width { get; protected set; }
        public double Height { get; protected set; }

        // Proprietà di sola lettura che indica se l'entità è viva.
        // È una proprietà "calcolata": restituisce il contrario di Stats.IsDead.
        public bool IsAlive => !Stats.IsDead;

        // Costruttore della classe base. Viene chiamato dalle classi derivate.
        protected Entity(Point startPosition, Stats stats, double width, double height)
        {
            Position = startPosition;
            Stats = stats;
            Width = width;
            Height = height;
        }

        // Restituisce il rettangolo che rappresenta l'area occupata dall'entità.
        // Serve per controllare collisioni con altre entità.
        public Rect GetBounds()
        {
            return new Rect(Position.X, Position.Y, Width, Height);
        }

        // Metodo virtuale per applicare danno all'entità.
        // "virtual" permette alle classi derivate di modificarne il comportamento.
        public virtual void TakeDamage(int amount)
        {
            Stats.TakeDamage(amount);
        }
    }
}