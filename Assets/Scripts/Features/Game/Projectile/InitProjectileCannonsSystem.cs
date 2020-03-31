using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Features.Game.Projectile
{
    public class InitProjectileCannonsSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
        public InitProjectileCannonsSystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.ProjectileCannonType);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasProjectileCannonType;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var gameEntity in entities)
            {
                var cannonType = gameEntity.projectileCannonType.Type;
                var cannonId = gameEntity.hasPlanetInfo ? gameEntity.planetInfo.Id : GetNewCannonId();
                InitCannon(cannonId, gameEntity, cannonType);
            }
        }

        private void InitCannon(int id, GameEntity entity, ProjectileCannonType type)
        {
            switch (type)
            {
                case ProjectileCannonType.Heavy:
                    entity.ReplaceProjectileCannon(id, 20, 40 ,1.5f, 3, 8, 3, Color.red);
                    break;
                case ProjectileCannonType.Frequent:
                    entity.ReplaceProjectileCannon(id, 4, 10 ,2f, 0.2f, 5, 1f, Color.yellow);
                    break;
                case ProjectileCannonType.Fast:
                    entity.ReplaceProjectileCannon(id, 5, 20 ,3.5f, 1.2f, 3, 2, Color.blue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private int GetNewCannonId()
        {
            //Get ids for cannons without planets
            return -1;
        }
    }   
}

