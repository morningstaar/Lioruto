using UnityEngine;

public class MainsVR : MonoBehaviour
{
    public TirMascotte scriptTir; 
    public GameObject canvasQuiz; // Case à remplir dans l'inspecteur

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ballon"))
        {
            Debug.Log("ARRÊT ! Affichage du Quiz...");
            if (canvasQuiz != null) canvasQuiz.SetActive(true);
            
            // On ajoute les 400 points de l'arrêt
            if (scriptTir != null) scriptTir.AjouterPoints(400);
        }
    }
}