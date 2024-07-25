using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Wave
{
    [CreateAssetMenu(fileName = "WaveConfiguration", menuName = "ScriptableObjects/WaveConfiguration")]
    public class WaveConfigurationSO:ScriptableObject
    {
        public int MapID;
        public List<WaveData> WaveDatas;
    }
}
