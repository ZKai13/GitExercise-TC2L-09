using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    [SerializeField] private int melonCountToCollect = 3;
    private int Melon = 0;
    public GameObject box2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Melon"))
        {
            Destroy(collision.gameObject);
            Melon++;
            Debug.Log("Collected Melon: " + Melon);

            if (Melon >= melonCountToCollect)  
            {   
                HandleThreeMelonsCollected();  
            }      
        }
    }

    private void HandleThreeMelonsCollected()  
    {  
       Debug.Log("Collected three melons!");  
       Destroy(box2);
    }  
}