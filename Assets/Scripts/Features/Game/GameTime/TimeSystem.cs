using Entitas;
using UnityEngine;

namespace Features.Game.GameTime
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
            _contexts.game.ReplaceGameDeltaTime(Time.realtimeSinceStartup, 0);
        }
        
        public void Execute()
        {
            _contexts.game.ReplaceGameTimeMilliseconds((long)(Time.realtimeSinceStartup * 1000));
            
            if (Time.realtimeSinceStartup - _contexts.game.gameTime.Time >= 1f)
            {
                _contexts.game.ReplaceGameTime((long)Time.realtimeSinceStartup);
            }
            
            var lastTime = _contexts.game.gameDeltaTime;
            _contexts.game.ReplaceGameDeltaTime(Time.realtimeSinceStartup - lastTime.TimeSinceStartup, Time.realtimeSinceStartup);
        }
    }
}
