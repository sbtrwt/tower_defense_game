using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Player;
using TowerDefense.Sound;
using TowerDefense.Utilities;
using TowerDefense.Wave;
using UnityEngine;

namespace TowerDefense.Enemy
{
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private PlayerService playerService;
        private WaveService waveService;
        private SoundService soundService;

        private EnemyView enemyPrefab;
        private List<EnemySO> enemyScriptableObjects;
        private Transform enemyContainer;

        public EnemyPool(WaveService waveService, PlayerService playerService, SoundService soundService, WaveSO waveScriptableObject)
        {
            this.playerService = playerService;
            this.waveService = waveService;
            this.soundService = soundService;

            enemyPrefab = waveScriptableObject.EnemyPrefab;
            enemyScriptableObjects = waveScriptableObject.EnemiesSO;
            enemyContainer = new GameObject("Enemy Container").transform;
        }

        public EnemyController GetEnemy(EnemyType enemyType)
        {
            EnemyController enemy = GetItem();
            EnemySO scriptableObjectToUse = enemyScriptableObjects.Find(so => so.Type == enemyType);
            enemy.Init(scriptableObjectToUse);
            return enemy;
        }

        protected override EnemyController CreateItem() => new EnemyController(playerService, waveService, soundService, enemyPrefab, enemyContainer);
    }
}
