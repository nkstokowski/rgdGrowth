using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int DEFAULT_FIRE_EFFECT_DURATION = 2;
    public FireBall fireBall;
    public SpriteRenderer fireEffect;
    public SpriteRenderer wateringEffect;

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

    public void SpawnWaterEffect(Vector2 position, Vector2 scale, float duration = DEFAULT_FIRE_EFFECT_DURATION)
    {
        SpriteRenderer newWateringEffect = Instantiate(wateringEffect);

        newWateringEffect.transform.position = position;
        newWateringEffect.transform.localScale = scale;

        Destroy(newWateringEffect.gameObject, duration);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
