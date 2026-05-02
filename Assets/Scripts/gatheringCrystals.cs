using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gatheringCrystals : MonoBehaviour
{
    [SerializeField] private TMP_Text crystalText;
    [SerializeField] private Button bonusText;

    [SerializeField] private int threshold = 5;

    private int crystalCount = 0;

    void Start()
    {
        bonusText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            AddCrystals();            
            Debug.Log($"Кол-во кристаллов: {crystalCount}");
            Destroy(other.gameObject);
            if (crystalCount >= threshold)
            {
                bonusText.gameObject.SetActive(true);
            }
        }
    }

    public void AddCrystals()
    {
        ++crystalCount;
        crystalText.text = $"Кол-во кристаллов: {crystalCount}";
    }

    public void SubstructCrystals()
    {
        crystalCount -= threshold;
        crystalText.text = $"Кол-во кристаллов: {crystalCount}";
    }
}
