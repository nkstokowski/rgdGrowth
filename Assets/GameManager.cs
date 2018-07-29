using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int DEFAULT_FIRE_EFFECT_DURATION = 2;
    public FireBall fireBall;
    public SpriteRenderer fireEffect;

    public void Awake()
    {
        Instance = this;
    }

    public void LaunchFireBall(Vector2 position, Vector2 direction, float speed, FireBall.Team team)
    {
        FireBall newFireBall = Instantiate(fireBall);

        fireBall.team = team;
        fireBall.direction = direction.normalized;
        fireBall.speed = speed;
        fireBall.transform.position = position;
        fireBall.transform.right = direction;
    }

    public void SpawnFireEffect(Vector2 position, Vector2 scale, float duration = DEFAULT_FIRE_EFFECT_DURATION)
    {
        SpriteRenderer newFireEffect = Instantiate(fireEffect);

        newFireEffect.transform.position = position;
        newFireEffect.transform.localScale = scale;

        Destroy(newFireEffect.gameObject, duration);
    }
}
