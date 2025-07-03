using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class Slime : Entity
{
    [SerializeField] private int hp = 2;
    [SerializeField] private float attackDuration = 0.3f;
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private float firstAttackOffset = 0f;
    [SerializeField] private AttackDirection attackDirection = AttackDirection.Left;
    [SerializeField] private Projectile Projectile;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float projectileSpeed = 3f;


    private Animator animator;
    private bool isImmune = false;

    private States State
    {
        get { return (States)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (attackDirection == AttackDirection.Right)
            SpawnPoint.localPosition = new Vector3(SpawnPoint.localPosition.x * (-1), SpawnPoint.localPosition.y, SpawnPoint.localPosition.z);

        StartCoroutine(Attack());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.Instance != null && collision.gameObject == Player.Instance.gameObject)
        {
            if (Player.Instance.transform.position.y - transform.position.y >= 0.6)
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
        State = States.Idle;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(firstAttackOffset);
        while (true)
        {
            if (!isImmune)
            {
                State = States.Attack;
                Projectile projectile = Instantiate(Projectile, SpawnPoint.position, Quaternion.identity);
                projectile.sprite.flipX = attackDirection == AttackDirection.Left;
                projectile.rigidBody.linearVelocityX = (attackDirection == AttackDirection.Right ? 1 : -1) * projectileSpeed;
                yield return new WaitForSeconds(attackDuration);
                if (!isImmune)
                    State = States.Idle;
            }
                yield return new WaitForSeconds(attackCoolDown - attackDuration);
        }
    }
}
public enum AttackDirection
{
    Left,
    Right
}


