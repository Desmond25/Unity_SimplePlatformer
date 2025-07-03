using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D rigidBody;

    [SerializeField] private float lifeTime = 3f;

    private void Awake()
    {

        StartCoroutine(Timer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Projectile" && Player.Instance != null && collision.gameObject != Player.Instance.gameObject)
        {
            if (collision.gameObject.tag == "Enemy")
                collision.GetComponent<Entity>().GetDamage(1);
            Destroy(gameObject);
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
