using Entitas;

namespace Features.GameInfoFeature
{
    public class StartGameSystem : IInitializeSystem
    {
        public void Initialize()
        {
            Contexts.sharedInstance.input.CreateEntity().isRestart = true;
        }
    }
}