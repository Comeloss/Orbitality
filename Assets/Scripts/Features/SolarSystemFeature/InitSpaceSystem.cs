using System;
using System.Collections.Generic;
using Entitas;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.SolarSystemFeature
{
    public class InitSpaceSystem : ReactiveSystem<InputEntity>
    {
        const string Sun = "Sun";

        private readonly Contexts _contexts;

        public InitSpaceSystem(Contexts context) : base(context.input)
        {
            _contexts = context;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.InitGameplay);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isInitGameplay;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            InitGame();
        }

        private void InitGame()
        {
            _contexts.game.ReplaceGravitationalConstant(50f); // real: double 6.6720e-08;

            _contexts.game.ReplaceBotsClock(0, 0.2f);

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

            var sun = _contexts.game.CreateEntity();

            sun.ReplacePlanetInfo(Sun, data.planets.Length, "sun", 10);
            sun.ReplaceMass(10000);
            sun.ReplacePosition(Vector2.zero);
        }

        private GameEntity CreatePlanet(PlanetInfoData info, int id)
        {
            var planet = _contexts.game.CreateEntity();

            var size = Random.Range(5, 8);
            var sprite = "planet" + Random.Range(1, 7);
            var orbitWidth = Random.Range(40, 60) * id + 130;
            var orbitHeight = Random.Range(25, 40) * id + 100;

            var orbitCenterY = Random.Range(-30, 30);
            var orbitCenterX = Random.Range(-50, 50);

            var values = Enum.GetValues(typeof(ProjectileCannonType));
            var cannonType = (ProjectileCannonType) values.GetValue(Random.Range(0,values.Length));

            var speed = Random.Range(0.05f, 0.4f);

            var mass = Random.Range(1500, 4000);
            
            var health = Random.Range(80, 150);

            planet.ReplacePlanetInfo(info.Name, id, sprite, size);
            planet.ReplacePlanetTrajectory(orbitWidth, orbitHeight, orbitCenterX, orbitCenterY, speed);
            planet.ReplaceProjectileCannonType(cannonType);
            planet.ReplaceMass(mass);
            planet.ReplaceHealth(health, health);

            return planet;
        }
    }
}