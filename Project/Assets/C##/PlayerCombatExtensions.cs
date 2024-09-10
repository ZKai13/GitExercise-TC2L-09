using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerCombatExtensions  
{  
    public static void OnEnemyAttack(this PlayerCombat playerCombat, float damage)  
    {  
        // Add the logic to handle the enemy's attack here  
        // For example, you could reduce the player's health or trigger a block/dodge animation  
        playerCombat.healthSystem.Takedamage((int)damage);  
    }  
}
