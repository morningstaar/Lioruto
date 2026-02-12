using UnityEngine;

public class MainsVR : MonoBehaviour
{
    public TirMascotte scriptMascotte;
    public GameObject effetConfettis; 
    public GameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        // Détection de la balle par les mains VR
        if (collision.gameObject.CompareTag("Ballon"))
        {
            Debug.Log("ARRÊT !");

            if (gameManager != null && !gameManager.TryResolveShot(true))
                return;

            if (scriptMascotte != null) 
                scriptMascotte.AjouterPoints(400); // +400 pour l'arrêt

            if (effetConfettis != null)
            {
                ContactPoint contact = collision.contacts[0];
                Instantiate(effetConfettis, contact.point, Quaternion.identity);
            }

            if (scriptMascotte != null)
                scriptMascotte.Invoke("ReplacerBallon", 1.0f);
        }
    }
}