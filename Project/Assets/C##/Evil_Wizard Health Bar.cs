
using UnityEngine;
using UnityEngine.UI;
using System;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private EvilWizardBoss WizardHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    
    private void Start()
    {
        totalhealthBar.fillAmount = WizardHealth.currentHealth /100;
    }
    private void Update()
    {
       currenthealthBar.fillAmount = WizardHealth.currentHealth /100;
    }
}
