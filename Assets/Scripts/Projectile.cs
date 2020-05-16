using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy this projectile once lifetime has passed
        //Destroy(gameObject, lifeTime);
        Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Vector2.up is forward
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Built in function when trigger fires -- collision has collision object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            DestroyProjectile();
        }

        if (collision.tag == "Wall")
        {
            DestroyProjectile();
        }

        if (collision.name == "Chest")
        {
            collision.GetComponent<Chest>().TakeDamage(damage);
            DestroyProjectile();
        }

        /*
        if (collision.tag == "Boss")
        {
            collision.GetComponent<Boss>().TakeDamage(damage);
            DestroyProjectile();
        }
        */
    }
}
