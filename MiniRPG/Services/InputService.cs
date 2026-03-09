using System.Windows;
using System.Windows.Input;

namespace MiniRPG.Services
{
    // Gestisce l'input da tastiera del giocatore
    public class InputService
    {
        // Restituisce la direzione di movimento in base ai tasti premuti
        public Vector GetMovementDirection()
        {
            Vector dir = new Vector(0, 0);

            // Controllo tasti movimento (WASD o frecce)
            if (Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up))
                dir.Y -= 1;

            if (Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.Down))
                dir.Y += 1;

            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left))
                dir.X -= 1;

            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right))
                dir.X += 1;

            return dir;
        }

        // Controlla se il giocatore sta premendo il tasto di attacco
        public bool IsAttackPressed()
        {
            return Keyboard.IsKeyDown(Key.Space);
        }
    }
}