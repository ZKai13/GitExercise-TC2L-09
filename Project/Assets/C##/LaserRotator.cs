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
            // 将激光组的位置更新为Boss的位置   
            transform.position = Evil_Wizard_Boss.transform.position;  

            // 围绕自身中心旋转激光组  
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);  

            // 更新每个激光射线的位置和角度  
            for (int i = 0; i < 4; i++)  
            {  
                // 计算每个激光射线的位置 
                float angle = i * 90f + transform.eulerAngles.z;  
                Vector3 position = new Vector3(  
                    Mathf.Cos(angle * Mathf.Deg2Rad),  
                    Mathf.Sin(angle * Mathf.Deg2Rad),  
                    0  
                ) * laserDistance;  
                // 获取每个激光射线的Transform 

                Transform laser = transform.GetChild(i);  
                laser.localPosition = position;  

                // 根据激光射线的位置调整其旋转角度  
                if (i == 1 || i == 3) // 激光2(右)和激光4(左)  
                {  
                    laser.rotation = Quaternion.Euler(0, 0, angle + 180f);  
                }  
                else // 激光1(上)和激光3(下)    
                {  
                    laser.rotation = Quaternion.Euler(0, 0, angle);  
                }  
            }  
        }  
    }  
}