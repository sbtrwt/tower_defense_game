using System.Collections;
using System.Collections.Generic;
using TowerDefense.Enemy;
using TowerDefense.Sound;
using UnityEngine;
namespace TowerDefense.Player
{
    public class TowerController
    {
        private SoundService soundService;
        private TowerSO towerScriptableObject;
        private ProjectilePool projectilePool;
        private TowerView towerView;

        private List<EnemyController> enemiesInRange;
        private float attackTimer;

        public TowerController(SoundService soundService, TowerSO towerScriptableObject, ProjectilePool projectilePool)
        {
            this.soundService = soundService;
            this.towerScriptableObject = towerScriptableObject;
            this.projectilePool = projectilePool;

            CreateTowerView();
            InitializeVariables();
        }

        private void CreateTowerView()
        {
            towerView = Object.Instantiate(towerScriptableObject.Prefab);
            towerView.SetController(this);
            towerView.SetTriggerRadius(towerScriptableObject.Range);
        }

        private void InitializeVariables()
        {
            enemiesInRange = new List<EnemyController>();
            ResetAttackTimer();
        }

        public void SetPosition(Vector3 positionToSet) => towerView.transform.position = positionToSet;

        public void EnemyEnteredRange(EnemyController enemy)
        {
            if (CanAttackEnemy(enemy.GetEnemyType()))
                enemiesInRange.Add(enemy);
        }

        public void EnemyExitedRange(EnemyController enemy)
        {
            if (CanAttackEnemy(enemy.GetEnemyType()))
                enemiesInRange.Remove(enemy);
        }

        public bool CanAttackEnemy(EnemyType bloonType) => towerScriptableObject.AttackableEnemies.Contains(bloonType);

        public void UpdateTower()
        {
            if (enemiesInRange.Count > 0)
            {
                RotateTowardsTarget(enemiesInRange[0]);
                ShootAtTarget(enemiesInRange[0]);
            }
        }

        private void RotateTowardsTarget(EnemyController targetEnemy)
        {
            Vector3 direction = targetEnemy.Position - towerView.transform.position;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 180;
            towerView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void ShootAtTarget(EnemyController targetEnemy)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                CreateProjectileForTarget(targetEnemy);
                soundService.PlaySoundEffects(SoundType.TowerShoot);
                ResetAttackTimer();
            }
        }

        private void CreateProjectileForTarget(EnemyController targetEnemy)
        {
            ProjectileController projectile = projectilePool.GetProjectile(towerScriptableObject.projectileType);
            projectile.SetPosition(towerView.transform.position);
            projectile.SetTarget(targetEnemy);
        }

        private void ResetAttackTimer() => attackTimer = towerScriptableObject.AttackRate;
    }
}