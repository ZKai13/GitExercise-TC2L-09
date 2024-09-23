using UnityEngine;

public class DamageZone1 : MonoBehaviour
{
    public int damage = 10; // 每次碰到时造成的伤害

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰到的对象是否是敌人
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Takedamage(damage, false); // 对敌人造成伤害, 第二个参数是是否是重击，这里传 false
            Debug.Log("Enemy entered the damage zone!");
        }
    }
}
