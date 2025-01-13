using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Card
{
    private Coroutine attack1;
    private Coroutine attack2;
    private Coroutine attack3;
    public float attackRadius = 1.2f;
    public float attackingTime = 0.3f;
    public float pushForce = 10f;
    
    public override void Use(PlayerControl player)
    {
        if (player.isAttacking && attack3 == null) {
            player.currentMovSpeed = 3;
        } else 
        {
            player.currentMovSpeed = player.maxMovSpeed;
        }

        if (Input.GetMouseButtonDown(0) && attack2 != null)
        {
            Debug.Log("Third attack!");
            attack3 = StartCoroutine(Attack3(player));
        }
        else if (Input.GetMouseButtonDown(0) && attack1 != null)
        {
            Debug.Log("Second attack!");
            attack2 = StartCoroutine(Attack(player, true));
        }
        else if (Input.GetMouseButtonDown(0) && player.canAttack && !player.isAttacking)
        {
            Debug.Log("First attack!");
            player.rb.velocity = player.rb.velocity / 2;
            attack1 = StartCoroutine(Attack(player, false));
        } 
    }

    public override void Description()
    {
        Debug.Log("Basic Attack!");
    }

    private IEnumerator Attack(PlayerControl player, bool flip)
    {
        player.canAttack = false;
        player.isAttacking = true;

        Debug.Log("Attempting Attack");

        // Spawn attack animation at attack point
        if (player.attackAnimationPrefab != null)
        {
            // Calculate the direction from the player to the attack point
            Vector3 direction = player.attackPoint.position - player.transform.position;

            // Calculate the angle in degrees for rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Instantiate the animation with the calculated rotation
            GameObject attackAnimation = Instantiate(player.attackAnimationPrefab, player.attackPoint.position, Quaternion.Euler(0, 0, angle));
            
            if (flip) 
            {
                // Flip the object by modifying its local scale
                Vector3 flippedScale = attackAnimation.transform.localScale;
                flippedScale.y *= -1; // Flip on the Y axis
                attackAnimation.transform.localScale = flippedScale;
            }
        }

        // Attack duration
        yield return new WaitForSeconds(attackingTime);

        player.isAttacking = false;
        player.canAttack = true;

        if (!flip)
        {
            attack1 = null;
        } else
        {
            attack2 = null;
        }
    }

    private IEnumerator Attack3(PlayerControl player)
    {
        // Wait until Attack1 completes
        while (attack2 != null)
        {
            yield return null; // Wait for the next frame
        }
        
        player.canAttack = false;
        player.isAttacking = true;

        // Calculate the push direction based on the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure it's in the 2D plane
        Vector2 pushDirection = (mousePosition - player.transform.position).normalized;

        // Apply force in the calculated direction
        player.rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

        // Spawn attack animation at attack point
        if (player.attackAnimationPrefab != null)
        {
            // Calculate the direction from the player to the attack point
            Vector3 direction = player.attackPoint.position - player.transform.position;

            // Calculate the angle in degrees for rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Instantiate the animation with the calculated rotation
            Instantiate(player.attackAnimationPrefab, player.attackPoint.position, Quaternion.Euler(0, 0, angle));
            GameObject attackAnimation = Instantiate(player.attackAnimationPrefab, player.attackPoint.position, Quaternion.Euler(0, 0, angle));

            // Flip the object by modifying its local scale
            Vector3 flippedScale = attackAnimation.transform.localScale;
            flippedScale.y *= -1; // Flip on the Y axis
            attackAnimation.transform.localScale = flippedScale;
        }

        // Attack duration
        yield return new WaitForSeconds(attackingTime);

        player.isAttacking = false;
        player.canAttack = true;
        attack3 = null;
    }
}