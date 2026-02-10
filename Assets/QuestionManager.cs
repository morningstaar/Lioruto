using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public string questionText;
    public string[] answers;
    public int correctAnswer;
}

public class QuestionManager : MonoBehaviour
{
    public List<Question> questions = new List<Question>();
    private int currentQuestion = 0;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (questions == null || questions.Count == 0)
            GenerateQuestions();
    }

    void GenerateQuestions()
    {
        if (questions == null)
            questions = new List<Question>();

        // Vider les questions existantes pour éviter les doublons lorsque Initialize/Start sont appelés plusieurs fois
        questions.Clear();

        questions.Add(new Question()
        {
            questionText = "Qui a gagné la Coupe du Monde 2018 ?",
            answers = new string[] { "Allemagne", "Argentine", "Brésil", "France" },
            correctAnswer = 3
        });

        questions.Add(new Question()
        {
            questionText = "C'est possible de sanctionner le coach d'un carton rouge ou jaune ?",
            answers = new string[] { "Oui", "Non", "Peut-être", "Inconnu" },
            correctAnswer = 1
        });

        questions.Add(new Question()
        {
            questionText = "Combien de joueurs dans une équipe ?",
            answers = new string[] { "9", "10", "11", "12" },
            correctAnswer = 2
        });

        questions.Add(new Question()
        {
            questionText = "Qui a gagné la Coupe du Monde 2014 ?",
            answers = new string[] { "Allemagne", "Argentine", "Brésil", "France" },
            correctAnswer = 0
        });

        questions.Add(new Question()
        {
            questionText = "Qui a gagné le ballon d'or 2025 ?",
            answers = new string[] { "Mbappe", "Rodri", "Dembele", "Modric" },
            correctAnswer = 0
        });

        questions.Add(new Question()
        {
            questionText = "Quel pays a remporté la coupe du monde 2022 ?",
            answers = new string[] { "Argentine", "France", "Brésil", "Maroc" },
            correctAnswer = 0
        });
    }

    public void LogCurrentQuestion()
    {
        var q = GetCurrentQuestion();
        if (q == null)
        {
            Debug.Log("No current question to display.");
            return;
        }

        Debug.Log($"Question: {q.questionText}");
        for (int i = 0; i < q.answers.Length; i++)
        {
            Debug.Log($"{i}: {q.answers[i]}");
        }
    }

    public Question GetCurrentQuestion()
    {
        if (questions == null || questions.Count == 0)
            return null;

        if (currentQuestion < 0 || currentQuestion >= questions.Count)
            currentQuestion = 0;

        return questions[currentQuestion];
    }

    public bool CheckAnswer(int index)
    {
        var q = GetCurrentQuestion();
        if (q == null)
            return false;

        return index == q.correctAnswer;
    }

    public void NextQuestion()
    {
        currentQuestion++;
        if (currentQuestion >= questions.Count)
            currentQuestion = 0;
    }
}
