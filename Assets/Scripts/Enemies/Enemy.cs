﻿using System.Linq;
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
    public GameObject[] pickups, summonerDeathParts;

    public int healthPickupChance;
    public GameObject healthPickup;
    public GameObject deathEffect;
    public GameObject deathSprite;
    public GameObject meleeEnemyDeathHead;
    public GameObject leftDeathWing, rightDeathWing;

    private bool indicatorSet;
    private int enemyObjectID;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyObjectID = gameObject.GetInstanceID();
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

        // Body Parts
        SpewBodyParts();

        // Destroy indicator if present
        if(indicatorSet) {
            player.GetComponent<Player>().RemoveEnemyIndicator(enemyObjectID);
        }
        
        // Destroy the enemy entity
        Destroy(gameObject);
    }

    public void SetIndicator() {
        if(!indicatorSet) {
            indicatorSet = true;

            player.GetComponent<Player>().AddEnemyIndicator(gameObject, enemyObjectID);
        }
    }

    public bool GetIfIndicatorSet() {
        return indicatorSet;
    }

    private void SpewBodyParts()
    {
        if (gameObject.GetComponent<MeleeEnemy>())
        {
            float ranDistanceX = Random.Range(-700f, 700f);

            GameObject deathHead = Instantiate(meleeEnemyDeathHead, transform.position, transform.rotation);

            if (deathHead.GetComponentsInChildren<Rigidbody2D>().Count() > 1)
            {
                foreach (Rigidbody2D eye in deathHead.GetComponentsInChildren<Rigidbody2D>())
                {
                    Rigidbody2D rb = eye.GetComponent<Rigidbody2D>();
                    //dhRigidBody2D.AddForce(transform.up * 10.0f, ForceMode2D.Impulse);
                    rb.AddForce(new Vector2(ranDistanceX, 600.0f));
                    rb.MoveRotation(Random.Range(1.0f, 360.0f));
                }
            }
            else
            {
                Rigidbody2D dhRigidBody2D = deathHead.GetComponent<Rigidbody2D>();
                //dhRigidBody2D.AddForce(transform.up * 10.0f, ForceMode2D.Impulse);
                dhRigidBody2D.AddForce(new Vector2(ranDistanceX, 600.0f));
                dhRigidBody2D.MoveRotation(Random.Range(1.0f, 360.0f));
            }


        }

        if (gameObject.GetComponent<FlyingEnemy>())
        {
            float distanceX = Random.Range(-700f, 700f);

            GameObject leftWing = Instantiate(leftDeathWing, transform.position + new Vector3(-0.3f, 0.3f), transform.rotation);
            GameObject rightWing = Instantiate(rightDeathWing, transform.position + new Vector3(0.3f, 0.3f), transform.rotation);
            Rigidbody2D leftRigidBody2D = leftWing.GetComponent<Rigidbody2D>();
            Rigidbody2D rightRigidBody2D = rightWing.GetComponent<Rigidbody2D>();
            //dhRigidBody2D.AddForce(transform.up * 10.0f, ForceMode2D.Impulse);

            // Add force to both wings
            {
                // Left
                leftRigidBody2D.AddForce(new Vector2(distanceX, 60.0f));
                leftRigidBody2D.MoveRotation(Random.Range(1.0f, 360.0f));

                // Right
                rightRigidBody2D.AddForce(new Vector2(distanceX, 60.0f));
                rightRigidBody2D.MoveRotation(Random.Range(1.0f, 360.0f));
            }
        }

        if (gameObject.GetComponent<Summoner>())
        {
            float ranDistanceX = Random.Range(-700f, 700f);

            foreach (GameObject bodyPart in summonerDeathParts)
            {
                Instantiate(bodyPart, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.0f, 0.3f)), transform.rotation);

                Rigidbody2D rb = bodyPart.GetComponent<Rigidbody2D>();

                rb.AddForce(new Vector2(ranDistanceX, 600.0f));
                rb.MoveRotation(Random.Range(1.0f, 360.0f));
            }
        }
    }
}
