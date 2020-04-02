using System.Collections.Generic;
using System.Linq;
using Entitas;

namespace Features.SolarSystemFeature
{
    public class CooldownSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;

        public CooldownSystem(Contexts context) : base(context.game)
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
            var cooldownEntities = _contexts.game.GetGroup(GameMatcher.Cooldown).Where(c => c.cooldown.IsCoolingDown);

            foreach (var gameEntity in cooldownEntities)
            {
                var timeLeft = gameEntity.cooldown.TimeLeft - _contexts.game.gameDeltaTime.TimeDelta;

                var cooldown = gameEntity.cooldown.Cooldown;
                
                var ready = timeLeft <= 0;

                if (ready)
                {
                    timeLeft = 0;
                }

                gameEntity.ReplaceCooldown(cooldown, ready ? cooldown : timeLeft, !ready);
            }
        }
    }
}