//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ProjectileCannonTypeComponent projectileCannonType { get { return (ProjectileCannonTypeComponent)GetComponent(GameComponentsLookup.ProjectileCannonType); } }
    public bool hasProjectileCannonType { get { return HasComponent(GameComponentsLookup.ProjectileCannonType); } }

    public void AddProjectileCannonType(ProjectileCannonType newType) {
        var index = GameComponentsLookup.ProjectileCannonType;
        var component = CreateComponent<ProjectileCannonTypeComponent>(index);
        component.Type = newType;
        AddComponent(index, component);
    }

    public void ReplaceProjectileCannonType(ProjectileCannonType newType) {
        var index = GameComponentsLookup.ProjectileCannonType;
        var component = CreateComponent<ProjectileCannonTypeComponent>(index);
        component.Type = newType;
        ReplaceComponent(index, component);
    }

    public void RemoveProjectileCannonType() {
        RemoveComponent(GameComponentsLookup.ProjectileCannonType);
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

    static Entitas.IMatcher<GameEntity> _matcherProjectileCannonType;

    public static Entitas.IMatcher<GameEntity> ProjectileCannonType {
        get {
            if (_matcherProjectileCannonType == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ProjectileCannonType);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherProjectileCannonType = matcher;
            }

            return _matcherProjectileCannonType;
        }
    }
}
