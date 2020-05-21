using System.Collections;
using UnityEngine;

public class TargetIndicator : MonoBehaviour {
    public float hideDistance;

    public GameObject environment, enemyIndicator;
    private GameObject exit;

    private void Start() {
        StartCoroutine(WaitForExitToSpawn());

        // Set arrow inactive
        SetChildrenActive(false);
    }

    private IEnumerator WaitForExitToSpawn() {
        yield return new WaitForSeconds(3.0f);

        // After waiting find the exit
        {
            DungeonManager dm = environment.GetComponent<DungeonManager>();
            exit = dm.GetExitObject();
            print(exit);

            // If it's found set it to inactive till enemies are destroyed
            if (exit) {
                exit.GetComponent<ExitDoorway>().SetExitActive(false);
            }
        }
    }

    private void Update() {
        int enemyCount = GetEnemyCount();
        if (exit) {
            if (enemyCount <= 0) {
                exit.GetComponent<ExitDoorway>().SetExitActive(true);
                Vector3 dir = exit.transform.position - transform.position;

                if (dir.magnitude < hideDistance) {
                    SetChildrenActive(false);
                } else {
                    SetChildrenActive(true);

                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            } else if (enemyCount != 0 && enemyCount < 6) {
                foreach(EnemyIndicator enemyInd in gameObject.GetComponents<EnemyIndicator>()) {
                    Destroy(enemyInd.gameObject);
                }
                foreach(GameObject enemy in GetEnemies()) {
                    GameObject indicator = Instantiate(enemyIndicator, GameObject.FindGameObjectWithTag("Player").transform);
                    indicator.GetComponent<EnemyIndicator>().enemy = enemy;
                }
            }
        }
    }

    private int GetEnemyCount() {
        GameObject[] enemies = GetEnemies();
        print("Enemies left --> " + enemies.Length);
        return enemies.Length;
    }

    private GameObject[] GetEnemies() {
        return GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void SetChildrenActive(bool value) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(value);
        }
    }
}