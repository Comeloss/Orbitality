namespace Features.CannonsFeature
{
    public class CannonsFeature : Feature
    {
        public CannonsFeature(Contexts contexts)
        {
            Add(new InitProjectileCannonsSystem(contexts));
            Add(new ProjectilesGravitySystem(contexts));
            Add(new UpdateProjectileStartVelocitySystem(contexts));
            Add(new ShootProjectileSystem(contexts));
            Add(new UpdateProjectilesLifeTimeSystem(contexts));
            Add(new HandleCollisionSystem(contexts));
        }
    }
}
