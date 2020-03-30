using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Features.Game.Projectile
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
            return true;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var inputEntity in entities)
            {
                if (!inputEntity.hasTriggerFire)
                {
                    continue;
                }

                var cannon = _contexts.game.GetGroup(GameMatcher.ProjectileCannon)
                    .FirstOrDefault(x => x.projectileCannon.CannonId == inputEntity.triggerFire.PlaneId && x.hasPosition);

                if (cannon == null)
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
                        
                cannon.ReplaceCannonCooldown(cannon.projectileCannon.ProjectileCooldown);
            }
        }
    }
}