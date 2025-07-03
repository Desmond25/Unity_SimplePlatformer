using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.Instance != null && collision.gameObject == Player.Instance.gameObject)
        {
            Player.Instance.GetDamage(1);
        }
    }
}
