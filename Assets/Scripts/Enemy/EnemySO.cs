
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/EnemyScriptableObject")]
    public class EnemySO:ScriptableObject
    {
        public EnemyType Type;
        public int Health;
        public int Damage;
        public int Reward;
        public float Speed;
        public Sprite Sprite;
        public List<EnemyType> LayeredEnemies;
        public float LayerEnemySpawnRate;
    }
}
