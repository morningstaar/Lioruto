using UnityEngine;

public class DetecteurBut : MonoBehaviour
{
    public GameObject canvasQuiz;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie que c'est bien le ballon qui entre
        if (other.CompareTag("Ballon"))
        {
            Debug.Log("BUT ! Affichage du Quiz...");
            if (canvasQuiz != null) canvasQuiz.SetActive(true);
        }
    }
}