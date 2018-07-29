using UnityEngine;

public class Sampling : MonoBehaviour, IWaterable
{
    public GameObject wateredPlant;

    void IWaterable.HandleWatering()
    {
        Destroy(gameObject);
        Instantiate(wateredPlant, transform.position, transform.rotation);
        GameManager.Instance.SpawnWaterEffect(transform.position, transform.localScale);
    }
}
