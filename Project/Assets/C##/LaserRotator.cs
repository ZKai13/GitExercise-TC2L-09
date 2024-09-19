using UnityEngine;  

public class LaserRotator : MonoBehaviour  
{  
    public GameObject Evil_Wizard_Boss;  
    public float rotationSpeed = 30f;  
    public float laserDistance = 2f; // Adjust this to change the distance of lasers from the center  

    private void Update()  
    {  
        if (Evil_Wizard_Boss != null)  
        {  
            // Update position to match the boss  
            transform.position = Evil_Wizard_Boss.transform.position;  

            // Rotate the Lasers Group around its center  
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);  

            // Update positions of individual laser beams  
            for (int i = 0; i < 4; i++)  
            {  
                float angle = i * 90f + transform.eulerAngles.z;  
                Vector3 position = new Vector3(  
                    Mathf.Cos(angle * Mathf.Deg2Rad),  
                    Mathf.Sin(angle * Mathf.Deg2Rad),  
                    0  
                ) * laserDistance;  

                Transform laser = transform.GetChild(i);  
                laser.localPosition = position;  

                // Adjust the rotation of each laser beam  
                if (i == 1 || i == 3) // Laser 2 (right) and Laser 4 (left)  
                {  
                    laser.rotation = Quaternion.Euler(0, 0, angle + 180f);  
                }  
                else // Laser 1 (top) and Laser 3 (bottom)  
                {  
                    laser.rotation = Quaternion.Euler(0, 0, angle);  
                }  
            }  
        }  
    }  
}