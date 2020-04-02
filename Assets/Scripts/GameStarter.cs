using Entitas;
using Features.BotsFeature;
using Features.CannonsFeature;
using Features.GameInfoFeature;
using Features.InputFeature;
using Features.SaveFeature;
using Features.SolarSystemFeature;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private Systems _systems;
    private Contexts _contexts;

    private void Awake()
    {
        _contexts = Contexts.sharedInstance;
    }

    void Start()
    {
        _systems = CreateSystems(_contexts);

        _systems.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSystems();
    }
    
    void UpdateSystems()
    {
        _systems.Execute();
        _systems.Cleanup();
    }
    
    Systems CreateSystems(Contexts contexts)
    {
        // Prepare all systems
        var root = new Feature("Systems")
            .Add(new GameInfoFeature(contexts))
            .Add(new GameplayFeature(contexts))
            .Add(new CannonsFeature(contexts))
            .Add(new BotsFeature(contexts))
            .Add(new SaveLoadFeature(contexts))
            .Add(new InputFeature(contexts));

        return root;
    }
}
