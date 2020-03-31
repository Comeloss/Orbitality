using Entitas;

namespace Features.Game
{
    public class InitGamePlayStateSystem : IInitializeSystem
    {
        public void Initialize()
        {
            Contexts.sharedInstance.game.ReplaceGamePlayState(GamePlayStateComponent.GamePlayStateType.Play);            
        }
    }
}
