using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerDefense.Player
{
    public class TowerController
    {
        private TowerView towerView;
        private TowerSO towerSO;
        public TowerController(TowerSO towerSO)
        {
            this.towerSO = towerSO;
        }

        private void CreateTowerView()
        {
            towerView = Object.Instantiate(towerSO.Prefab);
            towerView.SetController(this);
            //towerView.SetTriggerRadius(monkeyScriptableObject.Range);
        }
        public void SetPosition(Vector3 positionToSet) => towerView.transform.position = positionToSet;
    }
}