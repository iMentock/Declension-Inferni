using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int health;
    public Sprite[] chestStateSprites;

    private enum chestState { closed, damaged, open };
    private chestState currentChestState;
    private int originalHealth;

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
            // TODO open chest
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

}
