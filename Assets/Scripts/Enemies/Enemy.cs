using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    // public but not in inspector
    [HideInInspector]
    public Transform player;

    public float speed;
    public float timeBetweenAttacks;
    public int damage;

    public int pickupChance;
    public GameObject[] pickups;

    public int healthPickupChance;
    public GameObject healthPickup;
    public GameObject deathEffect;
    public GameObject deathSprite;
    public GameObject meleeEnemyDeathHead;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            // Between 0 and 100
            int randomNumber = Random.Range(0, 101);
            if (randomNumber < pickupChance)
            {
                GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
                Instantiate(randomPickup, transform.position, transform.rotation);
            }

            int randomHealthNumber = Random.Range(0, 101);
            if (randomHealthNumber < healthPickupChance)
            {
                Instantiate(healthPickup, transform.position, transform.rotation);
            }


            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        // Particle effect
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Make sprite
        Instantiate(deathSprite, transform.position, Quaternion.identity);

        if (gameObject.GetComponent<MeleeEnemy>())
        {
            float headDistanceX = Random.Range(-700f, 700f);

            GameObject deathHead = Instantiate(meleeEnemyDeathHead, transform.position, transform.rotation);
            Rigidbody2D dhRigidBody2D = deathHead.GetComponent<Rigidbody2D>();
            //dhRigidBody2D.AddForce(transform.up * 10.0f, ForceMode2D.Impulse);
            dhRigidBody2D.AddForce(new Vector2(headDistanceX, 600.0f));
            dhRigidBody2D.MoveRotation(Random.Range(1.0f, 360.0f));
        }

        // Destroy the enemy entity
        Destroy(gameObject);
    }
}
