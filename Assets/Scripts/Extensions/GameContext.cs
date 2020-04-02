public partial class GameContext
{
    public void CleanUp()
    {
        foreach (var gameEntity in GetEntities())
        {
            gameEntity.isReadyToDestroy = true;
        }
    }
}