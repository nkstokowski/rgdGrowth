using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    public enum Team { DAMAGES_PLAYER, DAMAGES_ENEMY}
    public Team team;

    public Vector2 direction;
    public float speed = 0.75f;

    public void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IFlammable flammable = collision.GetComponentInParent<IFlammable>() ?? collision.GetComponentInChildren<IFlammable>();

        if (flammable != null)
        {
            if (team == Team.DAMAGES_PLAYER && flammable is Character)
            {
                flammable.HandleFire();
                return;
            }
            else if (team == Team.DAMAGES_ENEMY && !(flammable is Character))
            {
                flammable.HandleFire();
                return;
            }
            else
            {
                return;
            }
        }

        Destroy(gameObject);

        GameManager.Instance.SpawnFireEffect(transform.position, transform.localScale);
    }
}
