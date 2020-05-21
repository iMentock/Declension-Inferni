using System.Collections;
using UnityEngine;

public class TargetIndicator : MonoBehaviour {
    public float hideDistance;

    public GameObject environment;
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
        if (exit) {
            if (GetEnemyCount() <= 0) {
                exit.GetComponent<ExitDoorway>().SetExitActive(true);
                Vector3 dir = exit.transform.position - transform.position;

                if (dir.magnitude < hideDistance) {
                    SetChildrenActive(false);
                } else {
                    SetChildrenActive(true);

                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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