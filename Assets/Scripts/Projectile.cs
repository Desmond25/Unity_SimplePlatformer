using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D rigidBody;

    [SerializeField] private float lifeTime = 3f;
    //[SerializeField] private float speed = 3f;
    //public AttackDirection direction = AttackDirection.Left;


    private void Awake()
    {
        //sprite = GetComponentInChildren<SpriteRenderer>();
        //sprite.flipX = direction == AttackDirection.Right;

        StartCoroutine(Timer());
    }

    private void Update()
    {
        //Vector3 projectileOffset = direction == AttackDirection.Left ? Vector3.left : Vector3.right;
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + projectileOffset, Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Projectile")
        {
            if (Player.Instance != null && collision.gameObject == Player.Instance.gameObject)
                Player.Instance.GetDamage(1);
            Destroy(gameObject);
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
