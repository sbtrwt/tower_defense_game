using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerDefense.Enemy;
using TowerDefense.Events;
using TowerDefense.Map;
using UnityEngine;

namespace TowerDefense.Wave
{
    public class WaveService
    {
        private WaveSO waveSO;

        private int currentWaveId;
        private List<WaveData> waveDatas;
        private List<EnemyController> activeEnemies;

        private MapService mapService;
        private EventService eventService;
        public WaveService(WaveSO waveScriptableObject) 
        {
            this.waveSO = waveScriptableObject; 
        }

        public void Init(EventService eventService, MapService mapService)
        {
           
            this.mapService = mapService;
            this.eventService = eventService;
            InitializeEnemies();
            SubscribeToEvents();
        }
        private void InitializeEnemies()
        {
            //bloonPool = new BloonPool(this, playerService, soundService, waveScriptableObject);
            activeEnemies = new List<EnemyController>();
            LoadWaveDataForMap(1);
            StarNextWave();
        }

        private void SubscribeToEvents() => eventService.OnMapSelected.AddListener(LoadWaveDataForMap);

        private void LoadWaveDataForMap(int mapId)
        {
            currentWaveId = 0;
            waveDatas = waveSO.WaveConfigurations.Find(config => config.MapID == mapId).WaveDatas;
        }
        public void StarNextWave()
        {
            currentWaveId++;
            var bloonsToSpawn = GetEnemiesForCurrentWave();
            var spawnPosition = mapService.GetEnemySpawnPositionForCurrentMap();
            SpawnBloons(bloonsToSpawn, spawnPosition, 0, waveSO.SpawnRate);
        }
        private List<EnemyType> GetEnemiesForCurrentWave() => waveDatas.Find(waveData => waveData.WaveID == currentWaveId).ListOfEnemies;
        public async void SpawnBloons(List<EnemyType> enemiesToSpawn, Vector3 spawnPosition, int startingWaypointIndex, float spawnRate)
        {
            GameObject enemyParent = new GameObject("enemy");
            foreach (EnemyType enemyType in enemiesToSpawn)
            {
                EnemyController enemy = new EnemyController(waveSO.EnemyPrefab, enemyParent.transform);
                enemy.Init(waveSO.EnemiesSO.Find(so => so.Type == enemyType));
                enemy.SetPosition(spawnPosition);
                enemy.SetWayPoints(mapService.GetWayPointsForCurrentMap(), startingWaypointIndex);


                AddBloon(enemy);
                await Task.Delay(Mathf.RoundToInt(spawnRate * 1000));
            }
        }
        private void AddBloon(EnemyController enemyToAdd)
        {
            activeEnemies.Add(enemyToAdd);
            enemyToAdd.SetOrderInLayer(-activeEnemies.Count);
        }
    }
}
