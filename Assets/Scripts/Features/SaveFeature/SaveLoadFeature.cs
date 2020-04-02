namespace Features.SaveFeature
{
    public class SaveLoadFeature : Feature
    {
        public SaveLoadFeature(Contexts contexts)
        {
            Add(new SaveSystem(contexts));
            Add(new LoadSystem(contexts));
        }
    }
}
