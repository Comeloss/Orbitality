using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Features.Game.Gravity
{
    public class UpdatePlanetsVelocitySystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;

        private float koef = 0.005f;
        
        public UpdatePlanetsVelocitySystem(Contexts context) : base(context.game)
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
            var planets = _contexts.game.GetGroup(GameMatcher.PlanetTrajectory);
            
            foreach (var gameEntity in planets)
            {
                var trajectory = gameEntity.planetTrajectory;

                var alpa = _contexts.game.gameTimeMilliseconds.Time * koef * trajectory.Speed;
                
                var X = trajectory.OrbitCenterX + (trajectory.OrbitWidthA * Mathf.Cos(alpa));
                var Y= trajectory.OrbitCenterY + (trajectory.OrbitHeightB * Mathf.Sin(alpa));

                var newPosition = new Vector2(X, Y);

                if (!gameEntity.hasPosition)
                {
                    gameEntity.ReplacePosition(newPosition);
                }
                
                var planetVelocity = newPosition - gameEntity.position.Position;
                gameEntity.ReplaceStartVelocity(planetVelocity);
            }
        }
    }
}
