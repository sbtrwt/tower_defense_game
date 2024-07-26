

using TowerDefense.Player;
using UnityEngine;

namespace TowerDefense.UI
{
    public class TowerCellController
    {
        private PlayerService playerService;
        private TowerCellView towerCellView;
        private TowerCellSO towerCellSO;

        public TowerCellController(Transform cellContainer, TowerCellView towerCellPrefab, TowerCellSO towerCellScriptableObject, PlayerService playerService)
        {
            this.playerService = playerService;
            this.towerCellSO = towerCellScriptableObject;
            towerCellView = Object.Instantiate(towerCellPrefab, cellContainer);
            towerCellView.SetController(this);
            towerCellView.ConfigureCellUI(towerCellSO.Sprite, towerCellSO.Name, towerCellSO.Cost);
        }

        public void TowerDraggedAt(Vector3 dragPosition)
        {
            playerService.ValidateSpawnPosition(towerCellSO.Cost, dragPosition);
        }

        public void TowerDroppedAt(Vector3 dropPosition)
        {
            playerService.TrySpawningTower(towerCellSO.Type, towerCellSO.Cost, dropPosition);
        }
    }
}