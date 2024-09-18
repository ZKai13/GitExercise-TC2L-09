
using UnityEngine;
using UnityEngine.UI;
using System;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private EvilWizardBoss WizardHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    public Image fillImage;
    
    private void Start()
    {
        totalhealthBar.fillAmount = WizardHealth.currentHealth /100;
    }
    private void Update()
    {
       currenthealthBar.fillAmount = WizardHealth.currentHealth /100;
    }
        public void SetHealth(float healthPercentage)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = healthPercentage;
        }
        else
        {
            Debug.LogError("Cannot update health bar: Fill Image is null!");
        }
    }
}
