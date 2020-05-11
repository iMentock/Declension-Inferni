using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public float stopDistance;
    public Transform shotPoint;
    public GameObject enemyBullet;

    private float _attackTime;
    private Animator _anim;

    public override void Start()
    {
        base.Start();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) > stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            if (Vector2.Distance(transform.position, player.position) <= stopDistance && Time.time > _attackTime)
            {
                _attackTime = Time.time + timeBetweenAttacks;
                _anim.SetTrigger("attack");
            }
        }
    }

    public void RangedAttack()
    {
        // Point weapon at mouse pointer
        Vector2 direction = player.position - shotPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotPoint.rotation = rotation;

        Instantiate(enemyBullet, shotPoint.position, shotPoint.rotation);
    }
}
