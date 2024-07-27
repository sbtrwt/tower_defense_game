using System.Collections;
using System.Collections.Generic;
using TowerDefense.Enemy;
using UnityEngine;

namespace TowerDefense.Player
{
    public class ProjectileView : MonoBehaviour
    {
        private ProjectileController controller;
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private float splashRadius = 0.1f; // Define the splash radius

        private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

        public void SetController(ProjectileController controller) => this.controller = controller;

        private void Update()
        {
            if (ProjectileOutOfBounds())
                controller.ResetProjectile();

            controller?.UpdateProjectileMotion();
        }

        private bool ProjectileOutOfBounds() => !spriteRenderer.isVisible;

        public void SetSprite(Sprite spriteToSet) => spriteRenderer.sprite = spriteToSet;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<EnemyView>() != null)
            {
                // Get the enemy controller of the hit enemy
                EnemyController hitEnemyController = collision.GetComponent<EnemyView>().Controller;
               // Trigger the splash damage effect
                if (controller.IsSplashAttack())
                { ApplySplashDamage(hitEnemyController); }
                // Notify the controller about the hit
                controller.OnHitEnemy(hitEnemyController);

            }
        }

        private void ApplySplashDamage(EnemyController hitEnemyController)
        {
            splashRadius = controller.SplashAttackEffectRadius();
            // Find all colliders within the splash radius
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRadius);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<EnemyView>() != null)
                {
                    // Get the enemy controller of the affected enemy
                    EnemyController enemyController = hitCollider.GetComponent<EnemyView>().Controller;
                  
                    // Apply damage or effect to the enemy
                    if (!hitEnemyController.Equals(enemyController))
                    {
                        Debug.Log(enemyController); 
                        controller.OnSplashHitEnemy(enemyController); 
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Draw the splash radius in the editor for debugging purposes
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, splashRadius);
        }
    }
}
