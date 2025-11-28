using UnityEngine;

public class PlantClickHandler : MonoBehaviour
{
    public int scoreValue = 1;   // Сколько очков даёт эта трава
    public float lifetime = 3f;  // Через сколько секунд исчезает сама

    private float _timer;

    void Start()
    {
        _timer = lifetime;
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            ForestGameManager.Instance.RemovePlant(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        ForestGameManager.Instance.CollectPlant(gameObject);
    }

    public void SetLifetime(float time)
    {
        lifetime = time;
        _timer = time;
    }
}
