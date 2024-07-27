using System.Collections.Generic;
using TowerDefense.Enemy;
using UnityEngine;

namespace TowerDefense.Player
{
    [CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "ScriptableObjects/TowerScriptableObject")]
    public class TowerSO : ScriptableObject
    {
        public TowerType Type;
        public TowerView Prefab;
        public float RotationSpeed;
        public ProjectileType projectileType;
        public float Range;
        public int Cost;
        public List<EnemyType> AttackableEnemies;
        public float AttackRate;
    }
}
