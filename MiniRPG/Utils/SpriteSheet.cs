using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiniRPG.Utils
{
    // Gestisce uno sprite sheet e permette di estrarre i singoli frame come immagini
    public class SpriteSheet
    {
        private readonly BitmapImage _image; // immagine completa dello sprite sheet
        private readonly int _columns;        // numero di colonne nello sprite sheet
        private readonly int _rows;           // numero di righe nello sprite sheet
        private readonly int _totalFrames;    // numero totale di frame validi

        private readonly double _frameWidth;  // larghezza di un singolo frame
        private readonly double _frameHeight; // altezza di un singolo frame

        public int TotalFrames => _totalFrames; // proprietà pubblica: numero totale frame

        // Costruttore: carica immagine e calcola dimensioni frame
        public SpriteSheet(string relativePath, int columns, int rows, int totalFrames)
        {
            _image = new BitmapImage(new Uri($"pack://application:,,,/{relativePath}", UriKind.Absolute));
            _columns = columns;
            _rows = rows;
            _totalFrames = totalFrames;

            _frameWidth = _image.PixelWidth / (double)_columns;
            _frameHeight = _image.PixelHeight / (double)_rows;
        }

        // Restituisce il frame richiesto come ImageSource
        public ImageSource GetFrame(int index)
        {
            if (index < 0 || index >= _totalFrames)
                index = 0; // sicurezza: frame fuori range → ritorna 0

            int frameWidth = (int)_frameWidth;
            int frameHeight = (int)_frameHeight;

            int col = index % _columns; // calcola colonna del frame
            int row = index / _columns; // calcola riga del frame

            if (row >= _rows)
                row = _rows - 1; // sicurezza: ultima riga valida

            var rect = new Int32Rect(
                col * frameWidth,
                row * frameHeight,
                frameWidth,
                frameHeight
            );

            return new CroppedBitmap(_image, rect); // ritaglia e ritorna il frame
        }
    }
}