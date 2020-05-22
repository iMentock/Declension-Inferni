using UnityEngine;

public class EnemyIndicator : MonoBehaviour {
    public float hideDistance;
    private GameObject enemy;

    public void SetEnemy(GameObject enemyGameObject) {
        enemy = enemyGameObject;
    }

    public GameObject GetEnemy() {
        return enemy;
    }

    private void Update() {
        if (enemy) {
            Vector3 dir = enemy.transform.position - transform.position;

            if (dir.magnitude < hideDistance) {
                SetChildrenActive(false);
            } else {
                SetChildrenActive(true);

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void SetChildrenActive(bool value) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(value);
        }
    }
}