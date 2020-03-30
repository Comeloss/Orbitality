using System.Collections.Generic;
using Entitas;

namespace Features.Game.Gravity
{
    public class UpdateVelocitySystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
    
        public UpdateVelocitySystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.StartVelocity);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasStartVelocity;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            //var velocityObjects = _contexts.game.GetGroup(GameMatcher.OrbitalVelocity);//.Where(x => x.hasPosition);
            
            foreach (var gameEntity in entities)
            {
                var resultVelocity =  gameEntity.startVelocity.Velocity;

                if (gameEntity.hasGravityVelocity)
                {
                    resultVelocity += gameEntity.gravityVelocity.Velocity;
                }

                gameEntity.ReplaceVelocity(resultVelocity);
            }
        }
    }
}
