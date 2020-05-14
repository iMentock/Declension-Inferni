using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

public class DeathHead : MonoBehaviour
{
    public float delayTime;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D collider = GetComponent<CircleCollider2D>();

        Destroy(rb, delayTime);
        Destroy(collider, delayTime);
    }
}
