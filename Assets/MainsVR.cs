using UnityEngine;

public class MainsVR : MonoBehaviour
{
    public TirMascotte scriptTir; 
    public GameObject canvasQuiz; // Case à remplir dans l'inspecteur

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ballon"))
        {
            // 1. On affiche l'interface du Quiz
            if (canvasQuiz != null)
            {
                canvasQuiz.SetActive(true);
            }

            // 2. On ajoute les points
            if (scriptTir != null)
            {
                scriptTir.AjouterPoints(400);
            }

            // 3. On arrête le ballon pour laisser le joueur lire
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true; // Empêche le ballon de bouger pendant le quiz
            }
        }
    }
}