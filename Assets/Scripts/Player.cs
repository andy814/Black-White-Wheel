using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Camera mainCamera;
    private Wheel wheel;
    private MyColor myColor; // Set this to "Black" or "White" in the Inspector

    public float startInvincibilityDuration;
    public float flipInvincibilityDuration;
    private float invincibilityTimer;

    private GameObject hitPoint;
    private GameObject surrounding;
    private Vector3 previousPosition;

    private float surviveTime;
    private PlayerHitEvent playerHitEvent;
    private bool hit;

    public float SurviveTime
    {
        get { return surviveTime; }
        set { surviveTime = value; }
    }

    private void Awake()
    {
        Cursor.visible = false; // hide cursor
        mainCamera = Camera.main;
        wheel = GameObject.FindWithTag("Wheel").GetComponent<Wheel>();
        rb2D = GetComponent<Rigidbody2D>();
        myColor = MyColor.Black;
        invincibilityTimer = startInvincibilityDuration;
        hitPoint = transform.Find("HitPoint").gameObject;
        surrounding = transform.Find("Surrounding").gameObject;
        previousPosition = transform.position;
        hitPoint.GetComponent<TrailRenderer>().enabled = false;
        surviveTime = 0;
    }

    private void Start()
    {
        if (playerHitEvent == null)
            playerHitEvent = new PlayerHitEvent();
        playerHitEvent.AddListener(PlayerManager.Instance.HandlePlayerHit);
        Flip(startInvincibilityDuration, false);
    }

    private void Update()
    {
        Move();

        // set flip
        invincibilityTimer = Mathf.Max(0,invincibilityTimer-Time.deltaTime);

        if (invincibilityTimer==0 && hit==false)
            surviveTime += Time.deltaTime;

        if (invincibilityTimer == 0)
        {
            //transform.rotation = Quaternion.identity;
            if (Input.GetMouseButtonDown(0))
            {
                Flip(flipInvincibilityDuration, true);
            }
        }

        // Collision detection (no bullet detection here)
        hitSegment();
        hitBorder();
    }

    private void hitSegment() // We are only doing pointwise collision detection here
    { // assumption: they lie in the same z-plane
        Ring[] Rings = wheel.Rings;
        Vector3 mousePosition = gameObject.transform.position;
        Vector3 wheelPosition = wheel.transform.position;
        float distance = Vector3.Distance(mousePosition, wheelPosition);
        bool hit = false;
        for (int i = 0; i < Rings.Length; i++)
        {
            Ring ring = Rings[i];
            if (distance >= ring.InnerRadius && distance <= ring.OuterRadius) // mouse in this ring
            {
                float mouseAngle = Mathf.Rad2Deg * Mathf.Atan2(mousePosition.y - wheelPosition.y, mousePosition.x - wheelPosition.x);
                if (mouseAngle < 0)
                    mouseAngle += 360;
                float ringAngle = ring.transform.rotation.eulerAngles.z;
                //Debug.Log("mouseAngle: " + mouseAngle + ",ringAngle:" + ringAngle);
                Segment[] segments = ring.Segments;
                for (int j = 0; j < segments.Length; j++)
                {
                    Segment segment = segments[j];
                    float startAngle = (ringAngle + segment.StartAngle) % 360;
                    float endAngle = (startAngle + segment.SegmentSize) % 360;
                    if ((mouseAngle >= startAngle && mouseAngle <= endAngle) ||
                        (startAngle > endAngle && mouseAngle > startAngle)) // mouse in this segment
                    {
                        if (segment.MyColor == myColor && !isInvincible())
                        {
                            //Debug.Log("hitSEGMENT");
                            hit = true;
                            continue;
                        }
                    }
                }

            }
        }
        if (hit)
        {
            //Debug.Log("hit");
            OnHit();
        }
    }

    private void hitBorder()
    {
        Ring[] Rings = wheel.Rings;
        Vector3 mousePosition = gameObject.transform.position;
        Vector3 wheelPosition = wheel.transform.position;
        float distance = Vector3.Distance(mousePosition, wheelPosition);
        bool hit = false;
        if (distance >= Rings[Rings.Length - 1].OuterRadius && !isInvincible())
        {
            //Debug.Log("hitBorder");
            hit = true;
        }
        if(hit)
        {
            OnHit();
        }
    }

    private bool isInvincible()
    {
        return invincibilityTimer > 0 ? true : false;
    }

    private void OnHit()
    {
        //Debug.Log("hit");
        playerHitEvent.Invoke(this);
    }

    private void Flip(float flipTime, bool changeColor)
    {
        //Debug.Log("flpping");
        invincibilityTimer = flipTime;
        
        if (changeColor)
        { 
            if (myColor == MyColor.Black)
            {
                myColor = MyColor.White;
                //SurroundingSpriteRenderer.color = Color.white;
            }
            else if (myColor== MyColor.White)
            {
                myColor = MyColor.Black;
                //SurroundingSpriteRenderer.color = Color.black;
            }
        }

        StartCoroutine(RotateDuringInvincibility(flipTime, changeColor));

    }

    private IEnumerator RotateDuringInvincibility(float flipTime, bool changeColor)
    {
        SpriteRenderer surroundingSpriteRenderer = surrounding.GetComponent<SpriteRenderer>();
        TrailRenderer trailRenderer = hitPoint.GetComponent<TrailRenderer>();
        trailRenderer.enabled = true;

        Color startColor = surroundingSpriteRenderer.color;
        Color targetColor = startColor; // this single line works
        if (changeColor)
        {
            if (myColor == MyColor.Black)
                targetColor = Color.black;
            else if (myColor == MyColor.White)
                targetColor = Color.white;
        }
        /*
        if (!changeColor)
            targetColor = startColor;
        else
            if (myColor == MyColor.White)
                targetColor = Color.white;
        */
        float elapsed = 0f;
        float totalDegreesToRotate = 360f;
        float degreesRotated = 0f;

        while (elapsed < flipTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flipTime;
            float targetDegreesRotated = totalDegreesToRotate * t;
            float deltaDegrees = targetDegreesRotated - degreesRotated;

            transform.Rotate(0, 0, deltaDegrees);

            degreesRotated = targetDegreesRotated;

            // Update the color based on the progress
            surroundingSpriteRenderer.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        // Ensure the final rotation is exactly 360 degrees more than the initial rotation
        float finalDeltaDegrees = totalDegreesToRotate - degreesRotated;
        transform.Rotate(0, 0, finalDeltaDegrees);

        // Set the final color to the target color
        surroundingSpriteRenderer.color = targetColor;

        trailRenderer.enabled = false;
    }


    public void HandleOnTriggerEnter2D(Collider2D other) // called by child HitPoint
    {
        // Get the color of the collided segment using its tag
        string tagName = other.tag;

        bool hit = false;
        /*
        if (tagName=="Segment")
        {
            MyColor segColor = other.GetComponent<Segment>().MyColor;
            if (segColor == myColor)
            {
                hit = true;
            }
        }
        */
        if (tagName == "Bullet" && other.GetComponent<Bullet>().MyColor==myColor && !isInvincible())
        {
            hit = true;
        }

        if (hit)
        {
            OnHit();
        }
    }

    public void Move()
    {
        // Get mouse position in world coordinates
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

        // Update the object position
        rb2D.MovePosition(mousePosition);

        if(isInvincible()) // flipping behaviour
        { 
            Vector3 currentDirection = transform.position - wheel.transform.position;
            Vector3 previousDirection = previousPosition - wheel.transform.position;
            float angle = Vector3.SignedAngle(previousDirection, currentDirection, Vector3.forward);    
            transform.Rotate(0f, 0f, angle);
        }
        else // if it's not flipping, make the rotation more precise
        {
            Vector3 directionToCenter = (wheel.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle); 
        }

        // Update the previous position
        previousPosition = transform.position;
    }

    public IEnumerator Disappear(float duration)
    {
        SpriteRenderer hitPointSpriteRenderer = hitPoint.GetComponent<SpriteRenderer>();
        Color initialColor = hitPointSpriteRenderer.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;
            hitPointSpriteRenderer.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }
        hitPointSpriteRenderer.color = targetColor;
    }
}