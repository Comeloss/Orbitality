namespace Features.GameInfoFeature
{
    public class GameInfoFeature : Feature
    {
        public GameInfoFeature(Contexts contexts)
        {
            Add(new InitGamePlayStateSystem());
            Add(new SwitchGamePlayStateSystem(contexts));
            Add(new TimeSystem(contexts));
        }
    }
}
