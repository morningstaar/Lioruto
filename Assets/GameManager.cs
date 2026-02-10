using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Rules")]
    public int score = 0;
    public int currentShot = 0;
    public int maxShots = 5;
    public int targetScore = 1000;

    [Header("References")]
    public QuestionManager questionManager;
    public QuestionUI questionUI;
    public GameHUD gameHUD;
    public UIFlowController uiFlow;

    void Start()
    {
        if (questionManager == null)
        {
            questionManager = FindObjectOfType<QuestionManager>();
            if (questionManager == null)
                Debug.LogWarning("No QuestionManager found in scene. Assign one to GameManager.");
            else
                questionManager.LogCurrentQuestion();
        }

        if (questionUI == null)
        {
            questionUI = FindObjectOfType<QuestionUI>();
            if (questionUI != null && questionManager != null)
                questionUI.questionManager = questionManager;
        }

        if (gameHUD == null)
            gameHUD = FindObjectOfType<GameHUD>();

        if (gameHUD != null)
            gameHUD.Refresh();

        if (uiFlow == null)
            uiFlow = FindObjectOfType<UIFlowController>();
    }

    // Appelé après chaque tir + réponse
    public void ProcessShotResult(bool stoppedShot, bool correctAnswer)
    {
        // CAS 1 : Tir arrêté
        if (stoppedShot)
        {
            int stopPoints = 400;
            if (correctAnswer)
            {
                // Doubler uniquement les points gagnés pour cet arrêt
                score += stopPoints * 2;
            }
            else
            {
                score += stopPoints;
            }
        }
        // CAS 2 : But
        else
        {
            if (correctAnswer)
            {
                score += 20;
            }
        }

        Debug.Log("Score actuel : " + score);

        if (gameHUD != null)
            gameHUD.Refresh();

        NextShot();
    }

    void NextShot()
    {
        currentShot++;

        if (currentShot >= maxShots)
        {
            EndGame();
        }
        else
        {
            if (questionManager != null)
            {
                questionManager.NextQuestion();
                questionManager.LogCurrentQuestion();
                if (questionUI != null)
                    questionUI.DisplayCurrentQuestion();
            }
            else
            {
                Debug.LogWarning("QuestionManager reference is missing. Cannot advance or display next question.");
            }
        }

        if (gameHUD != null)
            gameHUD.Refresh();
    }

    void EndGame()
    {
        bool victory = score >= targetScore;
        if (score >= targetScore)
        {
            Debug.Log("VICTOIRE !");
        }
        else
        {
            Debug.Log("DEFAITE !");
        }

        if (uiFlow != null)
            uiFlow.ShowResults(victory);
    }

    public void ResetGame()
    {
        score = 0;
        currentShot = 0;

        if (questionManager != null)
            questionManager.Initialize();

        if (gameHUD != null)
            gameHUD.Refresh();
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        Debug.Log("Test tir arrêté + bonne réponse");

        ProcessShotResult(true, true);
    }
}

}
