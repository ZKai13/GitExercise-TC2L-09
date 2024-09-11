using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public static class PlayerCombatExtensions  
{  
    /// <summary>  
    /// 处理敌人的攻击  
    /// </summary>  
    /// <param name="playerCombat">PlayerCombat 组件</param>  
    /// <param name="damage">受到的伤害</param>  
    public static void OnEnemyAttack(this PlayerCombat playerCombat, float damage)  
    {  
        // 在这里添加处理敌人攻击的逻辑  
        // 例如，可以减少玩家的生命值或触发格挡/闪避动画  
        playerCombat.healthSystem.Takedamage((int)damage);  
    }  
}  