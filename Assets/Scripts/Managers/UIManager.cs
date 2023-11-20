using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Manager))]
public class UIManager : MonoBehaviour
{
    [SerializeField] private float alphaChangeSpeed = 0.05f;
    [SerializeField] private float brightAlpha = 80f, paleAlpha = 40f;
    private float desirableAlpha;

    private Color curCrosshairColor;

    [SerializeField] private Slider healthBar, energyBar;
    [SerializeField] private Image focusCrosshair;
    [SerializeField] private TextMeshProUGUI focusInfoText;
    private Manager mngr;

    [SerializeField] private TextMeshProUGUI debugText;
    private int fps, fpsAccumulator;
    private float fpsNextPeriod = 0;
    const float fpsMeasurePeriod = 0.5f;

    private void Awake()
    {
        mngr = GetComponent<Manager>();

        curCrosshairColor = focusCrosshair.color;

        TurnCrosshair(false);

        fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
    }

    public void UpdateFunction()
    {
        if (curCrosshairColor.a != desirableAlpha)
        {
            curCrosshairColor.a = Mathf.MoveTowards(curCrosshairColor.a, desirableAlpha,
                alphaChangeSpeed * mngr.GetUnscaledDeltaTime());
            focusCrosshair.color = curCrosshairColor;
            focusInfoText.color = curCrosshairColor;
        }

        // measure average frames per second
        fpsAccumulator++;
        if (Time.realtimeSinceStartup > fpsNextPeriod)
        {
            fps = (int)(fpsAccumulator / fpsMeasurePeriod);
            fpsAccumulator = 0;
            fpsNextPeriod += fpsMeasurePeriod;
            debugText.SetText($"{fps} FPS");
        }
    }

    public void SetHealthBarMaxValue(float value)
    {
        healthBar.maxValue = value;
        healthBar.value = value;
    }

    public void SetEnergyBarMaxValue(float value)
    {
        energyBar.maxValue = value;
        energyBar.value = value;
    }

    public void SetHealthBarValue(float value) { healthBar.value = value; }

    public void SetEnergyBarValue(float value) { energyBar.value = value; }

    public void TurnCrosshair(bool on) { desirableAlpha = on ? paleAlpha : 0f; }

    public void ChangeCrosshairAlpha(bool bright) { desirableAlpha = bright ? brightAlpha : paleAlpha; }

    public void SetFocusInfoText(string text) { focusInfoText.text = text; }
}
