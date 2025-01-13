using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb;

    // Some important components
    public Animator animator;
    public TrailRenderer tr;


    // Player Stats
    public float maxHealth;
    public float currentHealth;
    public HealthBar healthBar;

    // Movement
    public float maxMovSpeed = 6;
    public float currentMovSpeed;
    private float speedX, speedY;
    public bool facingRight;
    public bool isDashing;

    // Attacking
    public Transform attackPoint;
    public GameObject attackAnimationPrefab;
    public float attackRadius = 2.0f;
    public bool isAttacking = false;
    public bool canAttack = true;

    // Cards
    private Card dashCard;
    private Card basicAttack; 

    // Deck Inventory
    private Card[] activeCards = new Card[5];
    private List<Card> deck = new List<Card>();


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentMovSpeed = maxMovSpeed;

        dashCard = gameObject.AddComponent<BasicDash>();
        basicAttack = gameObject.AddComponent<BasicAttack>();
    }

    // Update is called once per frame
    void Update()
    {   
        UpdateAttackPointPosition();

        if (Input.GetMouseButtonDown(0))
        {
            basicAttack.Description();
            basicAttack.Use(this);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashCard.Use(this);
            dashCard.Description();
        }
        
        if (isDashing) { return; }

        // Testing hp
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(20);
        }

        // Movement
        if (!isAttacking) 
        {
        speedX = Input.GetAxisRaw("Horizontal") * currentMovSpeed;
        animator.SetFloat("xVelocity", Mathf.Abs(speedX));

        speedY = Input.GetAxisRaw("Vertical") * currentMovSpeed;
        animator.SetFloat("xVelocity", Mathf.Abs(speedY));

        rb.velocity = new Vector2(speedX, speedY);
        animator.SetFloat("velocity", Mathf.Abs(speedX) + Mathf.Abs(speedY));
        }

        // Get horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Flip the player based on keyboard input
        if (horizontalInput > 0 && facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        healthBar.setHealth(currentHealth);
    }

    void UpdateAttackPointPosition()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction and distance from the player to the cursor
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0; // Ensure the direction is in the 2D plane
        float distance = direction.magnitude;

        // If the cursor is within the attack radius, set the attack point to the cursor
        if (distance <= attackRadius)
        {
            mousePosition.z = 0;
            attackPoint.position = mousePosition;
        }
        else
        {
            // If the cursor is beyond the radius, clamp the attack point to the radius
            direction = direction.normalized * attackRadius;
            attackPoint.position = transform.position + direction;
        }
    }
}
