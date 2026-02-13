using UnityEngine;

public class DetecteurBut : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie que c'est bien le ballon qui entre
        if (other.CompareTag("Ballon"))
        {
            Debug.Log("BUT ! Affichage du Quiz...");
            if (gameManager != null)
                gameManager.TryResolveShot(false);
        }
    }
}