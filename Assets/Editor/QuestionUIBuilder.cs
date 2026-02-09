using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

// Outil Editor pour générer rapidement la hiérarchie UI et un prefab de bouton de réponse
public static class QuestionUIBuilder
{
    [MenuItem("Tools/VR UI/Create Question UI Prototype")]
    public static void CreateQuestionUI()
    {
        // create folders
        Directory.CreateDirectory("Assets/Prefabs");
        Directory.CreateDirectory("Assets/UI");

        // Create Canvas (World Space)
        var canvasGO = new GameObject("QuestionCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var cr = canvasGO.GetComponent<RectTransform>();
        cr.sizeDelta = new Vector2(800, 600);
        canvasGO.transform.position = new Vector3(0, 1.6f, 2f);
        canvasGO.transform.rotation = Quaternion.identity;
        canvasGO.transform.localScale = Vector3.one * 0.0025f;

        // Ensure EventSystem exists
        if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Panel
        var panel = new GameObject("QuestionPanel");
        panel.transform.SetParent(canvasGO.transform, false);
        var panelRT = panel.AddComponent<RectTransform>();
        panelRT.sizeDelta = new Vector2(800, 300);

        // Question Text (TextMeshProUGUI)
        var qTextGO = new GameObject("QuestionText");
        qTextGO.transform.SetParent(panel.transform, false);
        var qText = qTextGO.AddComponent<TextMeshProUGUI>();
        qText.text = "Question exemple";
        qText.fontSize = 36;
        var qtr = qText.GetComponent<RectTransform>();
        qtr.anchorMin = new Vector2(0, 0.6f);
        qtr.anchorMax = new Vector2(1, 1f);
        qtr.offsetMin = new Vector2(10, 10);
        qtr.offsetMax = new Vector2(-10, -10);

        // Answers parent (Grid area)
        var answersParent = new GameObject("AnswersGrid");
        answersParent.transform.SetParent(panel.transform, false);
        var aprt = answersParent.AddComponent<RectTransform>();
        aprt.anchorMin = new Vector2(0, 0f);
        aprt.anchorMax = new Vector2(1, 0.6f);
        aprt.offsetMin = new Vector2(10, 10);
        aprt.offsetMax = new Vector2(-10, -10);

        // Create a temporary AnswerButton GameObject to make a prefab
        var buttonGO = new GameObject("AnswerButton");
        var brt = buttonGO.AddComponent<RectTransform>();
        brt.sizeDelta = new Vector2(760, 50);
        var img = buttonGO.AddComponent<Image>();
        img.color = Color.white;
        var btn = buttonGO.AddComponent<Button>();
        buttonGO.AddComponent<AnswerButton>();

        // Text child
        var textGO = new GameObject("Text");
        textGO.transform.SetParent(buttonGO.transform, false);
        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = "Réponse";
        tmp.alignment = TextAlignmentOptions.Center;
        var ttr = tmp.GetComponent<RectTransform>();
        ttr.anchorMin = Vector2.zero; ttr.anchorMax = Vector2.one;
        ttr.offsetMin = Vector2.zero; ttr.offsetMax = Vector2.zero;

        // Save prefab
        string prefabPath = "Assets/Prefabs/AnswerButton.prefab";
        PrefabUtility.SaveAsPrefabAsset(buttonGO, prefabPath, out bool success);

        // destroy temp button from scene
        Object.DestroyImmediate(buttonGO);

        // Create QuestionUI object
        var questionUIGO = new GameObject("QuestionUI");
        questionUIGO.transform.SetParent(canvasGO.transform, false);
        var questionUIScript = questionUIGO.AddComponent<QuestionUI>();
        questionUIScript.questionText = qText;
        questionUIScript.answerButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        questionUIScript.answersParent = answersParent.transform;

        // Link existing QuestionManager if present
        var qm = Object.FindObjectOfType<QuestionManager>();
        if (qm != null) questionUIScript.questionManager = qm;

        // Focus selection in editor
        Selection.activeGameObject = canvasGO;
        EditorUtility.DisplayDialog("Question UI", "Prototype créé. Prefab: " + prefabPath, "OK");
    }
}
