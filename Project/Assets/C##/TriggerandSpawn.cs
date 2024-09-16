using UnityEngine;

public class TriggerandSpawn : MonoBehaviour
{

    public GameObject objectToSpawn;

    public Transform spawnLocation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(objectToSpawn, spawnLocation.position, spawnLocation.rotation);
        }
    }
}
