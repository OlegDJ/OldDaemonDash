using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Manager))]
public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthBar, energyBar;

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

    public void SetHealthBarValue(float value)
    {
        healthBar.value = value;
    }

    public void SetEnergyBarValue(float value)
    {
        energyBar.value = value;
    }
}
