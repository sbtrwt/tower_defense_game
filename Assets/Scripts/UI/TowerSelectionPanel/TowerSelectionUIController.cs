using System.Collections.Generic;

using TowerDefense.Player;
using UnityEngine;

namespace TowerDefense.UI
{
    public class TowerSelectionUIController
    {
        private Transform cellContainer;
        private List<TowerCellController> towerCellControllers;
        //private PlayerService playerService;
        public TowerSelectionUIController(Transform cellContainer, TowerCellView towerCellPrefab, List<TowerCellSO> towerCellScriptableObjects, PlayerService playerService)
        {
            this.cellContainer = cellContainer;
            towerCellControllers = new List<TowerCellController>();

            foreach (TowerCellSO towerSO in towerCellScriptableObjects)
            {
                TowerCellController towerCell = new TowerCellController(cellContainer, towerCellPrefab, towerSO, playerService);
                towerCellControllers.Add(towerCell);
            }
        }

        public void SetActive(bool setActive) => cellContainer.gameObject.SetActive(setActive);

    }
}