using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    public enum Team { DAMAGES_PLAYER, DAMAGES_ANIMAL}
    public Team team;

    public Vector2 direction;
    public float speed = 0.75f;

    public void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // If colliding with a fire source
        if ((collision.GetComponentInParent<FireBall>() ?? collision.GetComponentInParent<FireBall>()) != null)
            return;

        // If colliding with something flammable
        IFlammable flammable = collision.GetComponentInParent<IFlammable>() ?? collision.GetComponentInChildren<IFlammable>();
        if (flammable != null)
        {
            if (team == Team.DAMAGES_PLAYER && flammable is Character)
            {
                flammable.HandleFire();
                return;
            }
            else if (team == Team.DAMAGES_ANIMAL && flammable is Animal)
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
