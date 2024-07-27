using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerDefense.Enemy;
using TowerDefense.Events;
using TowerDefense.Map;
using TowerDefense.Player;
using TowerDefense.Sound;
using TowerDefense.UI;
using UnityEngine;

namespace TowerDefense.Wave
{
    public class WaveService
    {
        private UIService uiService;
        private MapService mapService;
        private PlayerService playerService;
        private SoundService soundService;
        private EventService eventService;

        private WaveSO waveScriptableObject;
        private EnemyPool enemyPool;

        private int currentWaveId;
        private List<WaveData> waveDatas;
        private List<EnemyController> activeEnemies;

        public WaveService(WaveSO waveScriptableObject) => this.waveScriptableObject = waveScriptableObject;

        public void Init(UIService uiService, MapService mapService, PlayerService playerService, SoundService soundService, EventService eventService)
        {
            this.uiService = uiService;
            this.mapService = mapService;
            this.playerService = playerService;
            this.soundService = soundService;
            this.eventService = eventService;
            InitializeEnemys();
            SubscribeToEvents();
        }

        private void InitializeEnemys()
        {
            enemyPool = new EnemyPool(this, playerService, soundService, waveScriptableObject);
            activeEnemies = new List<EnemyController>();
        }

        private void SubscribeToEvents() => eventService.OnMapSelected.AddListener(LoadWaveDataForMap);

        private void LoadWaveDataForMap(int mapId)
        {
            currentWaveId = 0;
            waveDatas = waveScriptableObject.WaveConfigurations.Find(config => config.MapID == mapId).WaveDatas;
            uiService.UpdateWaveProgressUI(currentWaveId, waveDatas.Count);
        }

        public void StarNextWave()
        {
            currentWaveId++;
            var enemysToSpawn = GetEnemysForCurrentWave();
            var spawnPosition = mapService.GetEnemySpawnPositionForCurrentMap();
            SpawnEnemies(enemysToSpawn, spawnPosition, 0, waveScriptableObject.SpawnRate);
        }

        public async void SpawnEnemies(List<EnemyType> enemysToSpawn, Vector3 spawnPosition, int startingWaypointIndex, float spawnRate)
        {
            foreach (EnemyType enemyType in enemysToSpawn)
            {
                EnemyController enemy = enemyPool.GetEnemy(enemyType);
                enemy.SetPosition(spawnPosition);
                enemy.SetWayPoints(mapService.GetWayPointsForCurrentMap(), startingWaypointIndex);

                AddEnemy(enemy);
                await Task.Delay(Mathf.RoundToInt(spawnRate * 1000));
            }
        }

        private void AddEnemy(EnemyController enemyToAdd)
        {
            activeEnemies.Add(enemyToAdd);
            enemyToAdd.SetOrderInLayer(-activeEnemies.Count);
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            enemyPool.ReturnItem(enemy);
            
            activeEnemies.Remove(enemy);
            if (HasCurrentWaveEnded())
            {
                soundService.PlaySoundEffects(Sound.SoundType.WaveComplete);
                uiService.UpdateWaveProgressUI(currentWaveId, waveDatas.Count);

                if (IsLevelWon())
                    uiService.UpdateGameEndUI(true);
                else
                    uiService.SetNextWaveButton(true);
            }
        }

        private List<EnemyType> GetEnemysForCurrentWave() => waveDatas.Find(waveData => waveData.WaveID == currentWaveId).ListOfEnemies;

        private bool HasCurrentWaveEnded() => activeEnemies.Count == 0;

        private bool IsLevelWon() => currentWaveId >= waveDatas.Count;
    }
}
