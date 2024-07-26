using System.Collections;
using System.Collections.Generic;
using TowerDefense.Map;
using TowerDefense.Sound;
using TowerDefense.UI;
using UnityEngine;

namespace TowerDefense.Player
{
    public class PlayerService
    {
        private MapService mapService;
        private UIService uiService;
        private SoundService soundService;
        private PlayerSO playerScriptableObject;
        private ProjectilePool projectilePool;

        private List<TowerController> activeTowers;
        private TowerView selectedTowerView;
        private int health;
        public int Money { get; private set; }

        public PlayerService(PlayerSO playerScriptableObject)
        {
            this.playerScriptableObject = playerScriptableObject;
            projectilePool = new ProjectilePool(this, playerScriptableObject.ProjectilePrefab, playerScriptableObject.ProjectilesSO);
        }

        public void Init(MapService mapService, UIService uiService, SoundService soundService)
        {
            this.mapService = mapService;
            this.uiService = uiService;
            this.soundService = soundService;
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            activeTowers = new List<TowerController>();
            health = playerScriptableObject.Health;
            Money = playerScriptableObject.Money;
            uiService.UpdateHealthUI(health);
            uiService.UpdateMoneyUI(Money);
        }

        public void Update()
        {
            foreach (TowerController tower in activeTowers)
            {
                tower?.UpdateTower();
            }

            if (Input.GetMouseButtonDown(0))
            {
                TrySelectingTower();
            }
        }

        private void TrySelectingTower()
        {
            RaycastHit2D[] hits = GetRaycastHitsAtMousePoition();

            foreach (RaycastHit2D hit in hits)
            {
                if (IsTowerCollider(hit.collider))
                {
                    SetSelectedTowerView(hit.collider.GetComponent<TowerView>());
                    return;
                }
            }

            selectedTowerView?.MakeRangeVisible(false);
        }

        private RaycastHit2D[] GetRaycastHitsAtMousePoition()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return Physics2D.RaycastAll(mousePosition, Vector2.zero);
        }

        private bool IsTowerCollider(Collider2D collider) => collider != null && !collider.isTrigger && collider.GetComponent<TowerView>() != null;

        private void SetSelectedTowerView(TowerView towerViewToBeSelected)
        {
            selectedTowerView?.MakeRangeVisible(false);
            selectedTowerView = towerViewToBeSelected;
            selectedTowerView.MakeRangeVisible(true);
        }

        public void ValidateSpawnPosition(int towerCost, Vector3 dropPosition)
        {
            if (towerCost > Money)
                return;

            mapService.ValidateSpawnPosition(dropPosition);
        }

        public void TrySpawningTower(TowerType towerType, int towerCost, Vector3 dropPosition)
        {
            if (towerCost > Money)
                return;

            if (mapService.TryGetTowerSpawnPosition(dropPosition, out Vector3 spawnPosition))
            {
                SpawnTower(towerType, spawnPosition);
                soundService.PlaySoundEffects(SoundType.SpawnTower);
            }
        }

        public void SpawnTower(TowerType towerType, Vector3 spawnPosition)
        {
            TowerSO towerScriptableObject = GetTowerScriptableObjectByType(towerType);
            TowerController tower = new TowerController(soundService, towerScriptableObject, projectilePool);

            tower.SetPosition(spawnPosition);
            activeTowers.Add(tower);
            DeductMoney(towerScriptableObject.Cost);
        }

        private TowerSO GetTowerScriptableObjectByType(TowerType towerType) => playerScriptableObject.TowersSO.Find(so => so.Type == towerType);

        public void ReturnProjectileToPool(ProjectileController projectileToReturn) => projectilePool.ReturnItem(projectileToReturn);

        public void TakeDamage(int damageToTake)
        {
            int reducedHealth = health - damageToTake;
            health = reducedHealth <= 0 ? 0 : health - damageToTake;

            uiService.UpdateHealthUI(health);
            if (health <= 0)
                PlayerDeath();
        }

        private void DeductMoney(int moneyToDedecut)
        {
            Money -= moneyToDedecut;
            uiService.UpdateMoneyUI(Money);
        }

        public void GetReward(int reward)
        {
            Money += reward;
            uiService?.UpdateMoneyUI(Money);
        }

        private void PlayerDeath() => uiService.UpdateGameEndUI(false);
    }

}