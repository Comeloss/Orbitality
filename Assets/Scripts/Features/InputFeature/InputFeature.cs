namespace Features.InputFeature
{
    public class InputFeature : Feature
    {
        public InputFeature(Contexts contexts)
        {
            Add(new CleanupInputContext(contexts.input));
        }
    }
}