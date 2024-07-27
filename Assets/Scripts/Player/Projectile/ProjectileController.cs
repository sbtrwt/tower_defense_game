using System.Collections;
using System.Collections.Generic;
using TowerDefense.Enemy;
using UnityEngine;
namespace TowerDefense.Player
{
    public class ProjectileController
    {
        private PlayerService playerService;
        private ProjectileView projectileView;
        private ProjectileSO projectileScriptableObject;

        private EnemyController target;
        private ProjectileState currentState;
       
        public ProjectileController(PlayerService playerService, ProjectileView projectilePrefab, Transform projectileContainer)
        {
            this.playerService = playerService;
            projectileView = Object.Instantiate(projectilePrefab, projectileContainer);
            projectileView.SetController(this);
        }

        public void Init(ProjectileSO projectileScriptableObject)
        {
            this.projectileScriptableObject = projectileScriptableObject;
            projectileView.SetSprite(projectileScriptableObject.Sprite);
            projectileView.gameObject.SetActive(true);
            target = null;
        }

        public void SetPosition(Vector3 spawnPosition) => projectileView.transform.position = spawnPosition;

        public void SetTarget(EnemyController target)
        {
            this.target = target;
            SetState(ProjectileState.ACTIVE);
            RotateTowardsTarget();
        }

        private void RotateTowardsTarget()
        {
            Vector3 direction = target.Position - projectileView.transform.position;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 180;
            projectileView.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void UpdateProjectileMotion()
        {
            if (target != null && currentState == ProjectileState.ACTIVE)
                projectileView.transform.Translate(Vector2.left * projectileScriptableObject.Speed * Time.deltaTime, Space.Self);
        }

        public void OnHitEnemy(EnemyController enemyHit)
        {
            if (currentState == ProjectileState.ACTIVE)
            {
                enemyHit.TakeDamage(projectileScriptableObject.Damage);
                ResetProjectile();
                SetState(ProjectileState.HIT_TARGET);
            }
        }
        public void OnSplashHitEnemy(EnemyController enemyHit)
        {
            if (currentState == ProjectileState.ACTIVE)
            {
                enemyHit.TakeDamage(projectileScriptableObject.Damage);
            }
        }
        public void ResetProjectile()
        {
            target = null;
            projectileView.gameObject.SetActive(false);
            playerService.ReturnProjectileToPool(this);
        }

        private void SetState(ProjectileState newState) => currentState = newState;
        public bool IsSplashAttack() => projectileScriptableObject.IsSplashAttack;
        public float SplashAttackEffectRadius() => projectileScriptableObject.SplashEffectRadius;

    }

    public enum ProjectileState
    {
        ACTIVE,
        HIT_TARGET
    }
}

