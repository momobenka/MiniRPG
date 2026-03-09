using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRPG.Models
{
    /*
        Classe statica che contiene impostazioni globali del gioco.
        In questo caso contiene le dimensioni della mappa di gioco.
        Tutti i player e i nemici possono usarla per non uscire dai bordi.
    */
    public static class GameSettings
    {
        public static double MapWidth { get; set; } = 800;   // larghezza mappa
        public static double MapHeight { get; set; } = 540;  // altezza mappa
    }
}
