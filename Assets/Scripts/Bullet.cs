using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private Vector2 velocity; // The speed of the bullet
    private MyColor myColor; // The color of the bullet
    private Rigidbody2D rb2D; // The Rigidbody2D component of the bullet
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;
    private Vector3 scale;

    public Vector2 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
    public MyColor MyColor
    {
        get { return myColor; }
        set { myColor = value; }
    }
    public Vector3 Scale
    {
        get { return scale; }
        set { scale = value; }
    }
    public Rigidbody2D Rb2D
    {
        get { return rb2D; }
        set { rb2D = value; }
    }

    public CircleCollider2D CircleCollider2D
    {
        get { return circleCollider2D; }
        set { circleCollider2D = value; }
    }

    public SpriteRenderer SpriteRenderer
    {
        get { return spriteRenderer; }
        set { spriteRenderer = value; }
    }

    void Awake()
    {
        
    }

    public void Init()
    {
        // Get the Rigidbody2D component
        rb2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the bullet color
        switch (myColor)
        {
            case MyColor.Black:
                spriteRenderer.color = Color.black;
                break;
            case MyColor.White:
                spriteRenderer.color = Color.white;
                break;
            default:
                spriteRenderer.color = Color.green;
                break;
        }

        // Set the bullet's velocity
        rb2D.velocity = velocity;
        transform.localScale = scale;
        int sortingMultiplier = 10000; // decide their order only by their speed.
        spriteRenderer.sortingOrder = (int) velocity.magnitude * sortingMultiplier;
    }

    public bool hitBorder()
    {
        Wheel wheel = GameData.wheel;
        Vector3 wheelPos = wheel.gameObject.transform.position;
        float outerRadius = wheel.ringRadii[wheel.ringRadii.Length - 1];
        if(Vector3.Distance(transform.position,wheelPos) >
            outerRadius + Mathf.Max(transform.localScale.x, transform.localScale.y) * circleCollider2D.radius)
        {
            return true;
        }
        return false;
    }

    void FixedUpdate()
    {
        /*
        if (hitBorder())
        {
            //Destroy(gameObject);
        }
        */
        if (!spriteRenderer.IsVisibleFrom(Camera.main))
        {
            //Debug.Log("destroying");
            // If not visible, destroy the bullet
            Destroy(gameObject);
        }
        // Update the bullet's position using its velocity
        //rb2D.MovePosition(rb2D.position + rb2D.velocity * Time.fixedDeltaTime);
    }
}