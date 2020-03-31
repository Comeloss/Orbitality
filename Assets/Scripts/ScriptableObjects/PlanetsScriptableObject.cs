using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlanetsData", menuName = "CreateData/PlanetsList", order = 1)]
    public class PlanetsScriptableObject : ScriptableObject 
    {
        public string PlayerName;
        public int PlayerIndex;
        
        public PlanetInfo[] planets;
    }
    
    [Serializable]
    public struct PlanetInfo
    {
        public string Name;
        public Sprite Sprite;
        public int Mass;
        public float Size;
        public ProjectileCannonType Type;
        public int Health;

        public int OrbitWidth;
        public int OrbitHeight;
        public int OrbitCenterX;
        public int OrbitCenterY;
        public float Speed;
    }
}