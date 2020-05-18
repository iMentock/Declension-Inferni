using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TargetIndicator : MonoBehaviour
{
    private Transform exitTile;
    public float hideDistance;

    private void Start()
    {
        StartCoroutine(GetExitLocation());
    }

    IEnumerator GetExitLocation()
    {
        yield return new WaitForSeconds(2);

        exitTile = GameObject.FindGameObjectWithTag("Exit").GetComponent<Transform>();

        if (exitTile) print("Found");
    }

    private void Update()
    {
        if (exitTile)
        {
            Vector3 dir = exitTile.transform.position - transform.position;

            if (dir.magnitude < hideDistance)
            {
                SetChildrenActive(false);
            }
            else
            {
                SetChildrenActive(true);

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    void SetChildrenActive(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}
