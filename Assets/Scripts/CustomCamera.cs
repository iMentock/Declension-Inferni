using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public Transform target;
    public float distance;

    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, target.position.z - distance);
        }
    }

}
