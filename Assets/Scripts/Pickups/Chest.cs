using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int health;
    public Sprite[] chestStateSprites;
    public GameObject[] weaponDrops;
    public GameObject soulDrop;

    private enum chestState { closed, damaged, open };
    private chestState currentChestState;
    private int originalHealth;
    private bool hasOpened = false;

    private void Start()
    {
        originalHealth = health;
        currentChestState = chestState.closed;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        UpdateChestState();
    }

    private void UpdateChestState()
    {
        // Over 50%
        if ((originalHealth / 2) < health)
        {
            currentChestState = chestState.closed;
        }
        // Between 0 & 50%
        else if (health > 0 && (originalHealth / 2) > health)
        {
            currentChestState = chestState.damaged;
        }
        else
        {
            currentChestState = chestState.open;
            if (!hasOpened) OpenChest();
        }

        UpdateChestSprite();
    }

    private void UpdateChestSprite()
    {
        SpriteRenderer chestSprite = GetComponent<SpriteRenderer>();

        // Change to appropriate sprite
        switch (currentChestState)
        {
            case chestState.closed:
                chestSprite.sprite = chestStateSprites[0];
                break;
            case chestState.damaged:
                chestSprite.sprite = chestStateSprites[1];
                break;
            case chestState.open:
                chestSprite.sprite = chestStateSprites[2];
                break;
        }
    }

    private void OpenChest()
    {
        int amountOfSoulsToDrop = Random.Range(1, 4);
        hasOpened = true;

        for (int i = 0; i <= amountOfSoulsToDrop; i++)
        {
            Instantiate(soulDrop, transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0.3f, 0.0f), transform.rotation);
        }

        // Remove colliders and rigidbody (no use anymore)
        {
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
        }

    }

}
