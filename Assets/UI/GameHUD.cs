using UnityEngine;
using TMPro;

// Affiche score, tir actuel et objectif
public class GameHUD : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI shotText;
    public TextMeshProUGUI targetText;
    public GameManager gameManager;

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        Refresh();
    }

    public void Refresh()
    {
        if (gameManager == null)
            return;

        if (scoreText != null)
            scoreText.text = $"Score: {gameManager.score}";

        if (shotText != null)
            shotText.text = $"Tir: {gameManager.currentShot + 1}/{gameManager.maxShots}";

        if (targetText != null)
            targetText.text = $"Objectif: {gameManager.targetScore}";
    }
}
