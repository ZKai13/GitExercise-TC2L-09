using System.Collections.Generic;  
using UnityEngine;  

public class MagicBallPool : MonoBehaviour  
{  

    public GameObject magicBallPrefab; // 魔法球预制体 
    public int poolSize = 10; // 对象池的大小   

    private Queue<GameObject> magicBallPool = new Queue<GameObject>();  // 用于存储和管理魔法球的队列  

    private void Start()  
    {  
        // 初始化对象池   
        for (int i = 0; i < poolSize; i++)  
        {  
            GameObject magicBall = Instantiate(magicBallPrefab);  
            magicBall.SetActive(false); // 将魔法球设为不活跃状态    
            magicBallPool.Enqueue(magicBall);  // 将魔法球添加到对象池   
        }  
    }  

    public GameObject GetMagicBall()  
    {  
        // 从对象池中获取一个可用的魔法球  
        if (magicBallPool.Count > 0)  
        {  
            GameObject magicBall = magicBallPool.Dequeue(); // 从队列中取出一个魔法球 
            magicBall.SetActive(true); // 激活该魔法球   
            return magicBall;  
        }  
        else  
        {  
            // 如果对象池为空,则实例化一个新的魔法球 
            GameObject magicBall = Instantiate(magicBallPrefab);  
            return magicBall;  
        }  
    }  

    public void ReturnMagicBall(GameObject magicBall)  
    {  
        // 将使用完的魔法球返回到对象池 
        magicBall.SetActive(false); // 将魔法球设为不活跃状态 
        magicBallPool.Enqueue(magicBall); // 将魔法球添加到队列尾部  
    }  
}