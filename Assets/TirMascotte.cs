using UnityEngine;
using TMPro;

public class TirMascotte : MonoBehaviour
{
    public Rigidbody rbBallon;
    public Transform cibleBut;
    public TextMeshProUGUI texteScore;
    public int scoreTotal = 0;
    private Vector3 positionInitiale;

    void Start()
    {
        positionInitiale = rbBallon.transform.position;
    }

    void Update()
    {
        // On appuie sur T pour tirer sans gêner le simulateur VR
        if (Input.GetKeyDown(KeyCode.T))
        {
            Tirer();
        }
    }

    public void Tirer()
    {
        ReplacerBallon();
        rbBallon.isKinematic = false; // On libère le ballon pour le tir
        Vector3 direction = (cibleBut.position - rbBallon.transform.position).normalized;
        rbBallon.AddForce(direction * 22f, ForceMode.Impulse);

        // On lance un chrono : si au bout de 2s le quiz n'est pas là, on l'affiche
        Invoke("AfficherQuizAutomatique", 2f);
    }

    void AfficherQuizAutomatique()
    {
        GameObject canvas = GameObject.Find("CanvasQuiz");
        if (canvas != null && !canvas.activeSelf) 
        {
            canvas.SetActive(true);
            rbBallon.isKinematic = true; // On fige le ballon s'il n'a pas été arrêté
        }
    }

    public void ReplacerBallon()
    {
        rbBallon.velocity = Vector3.zero;
        rbBallon.transform.position = positionInitiale;
    }

    public void AjouterPoints(int points)
    {
        scoreTotal += points;
        if (texteScore != null)
            texteScore.text = "Score : " + scoreTotal;
    }

    public void ResetScore()
    {
        scoreTotal = 0;
        if (texteScore != null)
            texteScore.text = "Score : " + scoreTotal;
    }
}