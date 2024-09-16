using UnityEngine;  
using UnityEngine.UI;  

public class EquipButtonHandler1 : MonoBehaviour  
{  
    public Button equipButton;
    public PlayerCombat player; 
    public int damageIncreaseAmount = 10;
    public int heavyAttackDamage = 10;
    private UIManager1 uiManager; 

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager1>();
    }
    public void Equip()  
    {   
        OnEquipButtonClicked();
        uiManager.HideTreasureUI();
        Time.timeScale = 1; 
        
    } 

    private void OnEquipButtonClicked()  
    {  
        Debug.Log("Equip Now button clicked!");  
        EquipItem();  
    }  

    private void EquipItem()  
    {  

        if (player != null)  
        {  
            player.IncreaseLightAttackDamage(damageIncreaseAmount);
            player.IncreaseHeavyAttackDamage(heavyAttackDamage);
        }  
    }  
}