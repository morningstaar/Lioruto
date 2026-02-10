using UnityEngine;
using TMPro;

// Contrôle l'affichage des écrans Accueil / Questions / Résultat
public class UIFlowController : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject questionPanel;
    public GameObject resultPanel;
    public GameObject infoPanel;
    public TextMeshProUGUI resultText;

    public QuestionUI questionUI;
    public GameManager gameManager;

    void Start()
    {
        ShowHome();
    }

    public void StartGame()
    {
        if (gameManager != null)
            gameManager.ResetGame();

        if (homePanel != null) homePanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);
        if (questionPanel != null) questionPanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(true);
    }

    public void ShowQuestions()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
        if (questionPanel != null) questionPanel.SetActive(true);

        if (questionUI != null)
            questionUI.DisplayCurrentQuestion();
    }

    public void ShowHome()
    {
        if (homePanel != null) homePanel.SetActive(true);
        if (questionPanel != null) questionPanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);
    }

    public void ShowResults(bool victory)
    {
        if (resultPanel != null) resultPanel.SetActive(true);
        if (homePanel != null) homePanel.SetActive(false);
        if (questionPanel != null) questionPanel.SetActive(false);

        if (resultText != null)
            resultText.text = victory ? "VICTOIRE !" : "DEFAITE !";
    }
}
