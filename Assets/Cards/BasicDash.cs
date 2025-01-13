using System.Collections;
using UnityEngine;

public class BasicDash : Card
{
    [SerializeField] private float dashingPower = 20f; 
    [SerializeField] private float dashingTime = 0.2f; 
    // [SerializeField] private ParticleSystem trailEffect; 
    // [SerializeField] private Sprite cardSpite; 

    public override void Use(PlayerControl player)
    {
        // Start the dash coroutine
        player.GetComponent<MonoBehaviour>().StartCoroutine(Dash(player));
    }

    public override void Description()
    {
        Debug.Log("Basic Dash!");
    }

    private IEnumerator Dash(PlayerControl player)
    {
        player.isDashing = true;

        // Ensure dependencies exist
        if (player.rb == null)
        {
            Debug.LogError("Player is missing required components for Dash.");
            yield break;
        }

        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 dashDirection;

        if (moveX != 0 || moveY != 0)
        {
            // Use input direction
            dashDirection = new Vector2(moveX, moveY).normalized;
        }
        else
        {
            // Character is standing still, calculate direction toward the mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - player.transform.position;
            direction.z = 0;
            dashDirection = new Vector2(direction.x, direction.y).normalized;
        }

        // Apply the dash
        player.rb.velocity = dashDirection * dashingPower;
        
        // Reduce stamina and activate effects
        player.tr.emitting = true;


        yield return new WaitForSeconds(dashingTime);

        // Stop dashing
        player.isDashing = false;
        player.rb.velocity = Vector2.zero;
        player.tr.emitting = false;
    }
}
