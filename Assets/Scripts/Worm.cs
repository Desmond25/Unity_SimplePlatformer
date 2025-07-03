using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class Worm : Entity
{
    [SerializeField] private int hp = 1;

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.Instance != null && collision.gameObject == Player.Instance.gameObject)
        {
            if (Player.Instance.transform.position.y - transform.position.y >= 0.8)
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
}
