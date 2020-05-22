using Boo.Lang;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BuildType { mobile, web };

public class Player : MonoBehaviour {
    public float speed;
    public int health;
    public Transform weaponSpawnPoint;
    public Joystick movementJoystick;
    public Joystick rotationJoystick;
    public BuildType controllerType;
    public GameObject enemyIndicatorPrefab;

    //public Image[] hearts;
    //public Sprite fullHeart;
    //public Sprite emptyHeart;
    //public Animator hurtAnim;
    public Text soulCountText;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveAmount;
    private Animator _animator;

    private int soulCount;
    private Hashtable enemyIndicators = new Hashtable();

    private void Start() {
        // Set equal to rigidbody that is attached to character
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        soulCountText.text = soulCount.ToString();

        if (controllerType == BuildType.web) {
            movementJoystick.gameObject.SetActive(false);
            rotationJoystick.gameObject.SetActive(false);
        }
    }

    private void Update() {
        Vector2 moveInput;

        // Movement based on build (web or mobile)
        {
            if (controllerType == BuildType.mobile) {
                moveInput = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
                // *** normalized will not move faster diagonally ***
                _moveAmount = moveInput.normalized * speed;

                Vector2 joystickDirection = rotationJoystick.Direction;
                float angle = Mathf.Atan2(joystickDirection.y, joystickDirection.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                transform.rotation = rotation;
            } else {
                moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                // *** normalized will not move faster diagonally ***
                _moveAmount = moveInput.normalized * speed;

                // Face mouse
                Vector2 direction = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                transform.rotation = rotation;
            }
        }

        // Detect if player is moving
        if (moveInput != Vector2.zero) {
            _animator.SetBool("isRunning", true);
        } else {
            _animator.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate() {
        // Don't forget fixedDeltaTime to make frame rate independent
        _rigidbody.MovePosition(_rigidbody.position + _moveAmount * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damageAmount) {
        health -= damageAmount;
        //UpdateHealthUI(health);
        //hurtAnim.SetTrigger("hurt");

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public void AddSoul() {
        soulCount++;
        soulCountText.text = soulCount.ToString();
    }

    public void AddEnemyIndicator(GameObject forEnemy, int objectID) {
        GameObject indicator = Instantiate(enemyIndicatorPrefab, GameObject.FindGameObjectWithTag("Player").transform);
        indicator.GetComponent<EnemyIndicator>().SetEnemy(forEnemy);

        // Hash table construction <enemyObjectID, indicator>
        enemyIndicators.Add(objectID, indicator);
    }

    public void RemoveEnemyIndicator(int enemyObjectID) {
        Destroy((GameObject)enemyIndicators[enemyObjectID]);
    }

    /*
    public void ChangeWeapon(Weapon weaponToEquip)
    {
        Destroy(GameObject.FindGameObjectWithTag("Weapon"));
        Instantiate(weaponToEquip, weaponSpawnPoint.position, weaponSpawnPoint.rotation, weaponSpawnPoint);
    }

    void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void Heal(int healAmount)
    {
        if (health + healAmount > 5)
        {
            health = 5;
        }
        else
        {
            health += healAmount;
        }
        UpdateHealthUI(health);
    }
    */
}