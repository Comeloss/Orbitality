using Features.Game.GameTime;
using Features.Game.Gravity;
using Features.Game.Projectile;

namespace Features.Game
{
    public class GameplayFeature : Feature
    {
        public GameplayFeature(Contexts contexts)
        {
            Add(new InitSpaceSystem());
            Add(new InitProjectileCannonsSystem(contexts));
            
            Add(new TimeSystem(contexts));
            
            Add(new ProjectilesGravitySystem(contexts));
            
            Add(new UpdateGravityVelocitySystem(contexts));
            Add(new UpdatePlanetsVelocitySystem(contexts));
            
            Add(new UpdateVelocitySystem(contexts));
            
            Add(new UpdateProjectileStartVelocitySystem(contexts));
            
            Add(new UpdateObjectsPositionSystem(contexts));
            Add(new HandleObjectHealthSystem(contexts));
            
            Add(new ShootProjectileSystem(contexts));
            Add(new LifeTimeSystem(contexts));
            Add(new UpdateProjectilesLifeTimeSystem(contexts));
            
            
            
            Add(new HandleCollisionSystem(contexts));
            
            Add(new DestroyEntitiesGameSystem(contexts.game));
        }
    }
}
