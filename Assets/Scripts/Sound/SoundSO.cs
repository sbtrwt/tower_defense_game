using System;
using UnityEngine;


namespace TowerDefense.Sound
{
    [CreateAssetMenu(fileName = "SoundScriptableObject", menuName = "ScriptableObjects/SoundScriptableObject")]
    public class SoundSO : ScriptableObject
    {
        public Sounds[] audioList;
    }
    [Serializable]
    public struct Sounds
    {
        public SoundType soundType;
        public AudioClip audio;
    }
}
