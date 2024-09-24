using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone1 : MonoBehaviour
{
    public int damage = 10; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Takedamage(damage, false); 
        }

        
        Mushroom mushroom = other.GetComponent<Mushroom>();
        if (mushroom != null)
        {
            mushroom.Takedamage(damage, false);
            
        }
    }
}
