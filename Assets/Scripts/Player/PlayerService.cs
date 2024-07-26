using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerDefense.Player
{
    public class PlayerService
    {
        private PlayerSO playerSO;
        private List<TowerController> activeTowers;

        private TowerView selectedTowerView;
        private int health;
        public int Money { get; private set; }

        public PlayerService(PlayerSO playerSO)
        {
            this.playerSO = playerSO;
            SpawnMonkey(TowerType.CANON, new Vector3(2, 2, 0));
        }

        public void SpawnMonkey(TowerType towerType, Vector3 spawnPosition)
        {
            TowerSO towerSO = GetTowerSOByType(towerType);
            TowerController tower = new TowerController(towerSO);

            tower.SetPosition(spawnPosition);
            activeTowers.Add(tower);
        }

        private TowerSO GetTowerSOByType(TowerType towerType) => playerSO.TowersSO.Find(so => so.Type == towerType);
    }

}