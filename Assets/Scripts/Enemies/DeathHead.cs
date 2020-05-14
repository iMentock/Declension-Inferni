using System.Linq;
using UnityEngine;

public class DeathHead : MonoBehaviour
{
    public float delayTime;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Collider2D collider = null;

        if (GetComponentInChildren<CircleCollider2D>())
        {
            collider = GetComponent<CircleCollider2D>();
        }

        else if (GetComponentInChildren<CapsuleCollider2D>())
        {
            collider = GetComponent<CapsuleCollider2D>();
        }

        else
        {
            collider = GetComponent<BoxCollider2D>();
        }

        Destroy(rb, delayTime);

        if (collider)
        {
            Destroy(collider, delayTime);
        }
    }

}
