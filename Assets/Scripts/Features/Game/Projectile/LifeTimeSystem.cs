using System.Collections.Generic;
using System.Linq;
using Entitas;

namespace Features.Game.Projectile
{
    public class LifeTimeSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;

        public LifeTimeSystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.GameDeltaTime);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameDeltaTime;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var timedEntities = _contexts.game.GetGroup(GameMatcher.LifeTime);

            foreach (var gameEntity in timedEntities)
            {
                var timeLeft = gameEntity.lifeTime.TimeLeft - _contexts.game.gameDeltaTime.TimeDelta;
                if (timeLeft < 0)
                {
                    timeLeft = 0;
                }

                gameEntity.ReplaceLifeTime(timeLeft);
            }
        }
    }
}