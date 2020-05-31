using UnityEngine;

public class PlayerSafeZone : MonoBehaviour
{

    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            gameManager.OnSafeZoneEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.OnSafeZoneExit();
        }
    }
}
