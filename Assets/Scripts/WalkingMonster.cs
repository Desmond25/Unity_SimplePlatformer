using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    [SerializeField] private int hp = 2;
    [SerializeField] private float speed = 1.5f;

    private bool isImmune = false;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector3 dir;
    private SpriteRenderer sprite;

    private States State
    {
        get { return (States)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        dir = transform.right;
    }

    private void Update()
    {

        Move();
    }
    private void Move()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.6f, 0.1f);
        if (colliders.Length > 0 && Player.Instance != null && colliders[0].gameObject != Player.Instance.gameObject && colliders[0].gameObject.gameObject.tag != "Projectile")
        {
            dir *= -1;
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime * speed);
        sprite.flipX = dir.x > 0.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.Instance != null && collision.gameObject == Player.Instance.gameObject)
        {
            if (Player.Instance.transform.position.y - transform.position.y >= 0.83)
            {
                if (!isImmune)
                {
                    Player.Instance.DealDamage(this);
                }
            }
            else
            {
                Player.Instance.GetDamage(1);
            }
        }
    }

    public override void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
            return;
        }
        StartCoroutine(OnDamageTaken());
    }

    public override void Die()
    {
        State = States.Dead;
        GameManager.Instance.DestroyEntity(gameObject);
        Destroy(this);
    }

    private IEnumerator OnDamageTaken()
    {
        State = States.Damaged;
        isImmune = true;
        yield return new WaitForSeconds(0.3f);
        isImmune = false;
        State = States.Run;
    }
}