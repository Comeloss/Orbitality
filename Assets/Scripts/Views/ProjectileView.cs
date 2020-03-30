using Entitas.VisualDebugging.Unity;
using UnityEngine;
using UniRx;
using Views.SolarSystem;

namespace Views
{
    public class ProjectileView : ReactiveViewBase
    {
        private int _projectilePlanetId;
        private int _projectileId;

#pragma warning disable 0649
        [SerializeField] private SpriteRenderer _projectileSprite;
        [SerializeField] private Transform _gravityVector;
#pragma warning restore 0649

        protected override void OnEnable()
        {
            ObserveEntityWithComponents(
                GameMatcher.Position,
                entity => entity,
                entity => ValidateProjectile(entity),
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdateProjectilePositionData);

            ObserveEntityWithComponents(
                GameMatcher.ReadyToDestroy,
                entity => entity,
                entity => ValidateProjectile(entity),
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(DestroyProjectile);

            /*ObserveEntityWithComponents(
                GameMatcher.GravityVelocity,
                entity => entity,
                entity => ValidateProjectile(entity),
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdateGravityVectorData);*/
        }

        public void Init(int projectilePlanetId, int projectileId, float size, Color col)
        {
            _projectilePlanetId = projectilePlanetId;
            _projectileId = projectileId;
            transform.localScale = Vector2.one * size;
            
        }

        private void SetProjectileColour(Color col)
        {
            if (!_projectileSprite)
            {
                return;
            }

            _projectileSprite.color = col;
        }

        private void UpdateGravityVectorData(GameEntity entity)
        {
            if (!entity.hasGravityVelocity || !_gravityVector)
            {
                return;
            }

            var scale = _gravityVector.localScale;
            scale.x = entity.gravityVelocity.Velocity.magnitude;
            _gravityVector.localScale = scale;

            float angle = Vector2.SignedAngle(entity.gravityVelocity.Velocity, Vector3.right) * -1;

            var rot = _gravityVector.rotation;
            var rotAngle = rot.eulerAngles;
            rotAngle.z = angle;
            rot.eulerAngles = rotAngle;
            _gravityVector.rotation = rot;
        }

        private void UpdateProjectilePositionData(GameEntity entity)
        {
            if (!entity.hasPosition)
            {
                return;
            }

            transform.position = entity.position.Position;
        }

        private void DestroyProjectile(GameEntity entity)
        {
            gameObject.DestroyGameObject();
        }

        private bool ValidateProjectile(GameEntity entity)
        {
            return entity.hasProjectile &&
                   entity.projectile.PlanetId == _projectilePlanetId &&
                   entity.projectile.ProjectileId == _projectileId;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var planetView = other.transform.parent.GetComponent<PlanetView>();
            if (planetView != null)
            {
                Contexts.sharedInstance.input.CreateEntity()
                    .ReplaceCollision(_projectileId, _projectilePlanetId, planetView.GetPlanetId());
            }
        }
    }
}