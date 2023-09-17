using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    #region Singleton
    public static PostProcessingManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [SerializeField] private float focusCAS = -40, cASChangeSpeed = 4f;
    private float normalCAS, desirableCAS;

    [SerializeField] private Volume volume;
    private ColorAdjustments cA;

    private float GetDeltaTime() { return 1.0f / Time.unscaledDeltaTime * Time.deltaTime; }

    private void Start()
    {
        volume.profile.TryGet(out cA);

        normalCAS = cA.saturation.value;

        desirableCAS = normalCAS;
    }

    private void Update()
    {
        if (cA.saturation.value != desirableCAS)
        {
            cA.saturation.value =
                Mathf.MoveTowards(cA.saturation.value, desirableCAS, cASChangeSpeed * GetDeltaTime());
        }
    }

    public void SetColorAdjustmentsSaturation(bool isFocusing)
    {
        desirableCAS = isFocusing ? focusCAS : normalCAS;
    }
}
