using UnityEngine;  

public class UIManager1 : MonoBehaviour  
{  
    public CanvasGroup treasureCanvasGroup; // 拖拽宝箱 UI 的 CanvasGroup  

    public void ShowTreasureUI()  
    {  
        treasureCanvasGroup.alpha = 1; // 设置 alpha 为 1  
        treasureCanvasGroup.interactable = true; // 允许交互  
        treasureCanvasGroup.blocksRaycasts = true; // 允许阻挡射线  
    }  

    public void HideTreasureUI()  
    {  
        treasureCanvasGroup.alpha = 0; // 设置 alpha 为 0  
        treasureCanvasGroup.interactable = false; // 禁止交互  
        treasureCanvasGroup.blocksRaycasts = false; // 禁止阻挡射线  
    }  
}