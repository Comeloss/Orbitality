using System.Collections.Generic;
using Entitas.VisualDebugging.Unity;
using UniRx;
using UnityEngine;

namespace Views.SolarSystem
{
    public class SolarSystemView : ReactiveViewBase
    {
        
#pragma warning disable 0649
        [SerializeField] private PlanetView _planetPrefab;    
#pragma warning restore 0649
        

        private readonly Dictionary<int, PlanetView> _planets = new Dictionary<int, PlanetView>();

        protected override void OnEnable()
        {
            ObserveEntityWithComponents(
                GameMatcher.PlanetInfo,
                entity => entity.planetInfo,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(AddPlanet);
            
            ObserveEntityWithComponents(
                GameMatcher.ReadyToDestroy,
                entity => entity,
                entity => entity.hasPlanetInfo, 
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(RemovePlanet);
        }

        private void AddPlanet(PlanetInfoComponent component)
        {
            if (component == null)
            {
                return;
            }

            if (_planets.ContainsKey(component.Id))
            {
                return;
            }

            var newPlanet = Instantiate(_planetPrefab, transform);
            newPlanet.name = component.Name;
            
            _planets[component.Id] = newPlanet;
            _planets[component.Id].Init(component.Id);
        }
        
        private void RemovePlanet(GameEntity entity)
        {
            if (!_planets.ContainsKey(entity.planetInfo.Id))
            {
                return;
            }

            _planets[entity.planetInfo.Id].gameObject.DestroyGameObject();
        }
    }
}