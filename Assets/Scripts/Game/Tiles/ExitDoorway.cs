using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorway : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void SetExitActive(bool value) {
        gameObject.SetActive(value);
    }
}