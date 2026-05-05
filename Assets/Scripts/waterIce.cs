using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class waterIce : MonoBehaviour
{
    [Header("Объекты")]
    [SerializeField] private GameObject iceObject;
    [SerializeField] public Material iceMaterial;
    [SerializeField] public Button firstButton;
    [SerializeField] public Button secondButton; 
    [SerializeField] private gatheringCrystals gatheringCrystals;  // Перетащить в Inspector

    [Header("Настройки")]
    [SerializeField] private KeyCode pressKey = KeyCode.F;
    [SerializeField] private float freezeDuration = 2f;

    private float currentProgress = 0f;
    private bool isFreezing = false;
    private bool isMelting = false;
    private float animationTime = 0f;
    private bool isWaterActive = true;

    private Collider iceCollider;

    void Start() {
        iceCollider = iceObject.GetComponent<Collider>();
        iceCollider.enabled = !isWaterActive;
        iceMaterial.renderQueue = 2000;
        iceMaterial.SetFloat("_Progress", currentProgress);
    }

    void Update()
    {
        if (Input.GetKeyDown(pressKey))
        {
            if (isWaterActive)
            {
                StartFreezing();
            }
            else
            {
                iceMaterial.renderQueue = 2000;
                StartMelting();
            }
        }

        if (isFreezing || isMelting)
        {
            UpdateTransition();
        }
    }

    public void StartFreezing()
    {
        isFreezing = true;
        isMelting = false;
        animationTime = 0f;
        currentProgress = iceMaterial.GetFloat("_Progress");
    }

    public void StartMelting()
    {
        isMelting = true;
        isFreezing = false;
        animationTime = 0f;
        currentProgress = iceMaterial.GetFloat("_Progress");
    }

    private void UpdateTransition()
    {
        animationTime += Time.deltaTime;
        float duration = freezeDuration;

        float targetProgress;
        if (isFreezing)
            targetProgress = 1.1f;
        else
            targetProgress = 0f;

        float t = animationTime / duration;

        float easeT = Mathf.SmoothStep(0, 1.1f, t);

        float newProgress = Mathf.Lerp(currentProgress, targetProgress, easeT);
        // Debug.Log($"{newProgress} {t}");


        SetIceVisibility(newProgress);

        if (t >= 1.1f)
        {
            if (isWaterActive)
            {
                iceMaterial.renderQueue = 3000;
            }
            isWaterActive = !isWaterActive;
            Debug.Log(isFreezing ? "Заморозка завершена!" : "Таяние завершено!");
            if (isFreezing)
            {
                firstButton.gameObject.SetActive(false);
                secondButton.gameObject.SetActive(true);
                gatheringCrystals.SubstructCrystals();
            }
            else
            {
                secondButton.gameObject.SetActive(false);
                if (gatheringCrystals.CheckCrystals())
                {
                    firstButton.gameObject.SetActive(true);
                }
                else
                {
                    firstButton.gameObject.SetActive(false);
                }
            }
            isFreezing = false;
            isMelting = false;
            UpdatePhysics();
        }
    }

    private void SetIceVisibility(float progress)
    {
        iceMaterial.SetFloat("_Progress", progress);
    }

    private void UpdatePhysics()
    {
        iceCollider.enabled = !isWaterActive;
        Debug.Log($"Коллайдер льда: {iceCollider.enabled}");
    }
}
