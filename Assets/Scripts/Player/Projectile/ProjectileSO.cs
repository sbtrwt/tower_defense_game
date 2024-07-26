using UnityEngine;

namespace TowerDefense.Player
{
    [CreateAssetMenu(fileName = "ProjectileScriptableObject ", menuName = "ScriptableObjects/ProjectileScriptableObject")]
    public class ProjectileSO : ScriptableObject
    {
        public ProjectileType Type;
        public Sprite Sprite;
        public float Speed;
        public int Damage;
    }
}
