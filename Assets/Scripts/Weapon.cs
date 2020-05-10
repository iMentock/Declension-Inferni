﻿using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectile;
    public Transform shotPoint;
    public float timeBetweenShots;
    public Joystick rotationJoystick;
    [Range(0f, 0.99f)] public float joystickFireSensitivity;

    private float _shotTime;
    //private Animator cameraAnim;

    private void Start()
    {
        //cameraAnim = UnityEngine.Camera.main.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Point weapon at mouse pointer
        //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //transform.rotation = rotation;

        float horizontalValue = Math.Abs(rotationJoystick.Horizontal);
        float verticlValue = Math.Abs(rotationJoystick.Vertical);
        print(verticlValue);
        if (horizontalValue >= joystickFireSensitivity || verticlValue >= joystickFireSensitivity)
        {
            if (Time.time >= _shotTime)
            {
                // If weapon is facing right
                //Quaternion bulletRotate = Quaternion.Euler(0f, 0f, -90f);
                Instantiate(projectile, shotPoint.position, transform.rotation);
                //cameraAnim.SetTrigger("shake");
                _shotTime = Time.time + timeBetweenShots;
            }
        }

        //if (Input.GetMouseButton(0))
        //{
        //    if (Time.time >= _shotTime)
        //    {
        //        // If weapon is facing right
        //        //Quaternion bulletRotate = Quaternion.Euler(0f, 0f, -90f);
        //        Instantiate(projectile, shotPoint.position, transform.rotation);
        //        //cameraAnim.SetTrigger("shake");
        //        _shotTime = Time.time + timeBetweenShots;
        //    }
        //}
    }
}