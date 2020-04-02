using System.Collections.Generic;
using Entitas;

namespace Features.SolarSystemFeature
{
    public class HandleObjectHealthSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;

        public HandleObjectHealthSystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Health);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasHealth && 
                entity.health.CurrentHp <= 0;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var gameEntity in entities)
            {
                gameEntity.isReadyToDestroy = true;
            }
        }
    }
}
