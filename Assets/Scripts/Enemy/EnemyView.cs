using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        public EnemyController Controller { get; set; }
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void Update() => Controller?.FollowWayPoints();

        public void SetRenderer(Sprite spriteToSet)
        {
            if (spriteToSet != null)
                spriteRenderer.sprite = spriteToSet;
        }


        public void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;

        public void PopEnemy()
        {
            animator.enabled = true;
            animator.Play("pop", 0);
        }

        /// <summary>
        /// Called on pop event trigger of pop animation
        /// </summary>
        public void PopAnimationPlayed()
        {
            spriteRenderer.sprite = null;
            gameObject.SetActive(false);
            Controller.OnPopAnimationPlayed();
        }
    }
}
