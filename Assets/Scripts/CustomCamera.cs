using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public Transform target;
    public float distance;

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z - distance);
    }
}
