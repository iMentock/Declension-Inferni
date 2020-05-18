using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorway : MonoBehaviour
{
    public GameObject exitIndicator, enemyIndicator;


    private void Reset()
    {
        // Initially set to false
        SetExitActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Update()
    {
        ShowIndicators();
    }

    private int GetEnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Count();
    }

    private GameObject[] GetEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void ShowIndicators()
    {
        if (GetEnemyCount() < 5)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            foreach (GameObject enemy in GetEnemies())
            {
                GameObject newEnemyIndicator = Instantiate(enemyIndicator, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), Quaternion.identity);
                newEnemyIndicator.transform.parent = player.transform;
                newEnemyIndicator.GetComponent<EnemyIndicator>().SetEnemy(enemy);
            }
        }

        if (GetEnemyCount() <= 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            SetExitActive(true);

            GameObject newExitIndicator = Instantiate(exitIndicator, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), Quaternion.identity);
            newExitIndicator.transform.parent = player.transform;
        }
    }

    private void SetExitActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
