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
        Vector3 direction = (cibleBut.position - rbBallon.transform.position).normalized;
        rbBallon.AddForce(direction * 22f, ForceMode.Impulse);
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