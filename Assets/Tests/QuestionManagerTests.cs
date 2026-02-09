// Fichier de test : Assets/Tests/QuestionManagerTests.cs
using NUnit.Framework;
using UnityEngine;

public class QuestionManagerTests
{
    private QuestionManager questionManager;

    [SetUp]
    public void Setup()
    {
        var obj = new GameObject();
        questionManager = obj.AddComponent<QuestionManager>();
        questionManager.Initialize();
    }

    [Test]
    public void CheckAnswer_CorrectIndex_ReturnsTrue()
    {
        var result = questionManager.CheckAnswer(3); // France pour 2018
        Debug.Log($"CheckAnswer_CorrectIndex_ReturnsTrue: {result}");
        Assert.IsTrue(result);
    }

    [Test]
    public void CheckAnswer_WrongIndex_ReturnsFalse()
    {
        var result = questionManager.CheckAnswer(0); // Mauvaise réponse
        Debug.Log($"CheckAnswer_WrongIndex_ReturnsFalse: {result}");
        Assert.IsFalse(result);
    }

    [Test]
    public void NextQuestion_IncrementsCurrent()
    {
        var first = questionManager.GetCurrentQuestion();
        questionManager.NextQuestion();
        var second = questionManager.GetCurrentQuestion();
        Debug.Log($"NextQuestion: first={first.questionText}, second={second.questionText}");
        Assert.AreNotEqual(first, second);
    }
}