using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public int maxJumps = 2; // 1 = normal jump, 2 = double jump

    [Header("Better Jump Gravity")]
    public float fallMultiplier = 2.5f;      // gravity when falling
    public float lowJumpMultiplier = 2f;     // gravity when jump held shortly

    private Rigidbody2D rb;
    private int jumpCount;

    //attributes for pickup items
    [field: SerializeField] public int Coin { get; set; } = 0;
    [field: SerializeField] public int Health { get; set; } = 50;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;

        Debug.Log($"Player's initial Health: {Health} | Coin: {Coin}");
    }

    /// <summary>
    /// OOP: methods to interact with items
    /// </summary>

    //Player script
    public void OnTriggerEnter2D(Collider2D other)
    {
/*        Item item = other.GetComponent<Item>();
        if (item)  // (item != null) // implement polymorphism
        {
            item.PickUp(this); //works with all items
        }*/
    }

    public void AddCoin(int value)
    {
        Coin += value;
        Debug.Log("Picked up a coin! Total coins: " + Coin);
    }

    public void Heal(int value)
    {
        Health += value;
        Debug.Log("Eat a Heart! Health: " + Health);
    }

    public void DecreaseHealth(int value)
    {
        Health -= value;
        Debug.Log("Egggg..health decreased! Health: " + Health);
    }


    /// <summary>
    /// Player controller
    /// </summary>
    void Update()
    {
        Move();
        Jump();
        BetterJumpGravity();
        ResetJumpWhenStopped();
    }

    void Move()
    {
        float input = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            // reset vertical velocity before jumping to remove delay
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
        }
    }

    void BetterJumpGravity()
    {
        // Falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Short tap jump (release space early)
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void ResetJumpWhenStopped()
    {
        // When vertical movement nearly stops, assume grounded
        if (Mathf.Abs(rb.linearVelocity.y) < 0.05f)
        {
            jumpCount = maxJumps;
        }
    }
}
