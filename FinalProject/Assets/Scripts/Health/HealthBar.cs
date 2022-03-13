using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private Image fillArea;

    [SerializeField]
    private TextMeshProUGUI healthText;

    private Slider healthBarSlider = null;

    


    // Start is called before the first frame update
    void Awake()
    {
        healthBarSlider = GetComponent<Slider>();
        fillArea = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
    }

    public void SetMaxHealth(float maxHealth)
    {
        healthBarSlider.maxValue = maxHealth;

        if(healthText != null)
        {
            healthText.text = $"{(int)healthBarSlider.value}/{healthBarSlider.maxValue}";
        }
    }

    public void Reset(float maxHealth)
    {
        SetMaxHealth(maxHealth);
        SetHealth(maxHealth);
    }

    public void SetHealth(float health)
    {

        healthBarSlider.value = health;

        // Get the percentage of health left
        float healthPercentage = healthBarSlider.value / healthBarSlider.maxValue;

        Color color = gradient.Evaluate(healthPercentage);

        // Change color of fill area of health bar to the appropriate gradient color based off of the health percentage left
        fillArea.color = color;

        if (healthText != null)
        {
            healthText.text = $"{(int)healthBarSlider.value}/{healthBarSlider.maxValue}";
        }
    }

    public float GetMaxHealth()
    {
        return healthBarSlider.maxValue;
    }

    public float GetHealth()
    {
        return healthBarSlider.value;
    }
}
