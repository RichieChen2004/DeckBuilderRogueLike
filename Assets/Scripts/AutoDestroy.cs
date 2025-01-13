using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime = 1.0f;
    public float xRadius = 2.0f; // Radius along the x-axis
    public float yRadius = 1.0f; // Radius along the y-axis
    public LayerMask enemyLayers;
    public float atkDmg = 40;

    private LineRenderer lineRenderer;
    private int segments = 50; // Number of segments for the oval
    private Quaternion rotation; // Rotation to align oval with cursor
    public List<Collider2D> damagedEnemies = new List<Collider2D>();

    public bool drawOval = true;
    void Start()
    {
        // Get the direction from the player to the cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0; // Ensure 2D direction
        rotation = Quaternion.FromToRotation(Vector3.right, direction.normalized);

        // Set up the LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments + 1; // +1 to close the oval
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // Draw the oval
        if (drawOval) {DrawOval();}

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Detect enemies in range of the oval attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, Mathf.Max(xRadius, yRadius), enemyLayers);

        // Damage them if they haven't been damaged already
        foreach (Collider2D enemy in hitEnemies)
        {
            if (!damagedEnemies.Contains(enemy)) // Check if the enemy is not in damagedEnemies
            {
                // Check if the enemy is within the rotated oval's bounds
                Vector2 relativePosition = rotation * (enemy.transform.position - transform.position);
                if (IsWithinOval(relativePosition))
                {
                    Debug.Log("We hit an enemy");
                    enemy.GetComponent<Enemy>().TakeDamage(atkDmg);
                    damagedEnemies.Add(enemy); // Add the enemy to the list after damaging it
                }
            }
        }
    }

    void DrawOval()
    {
        for (int i = 0; i < segments + 1; i++)
        {
            float angle = i * Mathf.PI * 2 / segments; // Angle for each segment
            float x = Mathf.Cos(angle) * xRadius;
            float y = Mathf.Sin(angle) * yRadius;
            Vector3 rotatedPoint = rotation * new Vector3(x, y, 0); // Rotate the point
            lineRenderer.SetPosition(i, rotatedPoint + transform.position);
        }
    }

    bool IsWithinOval(Vector2 position)
    {
        // Normalize the position relative to the oval radii
        float normalizedX = position.x / xRadius;
        float normalizedY = position.y / yRadius;

        // Check if the normalized distance is within the oval bounds
        return (normalizedX * normalizedX + normalizedY * normalizedY) <= 1;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // Draw an approximation of the rotated oval using Gizmos
        if (Application.isPlaying)
        {
            for (int i = 0; i < segments; i++)
            {
                float angle1 = i * Mathf.PI * 2 / segments;
                float angle2 = (i + 1) * Mathf.PI * 2 / segments;

                Vector3 point1 = rotation * new Vector3(Mathf.Cos(angle1) * xRadius, Mathf.Sin(angle1) * yRadius, 0);
                Vector3 point2 = rotation * new Vector3(Mathf.Cos(angle2) * xRadius, Mathf.Sin(angle2) * yRadius, 0);

                Gizmos.DrawLine(point1 + transform.position, point2 + transform.position);
            }
        }
        else
        {
            // Default gizmo in the editor
            Gizmos.DrawWireSphere(transform.position, Mathf.Max(xRadius, yRadius));
        }
    }
}
