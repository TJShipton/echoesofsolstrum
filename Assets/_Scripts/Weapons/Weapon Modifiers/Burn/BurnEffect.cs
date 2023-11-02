using System.Collections;
using UnityEngine;

public class BurnEffect : Effect
{
    private int burnDamage = 10; // Damage per second
    private float burnDuration = 3.0f; // Duration in seconds
    private Coroutine burnCoroutine;

    public override void StartEffect(Enemy enemy)
    {
        // Start the burn effect as a Coroutine
        burnCoroutine = enemy.StartCoroutine(ApplyBurn(enemy));
    }

    public override void UpdateEffect(Enemy enemy)
    {
        // You can update the effect here if needed
        // For example, you could increase the burn damage over time
    }

    public override void EndEffect(Enemy enemy)
    {
        // Stop the Coroutine if it's running
        if (burnCoroutine != null)
        {
            enemy.StopCoroutine(burnCoroutine);
        }
    }

    private IEnumerator ApplyBurn(Enemy enemy)
    {
        Debug.Log("Burn effect Applied");
        float startTime = Time.time;

        while (Time.time - startTime < burnDuration)
        {
            enemy.TakeDamage(burnDamage, null); // Apply burn damage
            yield return new WaitForSeconds(1.0f); // Wait for 1 second
        }

        // Optionally, remove this effect after it's done
        enemy.RemoveEffect(this);
    }
}
