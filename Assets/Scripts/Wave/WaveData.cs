using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Enemy;

namespace TowerDefense.Wave
{
    [System.Serializable]
    public struct WaveData
    {
        public int WaveID;
        public List<EnemyType> ListOfEnemies;
    }
}
