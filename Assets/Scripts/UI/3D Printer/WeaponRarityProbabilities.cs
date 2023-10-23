using UnityEngine;

public class WeaponRarityProbabilities : MonoBehaviour
{
    [System.Serializable]
    public struct RarityProbability
    {
        public WeaponTier tier;
        public float probability;
    }

    public RarityProbability[] rarities;
}
