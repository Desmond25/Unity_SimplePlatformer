using UnityEngine;

public class Entity : MonoBehaviour
{

    public virtual void GetDamage(int damage) { }
    public virtual void Die() { Debug.Log(this + " is dead"); }
}
