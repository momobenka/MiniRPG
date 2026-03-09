using System;
using System.Diagnostics;

namespace MiniRPG.Utils
{
    // Timer semplice che invia eventi Tick a intervalli regolari
    public class Timer
    {
        private readonly double _intervalMilliseconds; // intervallo tra Tick
        private readonly Stopwatch _stopwatch;         // misura il tempo reale trascorso
        private bool _running;                         // flag se timer attivo
        private double _accumulator;                   // accumula tempo non ancora processato

        public event Action<double>? Tick; // evento: deltaTime in secondi

        public Timer(double intervalMilliseconds)
        {
            _intervalMilliseconds = intervalMilliseconds;
            _stopwatch = new Stopwatch();
        }

        // Avvia il timer e registra l'evento Rendering di WPF
        public void Start()
        {
            if (_running) return;
            _running = true;
            _accumulator = 0;
            _stopwatch.Restart();

            System.Windows.Media.CompositionTarget.Rendering += OnRendering;
        }

        // Ferma il timer
        public void Stop()
        {
            if (!_running) return;
            _running = false;
            System.Windows.Media.CompositionTarget.Rendering -= OnRendering;
            _stopwatch.Stop();
        }

        // Richiamato ad ogni frame WPF
        private void OnRendering(object? sender, EventArgs e)
        {
            if (!_running) return;

            double elapsed = _stopwatch.Elapsed.TotalMilliseconds; // tempo trascorso dall'ultimo frame
            _stopwatch.Restart();

            _accumulator += elapsed;

            // Invoca Tick più volte se necessario per recuperare il tempo accumulato
            while (_accumulator >= _intervalMilliseconds)
            {
                double deltaTimeSeconds = _intervalMilliseconds / 1000.0;
                Tick?.Invoke(deltaTimeSeconds);
                _accumulator -= _intervalMilliseconds;
            }
        }
    }
}