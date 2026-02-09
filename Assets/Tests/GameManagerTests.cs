using NUnit.Framework;
using UnityEngine;

public class GameManagerTests
{
    private GameManager gameManager;

    [SetUp]
    public void Setup()
    {
        GameObject obj = new GameObject("Game+QM");
        gameManager = obj.AddComponent<GameManager>();
        // créer et attacher un QuestionManager afin que les tests couvrent le flux complet sans avertissements
        var qm = obj.AddComponent<QuestionManager>();
        qm.Initialize();
        gameManager.questionManager = qm;

        gameManager.maxShots = 5;
        gameManager.score = 0;
        gameManager.currentShot = 0;
    }

    [Test]
    public void ProcessShotResult_StoppedShotCorrectAnswer_MultipliesScoreBy2()
    {
        gameManager.ProcessShotResult(true, true);
        
        // 400 * 2 = 800
        Assert.AreEqual(800, gameManager.score);
    }

    [Test]
    public void ProcessShotResult_StoppedShotWrongAnswer_Adds400()
    {
        gameManager.ProcessShotResult(true, false);
        
        Assert.AreEqual(400, gameManager.score);
    }

    [Test]
    public void ProcessShotResult_GoalCorrectAnswer_Adds20()
    {
        gameManager.ProcessShotResult(false, true);
        
        Assert.AreEqual(20, gameManager.score);
    }

    [TearDown]
    public void Cleanup()
    {
        Object.Destroy(gameManager.gameObject);
    }
}