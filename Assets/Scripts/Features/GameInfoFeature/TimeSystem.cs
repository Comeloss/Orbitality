using Entitas;
using UnityEngine;

namespace Features.GameInfoFeature
{
    public class TimeSystem : IExecuteSystem, IInitializeSystem
    {
        private readonly Contexts _contexts;
    
        public TimeSystem(Contexts contexts)
        {
            _contexts = contexts;
        }
        
        public void Initialize()
        {
            _contexts.game.ReplaceGameTime(0);
            _contexts.game.ReplaceGameTimeMilliseconds(0);
            _contexts.game.ReplaceGameDeltaTime(Time.time);
        }
        
        public void Execute()
        {
            if (!_contexts.game.hasGamePlayState || _contexts.game.gamePlayState.CurrentState == GamePlayStateComponent.GamePlayStateType.Pause)
            {
                return;
            }

            _contexts.game.ReplaceGameDeltaTime(Time.deltaTime);

            var gameTimeMilliseconds = _contexts.game.gameTimeMilliseconds.Time + _contexts.game.gameDeltaTime.TimeDelta; 
            
            _contexts.game.ReplaceGameTimeMilliseconds(gameTimeMilliseconds);
            
            if (gameTimeMilliseconds - _contexts.game.gameTime.Time >= 1f)
            {
                _contexts.game.ReplaceGameTime((long)gameTimeMilliseconds);
            }
        }
    }
}
