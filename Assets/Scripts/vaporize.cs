using UnityEngine;
using UnityEngine.UI;

public class vaporize : MonoBehaviour
{
    [Header("Эффект испарения")]
    [SerializeField] private ParticleSystem vaporEffect;   // Ваш эффект пара из Asset Store

    [Header("Кнопка")]
    [SerializeField] private Button buttonRestart;   // Ваш эффект пара из Asset Store


    private Rigidbody rb;
    private movement playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<movement>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Debug.Log("Куб коснулся огня!");
            StartVaporize();
        }
    }

    void StartVaporize()
    {
        playerMovement.enabled = false;

        ParticleSystem vapor = Instantiate(vaporEffect, transform.position, Quaternion.identity);
        vapor.Play();
        float effectDuration = vapor.main.duration;
        Destroy(vapor.gameObject, effectDuration);

        GetComponent<Renderer>().enabled = false;

        Debug.Log("Куб коснулся огня и испарился!");

        RectTransform rect = buttonRestart.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.anchoredPosition = Vector2.zero;

    }
}
