using UnityEngine;
using TMPro;

public class GestionQuiz : MonoBehaviour
{
    public TirMascotte scriptTir; 
    public GameObject canvasQuiz;
    public Rigidbody rbBallon; // AJOUT : Glisse le Soccer Ball ici dans l'inspecteur

    public void ReponseCorrecte()
    {
        // Si le ballon ne bouge plus (vitesse proche de 0), c'est un arrêt !
        bool arretReussi = (rbBallon.velocity.magnitude < 0.1f);

        if (arretReussi) {
            scriptTir.AjouterPoints(400); // Double les points de l'arrêt (400+400=800)
            Debug.Log("Combo Arrêt + Bonne réponse !");
        } else {
            scriptTir.AjouterPoints(20); // Juste 20 points si le tir est encaissé
            Debug.Log("But encaissé mais bonne réponse !");
        }
        FermerQuiz();
    }

    public void ReponseFausse()
    {
        Debug.Log("Mauvaise réponse : 0 point.");
        FermerQuiz();
    }

    private void FermerQuiz()
    {
        canvasQuiz.SetActive(false);
        
        // On libère le ballon pour le prochain tir
        if (rbBallon != null) {
            rbBallon.isKinematic = false;
        }

        if (scriptTir != null) scriptTir.ReplacerBallon();
    }
}