using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Features.Game.Gravity
{
    public class UpdateObjectsPositionSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
        
        public UpdateObjectsPositionSystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.GameTimeMilliseconds);
        }

        protected override bool Filter(GameEntity entity)
        {
            return true;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var velocityObjects = _contexts.game.GetGroup(GameMatcher.Velocity).Where(x => x.hasPosition);
            
            foreach (var gameEntity in velocityObjects)
            {
                var newPos = gameEntity.position.Position + gameEntity.velocity.Velocity;
                
                gameEntity.ReplacePosition(newPos);
            }
        }
    }
}
