using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour, IHealth
{
    [SerializeField] float _maxHealth = 10;
    public float maxHealth { get { return _maxHealth; } private set { _maxHealth = value; } }
    public float health { get; private set; }

    [SerializeField] Joystick joystick;
    [SerializeField] Weapon weapon;
    [SerializeField] Transform avatar;
    Inventory inventory;
    public float speed = 1.0f;
    Rigidbody2D rb2d;
    Animator animator;
    Enemy currentAim;
    public event System.Action OnPlayerDied = () => { };
    public event System.Action OnPlayerMove = () => { };

    int cachedDirection = 0; //  1: left/ 0: right

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        inventory.AddItemFixedCount(GameManager.instance.GetItemDatabase().ItemByName["Ammo"], 50);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Item")
            return;
        inventory.AddItem(collision.gameObject.GetComponent<ItemWorld>().item);
        Destroy(collision.gameObject);
    }

    void Update()
    {
        rb2d.velocity = joystick.Direction.normalized * speed;
        if(!GameManager.instance.levelBounds.Contains(transform.position))
            transform.position = GameManager.instance.levelBounds.ClosestPoint(transform.position);
        if (rb2d.velocity.magnitude > 0)
        {
            animator.SetTrigger("Move");
            OnPlayerMove?.Invoke();
        }
        else
            animator.SetTrigger("Idle");

        RefreshViewDirection();
    }

    private void FixedUpdate()
    {
        currentAim = TakeAim();
    }

    void RefreshViewDirection()
    {
        int dirCoeff;
        if (currentAim == null)
        {
            if (joystick.Direction.x != 0)
            {
                cachedDirection = joystick.Direction.x > 0 ? 0 : 1;
                dirCoeff = joystick.Direction.x > 0 ? 0 : 1;
            }
            else
            {
                dirCoeff = cachedDirection;
            }
        }
        else
        {
            dirCoeff = transform.position.x > currentAim.transform.position.x ? 1 : 0;
        }
        avatar.rotation = Quaternion.Euler(0, dirCoeff * 180, 0);
    }

    public void InitNewData(PlayerData playerData)
    {
        health = maxHealth;
        inventory = new Inventory();
        inventory.OnInventoryChanged += () => {
            playerData.itemsInInventory_Id_Count = inventory.items.Select(obj => new Vector2Int(obj.id, obj.count)).ToArray();
        };
    }

    public void LoadData(PlayerData playerData)
    {
        transform.position = playerData.position;
        health = playerData.health;
        inventory = new Inventory(playerData.itemsInInventory_Id_Count);
        inventory.OnInventoryChanged += () => {
            playerData.itemsInInventory_Id_Count = inventory.items.Select(obj => new Vector2Int(obj.id, obj.count)).ToArray();
        };
    }

    public void Shoot()
    {
        if (CheckAmmo())
        {
            inventory.DecreaseCountByOne(GameManager.instance.GetItemDatabase().ItemByName["Ammo"]);
            animator.SetTrigger("Attack");
            weapon.Shoot(currentAim);
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }
    
    public Enemy TakeAim()
    {
        Enemy enemyToShoot = null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weapon.range, LayerMask.GetMask("Enemy"));
        if(enemies.Length > 0)
        {
            Collider2D closestEnemy = enemies.OrderBy(x => (transform.position - x.transform.position).magnitude).FirstOrDefault();
            enemyToShoot = closestEnemy.gameObject.GetComponent<Enemy>();
        }
        return enemyToShoot;
    }

    public Vector3 GetCameraTarget()
    {
        return currentAim == null ? transform.position : (currentAim.transform.position + transform.position)/2;
    }
    
    bool CheckAmmo()
    {
        return inventory.Contains(GameManager.instance.GetItemDatabase().ItemByName["Ammo"]);
    }


    public Inventory GetInventory()
    {
        return inventory;
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health > 0)
            return;
        health = 0;
        Die();
    }

    void Die()
    {
        joystick.gameObject.SetActive(false);
        OnPlayerDied?.Invoke();
        Destroy(this.gameObject);
    }
}
