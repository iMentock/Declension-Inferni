using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    private GameObject enemy;

    public float hideDistance;

    public void SetEnemy(GameObject enemyGameObject)
    {
        enemy = enemyGameObject;
    }

    private void Update()
    {
        if (enemy)
        {
            Vector3 dir = enemy.transform.position - transform.position;

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
        else
        {
            Destroy(gameObject);
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
