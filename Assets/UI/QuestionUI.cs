using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Prototype d'interface questions en VR (Canvas en World Space)
public class QuestionUI : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public GameObject answerButtonPrefab; // prefab contenant un Button + TextMeshProUGUI
    public Transform answersParent;
    public QuestionManager questionManager;

    private List<GameObject> spawnedButtons = new List<GameObject>();

    public void DisplayCurrentQuestion()
    {
        if (questionManager == null)
        {
            Debug.LogWarning("QuestionManager non assigné dans QuestionUI.");
            return;
        }

        var q = questionManager.GetCurrentQuestion();
        if (q == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        questionText.text = q.questionText;

        // clear old buttons
        foreach (var b in spawnedButtons)
            Destroy(b);
        spawnedButtons.Clear();

        for (int i = 0; i < q.answers.Length; i++)
        {
            var go = Instantiate(answerButtonPrefab, answersParent);
            spawnedButtons.Add(go);
            var btn = go.GetComponent<Button>();
            var txt = go.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null) txt.text = q.answers[i];

            int idx = i; // capture
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnAnswerClicked(idx));
            }
        }
    }

    public void OnAnswerClicked(int index)
    {
        bool correct = questionManager.CheckAnswer(index);
        Debug.Log($"Réponse sélectionnée: {index} - correcte: {correct}");

        // Dans le prototype, on notifie le GameManager si présent (utile pour tests)
        var gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            // Ici on suppose que le shot a été arrêté pour la sélection manuelle en prototype.
            gm.ProcessShotResult(true, correct);
        }
    }
}
