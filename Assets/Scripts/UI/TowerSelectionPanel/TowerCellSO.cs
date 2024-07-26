using UnityEngine;
using TowerDefense.Player;

namespace TowerDefense.UI
{
    [CreateAssetMenu(fileName = "TowerCellScriptableObject", menuName = "ScriptableObjects/TowerCellScriptableObject")]
    public class TowerCellSO : ScriptableObject
    {
        public TowerType Type;
        public string Name;
        public Sprite Sprite;
        public int Cost;
    }
}
