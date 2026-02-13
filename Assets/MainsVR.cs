using UnityEngine;

public class MainsVR : MonoBehaviour
{
    public TirMascotte scriptMascotte;
    public GameObject effetConfettis;
    public GameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ballon"))
        {
            Debug.Log("ARRÊT !");

            if (gameManager != null && !gameManager.TryResolveShot(true))
                return;

            if (effetConfettis != null)
            {
                ContactPoint contact = collision.contacts[0];
                Instantiate(effetConfettis, contact.point, Quaternion.identity);
            }
        }
    }
}