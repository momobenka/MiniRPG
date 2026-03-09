using MiniRPG.Utils;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiniRPG.Services
{
    // Stati possibili delle animazioni dei personaggi
    public enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Death
    }

    // Gestisce il cambio e l'avanzamento delle animazioni tramite sprite sheet
    public class AnimationService
    {
        // SpriteSheet per ogni stato dell'animazione
        private readonly SpriteSheet _idle;
        private readonly SpriteSheet _walk;
        private readonly SpriteSheet _attack;
        private readonly SpriteSheet _hurt;
        private readonly SpriteSheet _death;

        // Stato attuale dell'animazione
        private AnimationState _currentState = AnimationState.Idle;

        // Frame corrente nello sprite sheet
        private int _currentFrame = 0;

        // Timer usato per controllare quando passare al frame successivo
        private double _frameTimer = 0;

        // Durata di ogni frame (più grande = animazione più lenta)
        private double _frameDuration = 0.12;

        // Costruttore: riceve tutti gli sprite sheet delle animazioni
        public AnimationService(
            SpriteSheet idle,
            SpriteSheet walk,
            SpriteSheet attack,
            SpriteSheet hurt,
            SpriteSheet death)
        {
            _idle = idle;
            _walk = walk;
            _attack = attack;
            _hurt = hurt;
            _death = death;
        }

        // Cambia lo stato dell'animazione
        public void SetState(AnimationState newState)
        {
            if (_currentState != newState)
            {
                _currentState = newState;

                // Reset animazione
                _currentFrame = 0;
                _frameTimer = 0;
            }
        }

        // Restituisce il frame corrente da disegnare
        public ImageSource GetCurrentFrame(double deltaTime)
        {
            // Seleziona lo sprite sheet in base allo stato
            SpriteSheet sheet = _currentState switch
            {
                AnimationState.Idle => _idle,
                AnimationState.Walk => _walk,
                AnimationState.Attack => _attack,
                AnimationState.Hurt => _hurt,
                AnimationState.Death => _death,
                _ => _idle
            };

            // Aggiorna timer del frame
            _frameTimer += deltaTime;

            if (_frameTimer >= _frameDuration)
            {
                _frameTimer = 0;
                _currentFrame++;

                // Se finisce l'animazione
                if (_currentFrame >= sheet.TotalFrames)
                {
                    if (_currentState == AnimationState.Attack ||
                        _currentState == AnimationState.Hurt)
                    {
                        // Dopo Attack o Hurt torna a Idle
                        SetState(AnimationState.Idle);
                    }
                    else if (_currentState == AnimationState.Death)
                    {
                        // Rimane sull'ultimo frame della morte
                        _currentFrame = sheet.TotalFrames - 1;
                    }
                    else
                    {
                        // Loop normale (Idle o Walk)
                        _currentFrame = 0;
                    }
                }
            }

            // Restituisce l'immagine del frame corrente
            return sheet.GetFrame(_currentFrame);
        }
    }
}