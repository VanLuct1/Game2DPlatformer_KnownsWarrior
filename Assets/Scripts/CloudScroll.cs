using UnityEngine;

public class CloudScroll : MonoBehaviour
{
    public float speed = 0.5f;
    private float width;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -width)
        {
            transform.position += Vector3.right * width * 2f;
        }
    }
}
