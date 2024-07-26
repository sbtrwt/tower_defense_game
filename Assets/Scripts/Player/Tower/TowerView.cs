using System.Collections;
using System.Collections.Generic;
using TowerDefense.Enemy;
using UnityEngine;
namespace TowerDefense.Player
{
    public class TowerView : MonoBehaviour
    {
        private TowerController controller;
        private CircleCollider2D rangeTriggerCollider;
        private Animator towerAnimator;
        [SerializeField] public SpriteRenderer RangeSpriteRenderer;

        private void Awake()
        {
            rangeTriggerCollider = GetComponent<CircleCollider2D>();
            towerAnimator = GetComponent<Animator>();
        }

        public void SetController(TowerController controller) => this.controller = controller;

        public void SetTriggerRadius(float radiusToSet)
        {
            if (rangeTriggerCollider != null)
                rangeTriggerCollider.radius = radiusToSet;

            RangeSpriteRenderer.transform.localScale = new Vector3(radiusToSet, radiusToSet, 1);
            MakeRangeVisible(false);
        }

        public void PlayAnimation(TowerAnimation animationToPlay) => towerAnimator.Play(animationToPlay.ToString(), 0);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<EnemyView>() != null)
                controller.EnemyEnteredRange(collision.GetComponent<EnemyView>().Controller);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<EnemyView>() != null)
                controller.EnemyExitedRange(collision.GetComponent<EnemyView>().Controller);
        }

        public void MakeRangeVisible(bool makeVisible) => RangeSpriteRenderer.color = makeVisible ? new Color(1, 1, 1, 0.25f) : new Color(1, 1, 1, 0);
    }
    public enum TowerAnimation
    {
        Idle,
        Shoot
    }
}