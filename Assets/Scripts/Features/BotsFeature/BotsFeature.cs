namespace Features.BotsFeature
{
    public class BotsFeature : Feature
    {
        public BotsFeature(Contexts contexts)
        {
            Add(new UpdateBotsClockSystem(contexts));
            Add(new UpdateBotsBehaviourSystem(contexts));
            
            Add(new HeavyCannonBotsBehaviourSystem(contexts));
            Add(new FastCannonBotBehaviourSystem(contexts));
            Add(new FrequentCannonBotBehaviourSystem(contexts));
        }
    }
}
