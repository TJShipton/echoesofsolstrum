using UnityEngine;

public class WeaponRaritySelector : MonoBehaviour
{
    public WeaponRarityProbabilities rarityProbabilities;

    public WeaponTier GetRandomTier()
    {
        float totalProbability = 0f;
        foreach (var rarity in rarityProbabilities.rarities)
        {
            totalProbability += rarity.probability;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;
        foreach (var rarity in rarityProbabilities.rarities)
        {
            cumulativeProbability += rarity.probability;
            if (randomValue <= cumulativeProbability)
            {
                return rarity.tier;
            }
        }
        return WeaponTier.Common;  // Default to Common if something goes wrong
    }
}
