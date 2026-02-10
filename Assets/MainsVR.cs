using UnityEngine;

public class MainsVR : MonoBehaviour
{
    public TirMascotte scriptMascotte;
    public GameObject effetConfettis; 

    private void OnCollisionEnter(Collision collision)
    {
        // Détection de la balle par les mains VR
        if (collision.gameObject.CompareTag("Ballon"))
        {
            Debug.Log("ARRÊT !");
            
            if (scriptMascotte != null) 
                scriptMascotte.AjouterPoints(400); // +400 pour l'arrêt

            if (effetConfettis != null)
            {
                ContactPoint contact = collision.contacts[0];
                Instantiate(effetConfettis, contact.point, Quaternion.identity);
            }

            scriptMascotte.Invoke("ReplacerBallon", 1.0f);
        }
    }
}