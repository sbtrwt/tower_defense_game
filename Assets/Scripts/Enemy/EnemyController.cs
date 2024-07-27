using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Player;
using TowerDefense.Sound;
using TowerDefense.UI;
using TowerDefense.Wave;
using UnityEngine;

namespace TowerDefense.Enemy
{
    public class EnemyController
    {
        private PlayerService playerService;
        private WaveService waveService;
        private SoundService soundService;

        private EnemyView enemyView;
        private EnemySO enemyScriptableObject;

        private const float waypointThreshold = 0.1f;
        private List<Vector3> waypoints;
        private float currentHealth;
        private int currentWaypointIndex;
        private EnemyState currentState;

        public Vector3 Position => enemyView.transform.position;

        public EnemyController(PlayerService playerService, WaveService waveService, SoundService soundService, EnemyView enemyPrefab, Transform enemyContainer)
        {
            this.playerService = playerService;
            this.waveService = waveService;
            this.soundService = soundService;

            enemyView = GameObject.Instantiate(enemyPrefab, enemyContainer);
            enemyView.Controller = this;
        }

        public void Init(EnemySO enemyScriptableObject)
        {
            this.enemyScriptableObject = enemyScriptableObject;
            InitializeVariables();
            SetState(EnemyState.ACTIVE);
        }

        private void InitializeVariables()
        {
            enemyView.SetRenderer(enemyScriptableObject.Sprite);
            currentHealth = enemyScriptableObject.Health;
            waypoints = new List<Vector3>();
        }

        public void SetPosition(Vector3 spawnPosition)
        {
            enemyView.transform.position = spawnPosition;
            enemyView.gameObject.SetActive(true);
        }

        public void SetWayPoints(List<Vector3> waypointsToSet, int startingWaypointIndex)
        {
            waypoints = waypointsToSet;
            currentWaypointIndex = startingWaypointIndex;
        }

        public void SetOrderInLayer(int orderInLayer) => enemyView.SetSortingOrder(orderInLayer);

        public void TakeDamage(float damageToTake)
        {
            float reducedHealth = currentHealth - damageToTake;
            currentHealth = reducedHealth <= 0 ? 0 : reducedHealth;

            if (currentHealth <= 0 && currentState == EnemyState.ACTIVE)
            {
                PopEnemy();
                soundService.PlaySoundEffects(SoundType.EnemyPop);
            }
        }

        public void FollowWayPoints()
        {
            if (HasReachedFinalWaypoint())
            {
                ResetEnemy();
            }
            else
            {
                Vector3 direction = GetDirectionToMoveTowards();
                MoveEnemy(direction);
                if (HasReachedNextWaypoint(direction.magnitude))
                    currentWaypointIndex++;
            }
        }

        private bool HasReachedFinalWaypoint() => currentWaypointIndex == waypoints.Count;

        private bool HasReachedNextWaypoint(float distance) => distance < waypointThreshold;

        private void ResetEnemy()
        {
            waveService.RemoveEnemy(this);
            playerService.TakeDamage(enemyScriptableObject.Damage);
            enemyView.gameObject.SetActive(false);
        }

        private Vector3 GetDirectionToMoveTowards() => waypoints[currentWaypointIndex] - enemyView.transform.position;

        private void MoveEnemy(Vector3 moveDirection) => enemyView.transform.Translate(moveDirection.normalized * enemyScriptableObject.Speed * Time.deltaTime);

        private void PopEnemy()
        {
            SetState(EnemyState.POPPED);
            enemyView.PopEnemy();
        }

        public void OnPopAnimationPlayed()
        {
            if (HasLayeredEnemys())
                SpawnLayeredEnemys();

            playerService.GetReward(enemyScriptableObject.Reward);
            waveService.RemoveEnemy(this);
        }

        private bool HasLayeredEnemys() => enemyScriptableObject.LayeredEnemies.Count > 0;

        private void SpawnLayeredEnemys() => waveService.SpawnEnemies(enemyScriptableObject.LayeredEnemies,
                                                                     enemyView.transform.position,
                                                                     currentWaypointIndex,
                                                                     enemyScriptableObject.LayerEnemySpawnRate);

        public EnemyType GetEnemyType() => enemyScriptableObject.Type;

        private void SetState(EnemyState state) => currentState = state;
       
    } 
    public enum EnemyState
        {
            ACTIVE,
            POPPED
        }
}
