using UnityEngine;
using UniRx;

namespace Views.SolarSystem
{
    public class PlanetView : ReactiveViewBase
    {
#pragma warning disable 0649
        [SerializeField] private SpriteRenderer _planetImage;
        [SerializeField] private ProgressBar _healthBar;
        [SerializeField] private ProgressBar _cooldownBar;
        [SerializeField] private CannonView _cannonTranform;
        [SerializeField] private Transform _orbitalVector;
        [SerializeField] private Transform _gravityVector;
        [SerializeField] private Transform _resultVector;
#pragma warning restore 0649

        private int _planetId;
        private CannonView _cannon;
        
        protected override void ViewEnable()
        {
            ObserveEntityWithComponents(
                GameMatcher.PlanetInfo,
                entity => entity.planetInfo,
                entit => entit.Id.ToString() == ViewId,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdateInfoData);
            
            ObserveEntityWithComponents(
                GameMatcher.ProjectileCannon,
                entity => entity,
                entit => entit.hasPlanetInfo && entit.planetInfo.Id.ToString() == ViewId,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(CreateCannon);
            
            ObserveEntityWithComponents(
                GameMatcher.Position,
                entity => entity,
                entit => entit.hasPlanetInfo && entit.planetInfo.Id.ToString() == ViewId,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdatePositionData);
            
            ObserveEntityWithComponents(
                GameMatcher.StartVelocity,
                entity => entity,
                entit => entit.hasPlanetInfo && entit.planetInfo.Id.ToString() == ViewId,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdateOrbitalVectorData);
            
            ObserveEntityWithComponents(
                GameMatcher.GravityVelocity,
                entity => entity,
                entit => entit.hasPlanetInfo && entit.planetInfo.Id.ToString() == ViewId,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdateGravityVectorData);
            
            ObserveEntityWithComponents(
                GameMatcher.Velocity,
                entity => entity,
                entit => entit.hasPlanetInfo && entit.planetInfo.Id.ToString() == ViewId,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdateResultVectorData);
        }

        public void Init(int id)
        {
            ViewId = id.ToString();
            _planetId = id;
            
            ViewEnable();
        }

        public int GetPlanetId()
        {
            return _planetId;
        }

        private void UpdateInfoData(PlanetInfoComponent component)
        {
            SetPlanetImageAndSize(component.Image, component.Size);
        }

        private void CreateCannon(GameEntity entity)
        {
            if (!entity.hasProjectileCannon || !_cannonTranform)
            {
                return;
            }

            if (_cannon != null)
            {
                Destroy(_cannon);
            }

            _cannon = Instantiate(_cannonTranform, transform);
            _cannon.ViewId = ViewId;
        }

        private void UpdatePositionData(GameEntity entity)
        {
            if (!entity.hasPosition)
            {
                return;
            }

            transform.position = entity.position.Position;
        }
        
        private void UpdateOrbitalVectorData(GameEntity entity)
        {
            if (!entity.hasStartVelocity || !_orbitalVector)
            {
                return;
            }

            var scale = _orbitalVector.localScale;
            scale.x = entity.startVelocity.Velocity.magnitude;
            _orbitalVector.localScale = scale;
            
            var angle = Vector2.SignedAngle(entity.startVelocity.Velocity, Vector3.right) * -1;

            var rot = _orbitalVector.rotation;
            var rotAngle = rot.eulerAngles;
            rotAngle.z = angle;
            rot.eulerAngles = rotAngle;
            _orbitalVector.rotation = rot;
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
        
        private void UpdateResultVectorData(GameEntity entity)
        {
            if (!entity.hasVelocity || !_resultVector)
            {
                return;
            }

            var scale = _resultVector.localScale;
            scale.x = entity.velocity.Velocity.magnitude;
            _resultVector.localScale = scale;
            
            var angle = Vector2.SignedAngle(entity.velocity.Velocity, Vector3.right) * -1;

            var rot = _resultVector.rotation;
            var rotAngle = rot.eulerAngles;
            rotAngle.z = angle;
            rot.eulerAngles = rotAngle;
            _resultVector.rotation = rot;
        }

        private void SetPlanetImageAndSize(Sprite sprite, float size)
        {
            if (!_planetImage)
            {
                return;
            }
            
            _planetImage.transform.localScale = Vector3.one * size;
            
            if (sprite == null)
            {
                return;
            }
            
            _planetImage.sprite = sprite;
        }
    }
}