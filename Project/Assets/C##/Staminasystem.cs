using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Staminasystem : MonoBehaviour
{
    public Image staminaBar; // Reference to the stamina bar Image UI element
    public float maxStamina = 100f; // Maximum stamina
    public float staminaRegenRate = 5f; // Rate at which stamina regenerates

    private float currentStamina;

    void Start()
    {
        currentStamina = maxStamina; // Initialize stamina to max
        UpdateStaminaBar(); // Update stamina bar UI
    }

    void Update()
    {
        // Regenerate stamina over time
        if (currentStamina < maxStamina)
        {
            currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * Time.deltaTime);
            UpdateStaminaBar(); // Update stamina bar UI
        }
    }

    // Method to reduce stamina
    public bool ConsumeStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            UpdateStaminaBar(); // Update stamina bar UI
            return true; // Return true if stamina was successfully consumed
        }
        else
        {
            return false; // Return false if not enough stamina
        }
    }

    // Update the stamina bar UI
    private void UpdateStaminaBar()
    {
        staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
