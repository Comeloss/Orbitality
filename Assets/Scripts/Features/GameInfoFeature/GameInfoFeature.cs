namespace Features.GameInfoFeature
{
    public class GameInfoFeature : Feature
    {
        public GameInfoFeature(Contexts contexts)
        {
            Add(new StartGameSystem());
            Add(new OnInitGameSystem(contexts));
            
            Add(new ResetTimeSystem(contexts));
            Add(new SwitchGamePlayStateSystem(contexts));
            Add(new TimeSystem(contexts));
        }
    }
}
