using Entitas;

namespace Features.GameInfoFeature
{
    public class InitGamePlayStateSystem : IInitializeSystem
    {
        public void Initialize()
        {
            Contexts.sharedInstance.game.ReplaceGamePlayState(GamePlayStateComponent.GamePlayStateType.Play);            
        }
    }
}
