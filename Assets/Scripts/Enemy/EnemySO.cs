
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/EnemyScriptableObject")]
    public class EnemySO:ScriptableObject
    {
        public EnemyType Type;
        public int Health; // max health at the start
        public int HealthRegenerationValue; // value of health regeneration
        public float HealthRegenerationAfter; // will start regeneration of health if not hit for the provided seconds
        public int Damage;
        public int Reward;
        public float Speed;
        public Sprite FullHealthSprite;
        public Sprite LowHealthSprite;
        public List<EnemyType> LayeredEnemy;
        public float LayerEnemySpawnRate;
    }
}
