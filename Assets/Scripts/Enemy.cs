using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        // Get the SpriteRenderer component and store the original color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log("Enemy has " + currentHealth + " hp left!");

        // Flash white
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashWhite());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashWhite()
    {
        // Change color to white
        spriteRenderer.color = Color.blue;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.2f);

        // Revert to the original color
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        Debug.Log("Enemy Died");

        // Play a death animation if needed

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
