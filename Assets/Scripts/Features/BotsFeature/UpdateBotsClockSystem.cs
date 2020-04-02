using System.Collections.Generic;
using Entitas;

namespace Features.BotsFeature
{
    public class UpdateBotsClockSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
        
        public UpdateBotsClockSystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.GameTimeMilliseconds);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameTimeMilliseconds &&
                _contexts.game.hasBotsClock;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            if (_contexts.game.botsClock.TickDuration <= 0)
            {
                return;
            }

            if (_contexts.game.gameTimeMilliseconds.Time - _contexts.game.botsClock.LastTick >
                _contexts.game.botsClock.TickDuration)
            {
                _contexts.game.ReplaceBotsClock(_contexts.game.botsClock.LastTick + 1, _contexts.game.botsClock.TickDuration);
            }

        }
    }
}
