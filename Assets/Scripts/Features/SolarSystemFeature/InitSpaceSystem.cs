using Entitas;
using ScriptableObjects;
using UnityEngine;

namespace Features.SolarSystemFeature
{
    public class InitSpaceSystem : IInitializeSystem
    {
        const string Sun = "Sun";      
        
        public void Initialize()
        {
            Contexts.sharedInstance.game.ReplaceGravitationalConstant(50f); // real: double 6.6720e-08;
            
            Contexts.sharedInstance.game.ReplaceBotsClock(0, 0.2f);
            
            var data = Resources.Load<PlanetsScriptableObject>("PlanetsData");

            for (var i = 0; i < data.planets.Length; i++)
            {
                var planet = CreatePlanet(data.planets[i], i);
                
                if (i == data.PlayerIndex)
                {
                    planet.ReplacePlayer(i, data.PlayerName);
                }
                else
                {
                    planet.ReplaceBotAi(1, 0);
                }
            }
            
            var sun = Contexts.sharedInstance.game.CreateEntity();

            var sunSprite = Resources.Load<Sprite>("sun");
            
            sun.ReplacePlanetInfo(Sun, data.planets.Length, "sun", 10);
            sun.ReplaceMass(10000);
            sun.ReplacePosition(Vector2.zero);
        }

        private GameEntity CreatePlanet(PlanetInfoData info, int id)
        {
            var planet = Contexts.sharedInstance.game.CreateEntity();
            
            planet.ReplacePlanetInfo(info.Name, id, info.Sprite, info.Size);
            planet.ReplacePlanetTrajectory(info.OrbitWidth, info.OrbitHeight, info.OrbitCenterX, info.OrbitCenterY, info.Speed);
            planet.ReplaceProjectileCannonType(info.Type);
            planet.ReplaceMass(info.Mass);
            planet.ReplaceHealth(info.Health, info.Health);

            return planet;
        }
    }
}
