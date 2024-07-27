
using UnityEngine;
using TMPro;


namespace TowerDefense.UI
{
    public class TowerCellView : MonoBehaviour
    {
        private TowerCellController controller;

        [SerializeField] private TowerImageHandler towerImageHandler;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;

        public void SetController(TowerCellController controllerToSet) => controller = controllerToSet;

        public void ConfigureCellUI(Sprite spriteToSet, string nameToSet, int costToSet)
        {
            towerImageHandler.ConfigureImageHandler(spriteToSet, controller);
            nameText.SetText(nameToSet);
            costText.SetText(costToSet.ToString());
        }
    }
}
