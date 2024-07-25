using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefense.Enemy
{
    public class EnemyController
    {
        private EnemyView enemyView;
        private EnemySO enemySO;

        private const float waypointThreshold = 0.1f;
        private List<Vector3> waypoints;
        private int currentHealth;
        private int currentWaypointIndex;
        private EnemyState currentState;
        public EnemyController(EnemyView enemyPrefab, Transform enemyContainer)
        {
            enemyView = UnityEngine.Object.Instantiate(enemyPrefab, enemyContainer);
            enemyView.Controller = this;
        }
        public void Init(EnemySO enemySO)
        {
            this.enemySO = enemySO;
            InitializeVariables();
            SetState(EnemyState.ACTIVE);
        }
        private void InitializeVariables()
        {
            enemyView.SetRenderer(enemySO.FullHealthSprite);
            currentHealth = enemySO.Health;
            waypoints = new List<Vector3>();
        }
        private void SetState(EnemyState state) => currentState = state;
        private Vector3 GetDirectionToMoveTowards() => waypoints[currentWaypointIndex] - enemyView.transform.position;
        private bool HasReachedFinalWaypoint() => currentWaypointIndex == waypoints.Count;
        private bool HasReachedNextWaypoint(float distance) => distance < waypointThreshold;
        private void Move(Vector3 moveDirection) => enemyView.transform.Translate(moveDirection.normalized * enemySO.Speed * Time.deltaTime);
        public void FollowWayPoints()
        {
            if (HasReachedFinalWaypoint())
            {
                
            }
            else
            {
                Vector3 direction = GetDirectionToMoveTowards();
                Move(direction);
                if (HasReachedNextWaypoint(direction.magnitude))
                    currentWaypointIndex++;
            }
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

        public enum EnemyState
        {
            ACTIVE,
            POPPED
        }
    }
}
