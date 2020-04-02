using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Features.CannonsFeature
{
    public class ShootProjectileSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public ShootProjectileSystem(Contexts context) : base(context.input)
        {
            _contexts = context;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.TriggerFire);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasTriggerFire &&
                   _contexts.game.hasGamePlayState &&
                   _contexts.game.gamePlayState.CurrentState == GamePlayStateComponent.GamePlayStateType.Play;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var inputEntity in entities)
            {
                var cannon = _contexts.game.GetGroup(GameMatcher.ProjectileCannon)
                    .FirstOrDefault(x => x.projectileCannon.CannonId == inputEntity.triggerFire.PlanetId && x.hasPosition);
                
                if (cannon == null || (cannon.hasCooldown && cannon.cooldown.IsCoolingDown))
                {
                    continue;
                }

                if (!cannon.hasProjectilesTotalShoot)
                {
                    cannon.ReplaceProjectilesTotalShoot(0);
                }

                var projectileId = cannon.projectilesTotalShoot.Count;
                var projectile = _contexts.game.CreateEntity();
                projectile.ReplaceProjectile(cannon.projectileCannon.CannonId, projectileId, cannon.projectileCannon.ProjectileDamage);
                projectile.ReplacePosition(cannon.position.Position);
                cannon.ReplaceProjectilesTotalShoot(projectileId + 1);

                var projectileStartVelocity = (cannon.hasVelocity ? cannon.velocity.Velocity : Vector2.left) * cannon.projectileCannon.ProjectileSpeed;
                        
                projectile.ReplaceStartVelocity(projectileStartVelocity);
                        
                projectile.ReplaceMass(cannon.projectileCannon.ProjectileMass);
                projectile.ReplaceLifeTime(cannon.projectileCannon.ProjectileMaxLifeTime);

                var cooldown = cannon.projectileCannon.ProjectileCooldown;
                
                cannon.ReplaceCooldown(cooldown, cooldown, true);
            }
        }
    }
}