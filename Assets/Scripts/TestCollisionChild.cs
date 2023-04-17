using UnityEngine;

public class TestCollisionChild : MonoBehaviour
{
    public float forceMagnitude = 10f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //AddForce();
    }

    private void AddForce()
    {
        rb.AddForce(Vector2.right * forceMagnitude, ForceMode2D.Impulse);
    }
}
