using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class Player : Entity
{
    [SerializeField] private Text hpText;
    [SerializeField] private int hp = 5;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private float attackCoolDown = 1f;
    [SerializeField] PlayerProjectile Projectile;
    [SerializeField] private float projectileSpeed = 6.5f;

    private bool isGrounded = false;
    private bool isImmune = false;
    private bool attackOnCoolDown = false;
    private Vector3 direction = Vector3.right;

    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private Animator animator;

    public static Player Instance { get; private set; }

    private States State
    {
        get { return (States)animator.GetInteger("State"); }
        set
        {
            if (!isImmune)
                animator.SetInteger("State", (int)value);
        }
    }

    private void Awake()
    {
        Instance = this;
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded) State = States.Idle;
        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetKeyDown(KeyCode.F) && canAttack && !attackOnCoolDown)
        {
            Attack();
        }
    }

    private void Run()
    {
        if (isGrounded) State = States.Run;

        direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, moveSpeed * Time.deltaTime);
        sprite.flipX = direction.x < 0.0f;
    }

    private void Jump()
    {
        rigidBody.linearVelocityY = 0;
        rigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
        if (!isGrounded) State = States.Jump;
    }

    public override void GetDamage(int damage)
    {
        if (isImmune)
            return;
        hp -= damage;
        if (hp < 0) hp = 0;
        hpText.text = hp.ToString();
        if (hp == 0)
        {
            Die();
            return;
        }
        StartCoroutine(OnDamageTaken());
    }

    public override void Die()
    {
        State = States.Dead;
        GameManager.Instance.OnLose();
        Destroy(this);
    }

    private IEnumerator OnDamageTaken()
    {
        State = States.Damaged;
        isImmune = true;
        yield return new WaitForSeconds(0.3f);
        isImmune = false;
    }

    public void DealDamage(Entity target)
    {
        target.GetDamage(1);
        Bounce();
    }

    private void Bounce()
    {
        float bounceForce = 6f;
        rigidBody.linearVelocityY = 0;
        rigidBody.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        PlayerProjectile projectile = Instantiate(Projectile, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        projectile.sprite.flipX = direction.x < 0.0f;
        projectile.rigidBody.linearVelocityX = (direction.x < 0.0f ? -1 : 1) * projectileSpeed;
        StartCoroutine(OnAttack());
    }
    private IEnumerator OnAttack()
    {
        attackOnCoolDown = true;
        yield return new WaitForSeconds(attackCoolDown);
        attackOnCoolDown = false;
    }
}

public enum States
{
    Idle,
    Run,
    Jump,
    Damaged,
    Dead,
    Attack
}