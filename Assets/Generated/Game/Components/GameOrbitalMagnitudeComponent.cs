//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public OrbitalMagnitudeComponent orbitalMagnitude { get { return (OrbitalMagnitudeComponent)GetComponent(GameComponentsLookup.OrbitalMagnitude); } }
    public bool hasOrbitalMagnitude { get { return HasComponent(GameComponentsLookup.OrbitalMagnitude); } }

    public void AddOrbitalMagnitude(float newMagnitude) {
        var index = GameComponentsLookup.OrbitalMagnitude;
        var component = CreateComponent<OrbitalMagnitudeComponent>(index);
        component.Magnitude = newMagnitude;
        AddComponent(index, component);
    }

    public void ReplaceOrbitalMagnitude(float newMagnitude) {
        var index = GameComponentsLookup.OrbitalMagnitude;
        var component = CreateComponent<OrbitalMagnitudeComponent>(index);
        component.Magnitude = newMagnitude;
        ReplaceComponent(index, component);
    }

    public void RemoveOrbitalMagnitude() {
        RemoveComponent(GameComponentsLookup.OrbitalMagnitude);
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

    static Entitas.IMatcher<GameEntity> _matcherOrbitalMagnitude;

    public static Entitas.IMatcher<GameEntity> OrbitalMagnitude {
        get {
            if (_matcherOrbitalMagnitude == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.OrbitalMagnitude);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherOrbitalMagnitude = matcher;
            }

            return _matcherOrbitalMagnitude;
        }
    }
}
