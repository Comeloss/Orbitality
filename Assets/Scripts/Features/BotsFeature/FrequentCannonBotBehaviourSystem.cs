using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Features.BotsFeature
{
    public class FrequentCannonBotBehaviourSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
        
        public FrequentCannonBotBehaviourSystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.BotAi);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasBotAi &&
                   entity.botAi.CanAct &&
                   entity.hasPosition &&
                   entity.hasPlanetInfo &&
                entity.hasProjectileCannon &&
                entity.hasProjectileCannonType && 
                entity.projectileCannonType.Type == ProjectileCannonType.Frequent;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var targets = _contexts.game.GetGroup(GameMatcher.PlanetInfo).Where(p => p.hasHealth && p.hasPosition);
            
            foreach (var bot in entities)
            {
                if (bot.hasCooldown && bot.cooldown.IsCoolingDown)
                {
                    continue;
                }

                var canShoot = CheckTargetIsReachable(bot, targets);

                if (canShoot)
                {
                    _contexts.input.CreateEntity().ReplaceTriggerFire(bot.planetInfo.Id);
                }
            }
        }

        private bool CheckTargetIsReachable(GameEntity entity, IEnumerable<GameEntity> targets)
        {
            if (!entity.hasVelocity) //TODO: replace with forward direction
            {
                return false;
            }

            foreach (var target in targets)
            {
                var distance = Vector2.Distance(entity.position.Position, target.position.Position);

                if (distance < 600)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

