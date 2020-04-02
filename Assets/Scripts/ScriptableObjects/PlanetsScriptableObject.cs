using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlanetsData", menuName = "CreateData/PlanetsList", order = 1)]
    public class PlanetsScriptableObject : ScriptableObject 
    {
        public string PlayerName;
        public int PlayerIndex;
        
        public PlanetInfoData[] planets;
    }
    
    [Serializable]
    public struct PlanetInfoData
    {
        public string Name;
        public string Sprite;
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