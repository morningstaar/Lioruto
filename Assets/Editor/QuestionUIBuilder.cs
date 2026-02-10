using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Events;
using TMPro;

// Outil Editor pour générer rapidement la hiérarchie UI et un prefab de bouton de réponse
public static class QuestionUIBuilder
{
    [MenuItem("Tools/VR UI/Create Question UI Prototype")]
    public static void CreateQuestionUI()
    {
        // Remove existing canvas to avoid duplicates
        var existingCanvas = GameObject.Find("QuestionCanvas");
        if (existingCanvas != null)
            Object.DestroyImmediate(existingCanvas);

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

        var defaultFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");

        // Ensure EventSystem exists
        if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Root containers
        var questionRoot = new GameObject("QuestionRoot");
        questionRoot.transform.SetParent(canvasGO.transform, false);

        var homeRoot = new GameObject("HomePanel");
        homeRoot.transform.SetParent(canvasGO.transform, false);
        var homeRT = homeRoot.AddComponent<RectTransform>();
        homeRT.sizeDelta = new Vector2(800, 600);

        var resultRoot = new GameObject("ResultPanel");
        resultRoot.transform.SetParent(canvasGO.transform, false);
        var resultRT = resultRoot.AddComponent<RectTransform>();
        resultRT.sizeDelta = new Vector2(800, 300);

        // Question Panel
        var panel = new GameObject("QuestionPanel");
        panel.transform.SetParent(questionRoot.transform, false);
        var panelRT = panel.AddComponent<RectTransform>();
        panelRT.sizeDelta = new Vector2(800, 300);

        // HUD Panel (Score / Shot / Target)
        var hudPanel = new GameObject("HUDPanel");
        hudPanel.transform.SetParent(questionRoot.transform, false);
        var hudRT = hudPanel.AddComponent<RectTransform>();
        hudRT.anchorMin = new Vector2(0, 1f);
        hudRT.anchorMax = new Vector2(1f, 1f);
        hudRT.pivot = new Vector2(0.5f, 1f);
        hudRT.sizeDelta = new Vector2(800, 80);
        hudRT.anchoredPosition = new Vector2(0, -10);

        // HUD Texts
        var scoreTextGO = new GameObject("ScoreText");
        scoreTextGO.transform.SetParent(hudPanel.transform, false);
        var scoreText = scoreTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) scoreText.font = defaultFont;
        scoreText.color = new Color(1f, 1f, 1f, 1f);
        scoreText.text = "Score: 0";
        scoreText.fontSize = 28;
        scoreText.alignment = TextAlignmentOptions.Left;
        var scoreRT = scoreText.GetComponent<RectTransform>();
        scoreRT.anchorMin = new Vector2(0f, 0f);
        scoreRT.anchorMax = new Vector2(0.33f, 1f);
        scoreRT.offsetMin = new Vector2(10, 0);
        scoreRT.offsetMax = new Vector2(-10, 0);

        var shotTextGO = new GameObject("ShotText");
        shotTextGO.transform.SetParent(hudPanel.transform, false);
        var shotText = shotTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) shotText.font = defaultFont;
        shotText.color = new Color(1f, 1f, 1f, 1f);
        shotText.text = "Tir: 1/5";
        shotText.fontSize = 28;
        shotText.alignment = TextAlignmentOptions.Center;
        var shotRT = shotText.GetComponent<RectTransform>();
        shotRT.anchorMin = new Vector2(0.33f, 0f);
        shotRT.anchorMax = new Vector2(0.66f, 1f);
        shotRT.offsetMin = new Vector2(10, 0);
        shotRT.offsetMax = new Vector2(-10, 0);

        var targetTextGO = new GameObject("TargetText");
        targetTextGO.transform.SetParent(hudPanel.transform, false);
        var targetText = targetTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) targetText.font = defaultFont;
        targetText.color = new Color(1f, 1f, 1f, 1f);
        targetText.text = "Objectif: 1000";
        targetText.fontSize = 28;
        targetText.alignment = TextAlignmentOptions.Right;
        var targetRT = targetText.GetComponent<RectTransform>();
        targetRT.anchorMin = new Vector2(0.66f, 0f);
        targetRT.anchorMax = new Vector2(1f, 1f);
        targetRT.offsetMin = new Vector2(10, 0);
        targetRT.offsetMax = new Vector2(-10, 0);

        // Question Text (TextMeshProUGUI)
        var qTextGO = new GameObject("QuestionText");
        qTextGO.transform.SetParent(panel.transform, false);
        var qText = qTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) qText.font = defaultFont;
        qText.color = new Color(1f, 1f, 1f, 1f);
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
        if (defaultFont != null) tmp.font = defaultFont;
        tmp.color = Color.black;
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
        questionUIGO.transform.SetParent(questionRoot.transform, false);
        var questionUIScript = questionUIGO.AddComponent<QuestionUI>();
        questionUIScript.questionText = qText;
        questionUIScript.answerButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        questionUIScript.answersParent = answersParent.transform;

        // Create HUD script and wire texts
        var hudScript = hudPanel.AddComponent<GameHUD>();
        hudScript.scoreText = scoreText;
        hudScript.shotText = shotText;
        hudScript.targetText = targetText;

        // Home UI (Accueil)
        var homeBg = homeRoot.AddComponent<Image>();
        homeBg.color = new Color(0.05f, 0.1f, 0.15f, 0.85f);

        // After-Play Info Panel (container) under Canvas (sibling of QuestionRoot)
        var infoRoot = new GameObject("InfoPanel");
        infoRoot.transform.SetParent(canvasGO.transform, false);
        var infoRootRT = infoRoot.AddComponent<RectTransform>();
        infoRootRT.sizeDelta = new Vector2(800, 300);

        var infoLeft = new GameObject("InfoPanelLeft");
        infoLeft.transform.SetParent(infoRoot.transform, false);
        var infoLeftRT = infoLeft.AddComponent<RectTransform>();
        infoLeftRT.sizeDelta = new Vector2(360, 180);
        infoLeftRT.anchoredPosition = new Vector2(-200, 140);
        var infoLeftBg = infoLeft.AddComponent<Image>();
        infoLeftBg.color = new Color(0.1f, 0.25f, 0.2f, 0.9f);

        var infoLeftTitleGO = new GameObject("Title");
        infoLeftTitleGO.transform.SetParent(infoLeft.transform, false);
        var infoLeftTitle = infoLeftTitleGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) infoLeftTitle.font = defaultFont;
        infoLeftTitle.color = Color.white;
        infoLeftTitle.text = "Bloquez les ballons !";
        infoLeftTitle.fontSize = 26;
        infoLeftTitle.alignment = TextAlignmentOptions.Left;
        var infoLeftTitleRT = infoLeftTitle.GetComponent<RectTransform>();
        infoLeftTitleRT.anchorMin = new Vector2(0f, 0.6f);
        infoLeftTitleRT.anchorMax = new Vector2(1f, 1f);
        infoLeftTitleRT.offsetMin = new Vector2(10, 5);
        infoLeftTitleRT.offsetMax = new Vector2(-10, -5);

        var infoLeftTextGO = new GameObject("Body");
        infoLeftTextGO.transform.SetParent(infoLeft.transform, false);
        var infoLeftText = infoLeftTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) infoLeftText.font = defaultFont;
        infoLeftText.color = Color.white;
        infoLeftText.text = "Arrête un maximum de tirs avec tes mains.\n\nGagne +400 points par arrêt !";
        infoLeftText.fontSize = 20;
        infoLeftText.alignment = TextAlignmentOptions.TopLeft;
        var infoLeftTextRT = infoLeftText.GetComponent<RectTransform>();
        infoLeftTextRT.anchorMin = new Vector2(0f, 0f);
        infoLeftTextRT.anchorMax = new Vector2(1f, 0.6f);
        infoLeftTextRT.offsetMin = new Vector2(10, 10);
        infoLeftTextRT.offsetMax = new Vector2(-10, -10);

        var infoRight = new GameObject("InfoPanelRight");
        infoRight.transform.SetParent(infoRoot.transform, false);
        var infoRightRT = infoRight.AddComponent<RectTransform>();
        infoRightRT.sizeDelta = new Vector2(360, 180);
        infoRightRT.anchoredPosition = new Vector2(200, 140);
            var infoAlert = new GameObject("InfoAlert");
            infoAlert.transform.SetParent(infoRoot.transform, false);
            var infoAlertRT = infoAlert.AddComponent<RectTransform>();
            infoAlertRT.sizeDelta = new Vector2(760, 60);
            infoAlertRT.anchoredPosition = new Vector2(0, -10);
            var infoAlertBg = infoAlert.AddComponent<Image>();
            infoAlertBg.color = new Color(0.95f, 0.75f, 0.2f, 0.9f);

            var infoAlertTextGO = new GameObject("Text");
            infoAlertTextGO.transform.SetParent(infoAlert.transform, false);
            var infoAlertText = infoAlertTextGO.AddComponent<TextMeshProUGUI>();
            if (defaultFont != null) infoAlertText.font = defaultFont;
            infoAlertText.color = Color.black;
            infoAlertText.text = "Attention ! Vous avez 5 tirs au total et vous devez atteindre 1000 points !";
            infoAlertText.fontSize = 20;
            infoAlertText.alignment = TextAlignmentOptions.Center;
            var infoAlertTextRT = infoAlertText.GetComponent<RectTransform>();
            infoAlertTextRT.anchorMin = Vector2.zero;
            infoAlertTextRT.anchorMax = Vector2.one;
            infoAlertTextRT.offsetMin = Vector2.zero;
            infoAlertTextRT.offsetMax = Vector2.zero;

            var readyBtnGO = new GameObject("ReadyButton");
            readyBtnGO.transform.SetParent(infoRoot.transform, false);
            var readyBtnRT = readyBtnGO.AddComponent<RectTransform>();
            readyBtnRT.sizeDelta = new Vector2(240, 60);
            readyBtnRT.anchoredPosition = new Vector2(0, -90);
            var readyImg = readyBtnGO.AddComponent<Image>();
            readyImg.color = new Color(0.2f, 0.7f, 1f, 1f);
            var readyButton = readyBtnGO.AddComponent<Button>();

            var readyTextGO = new GameObject("Text");
            readyTextGO.transform.SetParent(readyBtnGO.transform, false);
            var readyText = readyTextGO.AddComponent<TextMeshProUGUI>();
            if (defaultFont != null) readyText.font = defaultFont;
            readyText.color = Color.white;
            readyText.text = "PRÊT !";
            readyText.fontSize = 28;
            readyText.alignment = TextAlignmentOptions.Center;
            var readyTextRT = readyText.GetComponent<RectTransform>();
            readyTextRT.anchorMin = Vector2.zero;
            readyTextRT.anchorMax = Vector2.one;
            readyTextRT.offsetMin = Vector2.zero;
            readyTextRT.offsetMax = Vector2.zero;
        var infoRightBg = infoRight.AddComponent<Image>();
        infoRightBg.color = new Color(0.12f, 0.18f, 0.35f, 0.9f);

        var infoRightTitleGO = new GameObject("Title");
        infoRightTitleGO.transform.SetParent(infoRight.transform, false);
        var infoRightTitle = infoRightTitleGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) infoRightTitle.font = defaultFont;
        infoRightTitle.color = Color.white;
        infoRightTitle.text = "Répondez aux questions de foot !";
        infoRightTitle.fontSize = 24;
        infoRightTitle.alignment = TextAlignmentOptions.Left;
        var infoRightTitleRT = infoRightTitle.GetComponent<RectTransform>();
        infoRightTitleRT.anchorMin = new Vector2(0f, 0.6f);
        infoRightTitleRT.anchorMax = new Vector2(1f, 1f);
        infoRightTitleRT.offsetMin = new Vector2(10, 5);
        infoRightTitleRT.offsetMax = new Vector2(-10, -5);

        var infoRightTextGO = new GameObject("Body");
        infoRightTextGO.transform.SetParent(infoRight.transform, false);
        var infoRightText = infoRightTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) infoRightText.font = defaultFont;
        infoRightText.color = Color.white;
        infoRightText.text = "Réponds correctement après chaque tir pour gagner des bonus.\n\nTir arrêté + bonne réponse : x2\nTir manqué + bonne réponse : +20";
        infoRightText.fontSize = 18;
        infoRightText.alignment = TextAlignmentOptions.TopLeft;
        var infoRightTextRT = infoRightText.GetComponent<RectTransform>();
        infoRightTextRT.anchorMin = new Vector2(0f, 0f);
        infoRightTextRT.anchorMax = new Vector2(1f, 0.6f);
        infoRightTextRT.offsetMin = new Vector2(10, 10);
        infoRightTextRT.offsetMax = new Vector2(-10, -10);

        var homeBgImageGO = new GameObject("HomeBackgroundImage");
        homeBgImageGO.transform.SetParent(homeRoot.transform, false);
        var homeBgImage = homeBgImageGO.AddComponent<Image>();
        homeBgImage.color = new Color(1f, 1f, 1f, 0f);
        homeBgImage.preserveAspect = true;
        var homeBgImageRT = homeBgImage.GetComponent<RectTransform>();
        homeBgImageRT.anchorMin = Vector2.zero;
        homeBgImageRT.anchorMax = Vector2.one;
        homeBgImageRT.offsetMin = Vector2.zero;
        homeBgImageRT.offsetMax = Vector2.zero;

        var homeTitleGO = new GameObject("HomeTitle");
        homeTitleGO.transform.SetParent(homeRoot.transform, false);
        var homeTitle = homeTitleGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) homeTitle.font = defaultFont;
        homeTitle.color = new Color(1f, 1f, 1f, 1f);
        homeTitle.text = "RÈGLES DU JEU";
        homeTitle.fontSize = 56;
        homeTitle.alignment = TextAlignmentOptions.Center;
        var homeTitleRT = homeTitle.GetComponent<RectTransform>();
        homeTitleRT.anchorMin = new Vector2(0f, 0.75f);
        homeTitleRT.anchorMax = new Vector2(1f, 1f);
        homeTitleRT.offsetMin = new Vector2(10, 10);
        homeTitleRT.offsetMax = new Vector2(-10, -10);

        var rulesGO = new GameObject("RulesText");
        rulesGO.transform.SetParent(homeRoot.transform, false);
        var rulesText = rulesGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) rulesText.font = defaultFont;
        rulesText.color = new Color(0.95f, 0.98f, 1f, 1f);
        rulesText.text = "• Arrête un maximum de tirs au but, deviens ballons avec tes mains.\n\n" +
                 "• Après chaque tir, réponds correctement aux questions de culture foot.\n\n" +
                 "• Atteins 1000 points en 5 tirs pour gagner.\n\n" +
                 "• +400 points par arrêt parfait.\n\n" +
                 "• Bonne réponse après un arrêt = points doublés.";
        rulesText.fontSize = 28;
        rulesText.alignment = TextAlignmentOptions.TopLeft;
        var rulesRT = rulesText.GetComponent<RectTransform>();
        rulesRT.anchorMin = new Vector2(0.05f, 0.25f);
        rulesRT.anchorMax = new Vector2(0.95f, 0.75f);
        rulesRT.offsetMin = new Vector2(10, 10);
        rulesRT.offsetMax = new Vector2(-10, -10);

        var playBtnGO = new GameObject("PlayButton");
        playBtnGO.transform.SetParent(homeRoot.transform, false);
        var playBtnRT = playBtnGO.AddComponent<RectTransform>();
        playBtnRT.sizeDelta = new Vector2(320, 70);
        playBtnRT.anchorMin = new Vector2(0.5f, 0.05f);
        playBtnRT.anchorMax = new Vector2(0.5f, 0.05f);
        playBtnRT.anchoredPosition = new Vector2(0, 10);
        var playImage = playBtnGO.AddComponent<Image>();
        playImage.color = new Color(0.2f, 0.8f, 0.3f, 1f);
        var playButton = playBtnGO.AddComponent<Button>();

        var playTextGO = new GameObject("Text");
        playTextGO.transform.SetParent(playBtnGO.transform, false);
        var playText = playTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) playText.font = defaultFont;
        playText.color = Color.white;
        playText.text = "JOUER";
        playText.fontSize = 32;
        playText.alignment = TextAlignmentOptions.Center;
        var playTextRT = playText.GetComponent<RectTransform>();
        playTextRT.anchorMin = Vector2.zero;
        playTextRT.anchorMax = Vector2.one;
        playTextRT.offsetMin = Vector2.zero;
        playTextRT.offsetMax = Vector2.zero;

        // Result UI
        var resultBg = resultRoot.AddComponent<Image>();
        resultBg.color = new Color(0f, 0f, 0f, 0.75f);

        var resultTextGO = new GameObject("ResultText");
        resultTextGO.transform.SetParent(resultRoot.transform, false);
        var resultText = resultTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) resultText.font = defaultFont;
        resultText.color = Color.white;
        resultText.text = "RESULTAT";
        resultText.fontSize = 48;
        resultText.alignment = TextAlignmentOptions.Center;
        var resultTextRT = resultText.GetComponent<RectTransform>();
        resultTextRT.anchorMin = new Vector2(0f, 0.4f);
        resultTextRT.anchorMax = new Vector2(1f, 1f);
        resultTextRT.offsetMin = new Vector2(10, 10);
        resultTextRT.offsetMax = new Vector2(-10, -10);

        var replayBtnGO = new GameObject("ReplayButton");
        replayBtnGO.transform.SetParent(resultRoot.transform, false);
        var replayBtnRT = replayBtnGO.AddComponent<RectTransform>();
        replayBtnRT.sizeDelta = new Vector2(260, 60);
        replayBtnRT.anchorMin = new Vector2(0.5f, 0.1f);
        replayBtnRT.anchorMax = new Vector2(0.5f, 0.1f);
        replayBtnRT.anchoredPosition = new Vector2(0, 10);
        var replayImage = replayBtnGO.AddComponent<Image>();
        replayImage.color = new Color(0.2f, 0.7f, 1f, 1f);
        var replayButton = replayBtnGO.AddComponent<Button>();

        var replayTextGO = new GameObject("Text");
        replayTextGO.transform.SetParent(replayBtnGO.transform, false);
        var replayText = replayTextGO.AddComponent<TextMeshProUGUI>();
        if (defaultFont != null) replayText.font = defaultFont;
        replayText.color = Color.white;
        replayText.text = "REJOUER";
        replayText.fontSize = 28;
        replayText.alignment = TextAlignmentOptions.Center;
        var replayTextRT = replayText.GetComponent<RectTransform>();
        replayTextRT.anchorMin = Vector2.zero;
        replayTextRT.anchorMax = Vector2.one;
        replayTextRT.offsetMin = Vector2.zero;
        replayTextRT.offsetMax = Vector2.zero;

        // Link existing QuestionManager if present
        var qm = Object.FindObjectOfType<QuestionManager>();
        if (qm != null) questionUIScript.questionManager = qm;

        // UI Flow Controller
        var flow = canvasGO.AddComponent<UIFlowController>();
        flow.homePanel = homeRoot;
        flow.questionPanel = questionRoot;
        flow.resultPanel = resultRoot;
        flow.infoPanel = infoRoot;
        flow.resultText = resultText;
        flow.questionUI = questionUIScript;

        // Link existing GameManager if present
        var gm = Object.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            hudScript.gameManager = gm;
            gm.gameHUD = hudScript;
            flow.gameManager = gm;
            gm.uiFlow = flow;
        }

        UnityEventTools.AddPersistentListener(playButton.onClick, flow.StartGame);
        UnityEventTools.AddPersistentListener(replayButton.onClick, flow.StartGame);
        UnityEventTools.AddPersistentListener(readyButton.onClick, flow.ShowQuestions);

        // Start with Home visible
        questionRoot.SetActive(false);
        resultRoot.SetActive(false);
        infoRoot.SetActive(false);

        // Focus selection in editor
        Selection.activeGameObject = canvasGO;
        EditorUtility.DisplayDialog("Question UI", "Prototype créé. Prefab: " + prefabPath, "OK");
    }
}
