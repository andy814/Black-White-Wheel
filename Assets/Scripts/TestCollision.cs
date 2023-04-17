using UnityEngine;

public class TestCollision : MonoBehaviour
{
    public float forceMagnitude = 10f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        AddForce();
        gameObject.transform.localScale = new Vector3(2f, 1f, 1f);

    }

    private void AddForce()
    {
        rb.AddForce(Vector2.left * forceMagnitude, ForceMode2D.Impulse);
    }
}
