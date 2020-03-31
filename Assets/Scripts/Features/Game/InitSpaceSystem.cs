using Entitas;
using UnityEngine;

namespace Features.Game
{
    public class InitSpaceSystem : IInitializeSystem
    {
        public const string Sun = "Sun";      
        
        public void Initialize()
        {
            Contexts.sharedInstance.game.ReplaceGravitationalConstant(10f); // real: double 6.6720e-08;
            
            var sun = Contexts.sharedInstance.game.CreateEntity();

            var sunSprite = Resources.Load<Sprite>("sun");
            
            sun.ReplacePlanetInfo(Sun, 0, sunSprite, 10);
            sun.ReplaceMass(10000);
            sun.ReplacePosition(Vector2.zero);
            
            var planet1 = Contexts.sharedInstance.game.CreateEntity();
            
            var planet1Sprite = Resources.Load<Sprite>("planet1");
            
            planet1.ReplacePlanetInfo("Earth", 1, planet1Sprite, 5);
            planet1.ReplacePlanetTrajectory(80, 100, 0, 0, 0.3f);
            planet1.ReplaceProjectileCannonType(ProjectileCannonType.Heavy);
            planet1.ReplaceMass(3000);
            planet1.ReplaceHealth(100, 100);

            Contexts.sharedInstance.game.ReplacePlayer(1);
            
            var planet2 = Contexts.sharedInstance.game.CreateEntity();
            
            var planet2Sprite = Resources.Load<Sprite>("planet2");
            
            planet2.ReplacePlanetInfo("Venus", 2, planet2Sprite, 7);
            planet2.ReplacePlanetTrajectory(150, 150, 0, 0, 0.25f);
            planet2.ReplaceProjectileCannonType(ProjectileCannonType.Heavy);
            planet2.ReplaceMass(5000);
            planet2.ReplaceHealth(100, 100);
            
            var planet3 = Contexts.sharedInstance.game.CreateEntity();
            
            var planet3Sprite = Resources.Load<Sprite>("planet3");
            
            planet3.ReplacePlanetInfo("Jupiter", 3, planet3Sprite, 8);
            planet3.ReplacePlanetTrajectory(350, 250, 50, 0, 0.15f);
            planet3.ReplaceProjectileCannonType(ProjectileCannonType.Frequent);
            planet3.ReplaceHealth(100, 100);
            planet3.ReplaceMass(6000);
            
            var planet4 = Contexts.sharedInstance.game.CreateEntity();
            
            var planet4Sprite = Resources.Load<Sprite>("planet4");
            
            planet4.ReplacePlanetInfo("Pluto", 4, planet4Sprite, 6);
            planet4.ReplacePlanetTrajectory(280, 200, -50, -50, 0.1f);
            planet4.ReplaceProjectileCannonType(ProjectileCannonType.Fast);
            planet4.ReplaceHealth(100, 100);
            planet4.ReplaceMass(4000);
        }
    }
}
