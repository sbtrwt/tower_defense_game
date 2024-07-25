using System.Collections.Generic;
using TowerDefense.Enemy;
using UnityEngine;

namespace TowerDefense.Wave
{
    [CreateAssetMenu(fileName = "WaveScriptableObject", menuName = "ScriptableObjects/WaveScriptableObject")]
    public class WaveSO : ScriptableObject
    {
        public float SpawnRate;
        public List<WaveConfigurationSO> WaveConfigurations;
        public EnemyView EnemyPrefab;
        public List<EnemySO> EnemiesSO;
    }
}
