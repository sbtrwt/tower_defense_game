using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Player
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerScriptableObject")]
    public class PlayerSO : ScriptableObject
    {
        public int Health;
        public int Money;
        public List<TowerSO> TowersSO;
        public List<ProjectileSO> ProjectilesSO;
        public ProjectileView ProjectilePrefab;
    }
}
