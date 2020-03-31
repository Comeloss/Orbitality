using UnityEngine;
using UniRx;

namespace Views
{
    public class CannonView : ReactiveViewBase
    {
#pragma warning disable 0649
        [SerializeField] private ProjectileView _projectile;
#pragma warning restore 0649

        private float _projectileSize = 1;
        private Color _projectileColour = Color.white;
        
        protected override void ViewEnable()
        {
            ObserveEntityWithComponents(
                GameMatcher.Projectile,
                entity => entity.projectile,
                entity => entity.PlanetId.ToString() == ViewId,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(LaunchProjectile);
            
            ObserveEntityWithComponents(
                GameMatcher.ProjectileCannon,
                entity => entity,
                entity => entity.hasProjectileCannon && entity.projectileCannon.CannonId.ToString() == ViewId,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(InitCannon);
        }

        public void Init()
        {
            ViewEnable();
        }

        private void InitCannon(GameEntity entity)
        {
            _projectileSize = entity.projectileCannon.ProjectileSize;
            _projectileColour = entity.projectileCannon.ProjectileColour;
        }

        private void LaunchProjectile(ProjectileComponent component)
        {
            var projectile = Instantiate(_projectile, transform.parent);
            projectile.Init(component.PlanetId, component.ProjectileId, _projectileSize, _projectileColour);
        }
    }
}