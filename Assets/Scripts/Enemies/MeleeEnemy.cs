using System.Collections;
using UnityEngine;

public class MeleeEnemy : Enemy
{

    public float stopDistance;
    public float attackSpeed;
    public float distanceToStartPursuingPlayer;

    private float attackTime;



    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) < distanceToStartPursuingPlayer)
            {
                if (Vector2.Distance(transform.position, player.position) > stopDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                }

                else
                {
                    if (Time.time >= attackTime)
                    {
                        StartCoroutine(Attack());
                        attackTime = Time.time + timeBetweenAttacks;
                    }

                }
            }
        }
    }

    // Coroutine
    IEnumerator Attack()
    {
        player.GetComponent<Player>().TakeDamage(damage);

        // Before leap
        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = player.position;

        // How much of the animation has happened
        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
            yield return null;
        }
    }
}
