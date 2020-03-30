//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ProjectilesTotalShootComponent projectilesTotalShoot { get { return (ProjectilesTotalShootComponent)GetComponent(GameComponentsLookup.ProjectilesTotalShoot); } }
    public bool hasProjectilesTotalShoot { get { return HasComponent(GameComponentsLookup.ProjectilesTotalShoot); } }

    public void AddProjectilesTotalShoot(int newCount) {
        var index = GameComponentsLookup.ProjectilesTotalShoot;
        var component = CreateComponent<ProjectilesTotalShootComponent>(index);
        component.Count = newCount;
        AddComponent(index, component);
    }

    public void ReplaceProjectilesTotalShoot(int newCount) {
        var index = GameComponentsLookup.ProjectilesTotalShoot;
        var component = CreateComponent<ProjectilesTotalShootComponent>(index);
        component.Count = newCount;
        ReplaceComponent(index, component);
    }

    public void RemoveProjectilesTotalShoot() {
        RemoveComponent(GameComponentsLookup.ProjectilesTotalShoot);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherProjectilesTotalShoot;

    public static Entitas.IMatcher<GameEntity> ProjectilesTotalShoot {
        get {
            if (_matcherProjectilesTotalShoot == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ProjectilesTotalShoot);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherProjectilesTotalShoot = matcher;
            }

            return _matcherProjectilesTotalShoot;
        }
    }
}
