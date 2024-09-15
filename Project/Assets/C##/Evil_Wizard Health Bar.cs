
using UnityEngine;
using UnityEngine.UI;
using System;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Text healthText;

    private int currentHealth;

    public event Action<float> OnHealthPercentChanged;

    public int CurrentHealth
    {
        get => currentHealth;
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            UpdateHealthBar();
        }
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = (float)CurrentHealth / maxHealth;
        healthBarImage.fillAmount = healthPercentage;
        healthText.text = $"{CurrentHealth} / {maxHealth}";
        
        OnHealthPercentChanged?.Invoke(healthPercentage);
    }
}
