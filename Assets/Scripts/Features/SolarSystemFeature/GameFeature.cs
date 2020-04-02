namespace Features.SolarSystemFeature
{
    public class GameplayFeature : Feature
    {
        public GameplayFeature(Contexts contexts)
        {
            Add(new InitSpaceSystem());
            Add(new UpdateGravityVelocitySystem(contexts));
            Add(new UpdatePlanetsVelocitySystem(contexts));
            Add(new UpdateVelocitySystem(contexts));
            Add(new UpdateObjectsPositionSystem(contexts));
            Add(new HandleObjectHealthSystem(contexts));
            Add(new LifeTimeSystem(contexts));
            Add(new CooldownSystem(contexts));
            
            Add(new DestroyEntitiesGameSystem(contexts.game));
        }
    }
}
