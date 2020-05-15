using UnityEngine;

public class SoulPickup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().AddSoul();
            Destroy(gameObject);
        }
    }

}
