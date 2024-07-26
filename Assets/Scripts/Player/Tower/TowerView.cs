using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerDefense.Player
{
    public class TowerView : MonoBehaviour
    {
        private TowerController controller;

        public void SetController(TowerController controller) => this.controller = controller;
    }
    public enum TowerAnimation
    {
        Idle,
        Shoot
    }
}