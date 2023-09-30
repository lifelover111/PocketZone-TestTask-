using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IHealth
{
    [SerializeField] float _maxHealth = 10;
    [SerializeField] float speed = 1;
    [SerializeField] float damage = 2;
    [SerializeField] float visionRange = 5;
    [SerializeField] float attackRange = 1;
    [SerializeField] float attackTimeDelay = 1;
    [SerializeField] GameObject itemWorldTemplate;
    [SerializeField] int[] dropItemsID = new int[] { 0 };
    [SerializeField] Transform avatar;
    Animator animator;
    float timeAttackDone;
    Player aim = null;
    public float maxHealth { get { return _maxHealth; } private set { _maxHealth = value; } }
    public float health { get; private set; }
    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (aim == null)
        {
            animator.SetBool("Idle", true);
            return;
        }
        animator.SetBool("Idle", false);

        RefreshViewDirection();
        
        if ((aim.transform.position - transform.position).magnitude <= attackRange)
        {
            rb2d.velocity = Vector3.zero;
            Attack();
            return;
        }

        rb2d.velocity = speed * (aim.transform.position - transform.position).normalized;
        animator.SetTrigger("Move");
    }

    private void FixedUpdate()
    {
        SeekAim();
    }

    void RefreshViewDirection()
    {
        int dirCoeff = transform.position.x > aim.transform.position.x ? 1 : 0;
        avatar.rotation = Quaternion.Euler(0, dirCoeff * 180, 0);
    }


    void Attack()
    {
        if (aim == null || Time.time - timeAttackDone < attackTimeDelay) 
            return;
        timeAttackDone = Time.time;
        animator.SetTrigger("Attack");
        aim.TakeDamage(damage);        
    }

    void SeekAim()
    {
        if (aim != null)
            return;

        Collider2D aimCollider = Physics2D.OverlapCircle(transform.position, visionRange, LayerMask.GetMask("Player"));
        
        if (aimCollider == null)
            return;

        aim = aimCollider.gameObject.GetComponent<Player>();
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
        DropItem();
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        GameObject go = Instantiate(itemWorldTemplate);
        go.transform.SetParent(GameManager.itemAnchor);
        int randIndex = dropItemsID[Random.Range(0, dropItemsID.Length)];
        go.GetComponent<ItemWorld>().item = GameManager.instance.GetItemDatabase().ItemById[randIndex];
        go.transform.position = transform.position;
    }
}
