using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAutoBuilder : MonoBehaviour
{
    [Header("Placement")]
    public float distanceDevantCamera = 2.0f;
    public Vector2 canvasSize = new Vector2(900, 600);
    public float canvasScale = 0.0015f;

    [Header("Style")]
    public Color fondPanel = new Color(0.05f, 0.25f, 0.35f, 0.85f);
    public Color fondHUD = new Color(0f, 0.15f, 0.2f, 0.7f);
    public Color boutonPrincipal = new Color(0.2f, 0.8f, 0.2f, 1f);
    public Color boutonSecondaire = new Color(0.1f, 0.5f, 0.6f, 1f);
    public Color texteBlanc = Color.white;

    GameManager gameManager;
    TirMascotte tirMascotte;

    void Start()
    {
        gameManager = Object.FindAnyObjectByType<GameManager>();
        tirMascotte = Object.FindAnyObjectByType<TirMascotte>();

        var canvas = CreerCanvas("UIRoot");

        var panelRegles1 = CreerPanel(canvas.transform, "Panel_Regles_1", new Vector2(700, 420), Vector2.zero, fondPanel);
        var panelRegles2 = CreerPanel(canvas.transform, "Panel_Regles_2", new Vector2(720, 460), Vector2.zero, fondPanel);
        var panelQuestion = CreerPanel(canvas.transform, "Panel_Question", new Vector2(720, 420), Vector2.zero, fondPanel);
        var panelPause = CreerPanel(canvas.transform, "Panel_Pause", new Vector2(520, 360), Vector2.zero, fondPanel);
        var panelFin = CreerPanel(canvas.transform, "Panel_Fin", new Vector2(520, 300), Vector2.zero, fondPanel);
        var hud = CreerPanel(canvas.transform, "HUD", new Vector2(420, 80), new Vector2(0, 230), fondHUD);

        var texteRegles1 = CreerTexte(panelRegles1.transform, "REGLES DU JEU\n\nArrête un maximum de tirs.\nAprès chaque tir, réponds à une question.\nAtteins 1000 points en 5 tirs.", 28, TextAlignmentOptions.Center, new Vector2(0, 40));
        var boutonJouer = CreerBouton(panelRegles1.transform, "JOUER", new Vector2(200, 50), new Vector2(0, -140), boutonPrincipal);

        var texteRegles2 = CreerTexte(panelRegles2.transform, "Bloque les ballons avec tes mains.\nRéponds correctement pour gagner des points bonus.\n\nArrêt = 400 points\nBonne réponse après arrêt = x2", 26, TextAlignmentOptions.Center, new Vector2(0, 40));
        var boutonPret = CreerBouton(panelRegles2.transform, "PRET !", new Vector2(200, 50), new Vector2(0, -160), boutonPrincipal);

        var texteNumeroQuestion = CreerTexte(panelQuestion.transform, "QUESTION 1", 26, TextAlignmentOptions.Center, new Vector2(0, 150));
        var texteQuestion = CreerTexte(panelQuestion.transform, "Qui a remporté la Coupe du Monde 2018 ?", 26, TextAlignmentOptions.Center, new Vector2(0, 90));

        var boutonsReponses = new Button[4];
        var textesReponses = new TMP_Text[4];
        var fondsReponses = new Image[4];

        for (int i = 0; i < 4; i++)
        {
            var posY = 20 - (i * 55);
            var btn = CreerBouton(panelQuestion.transform, "Reponse " + (i + 1), new Vector2(260, 45), new Vector2(0, posY), boutonSecondaire);
            var tmp = btn.GetComponentInChildren<TMP_Text>();
            tmp.text = "Reponse " + (i + 1);
            boutonsReponses[i] = btn;
            textesReponses[i] = tmp;
            fondsReponses[i] = btn.GetComponent<Image>();

            var answerButton = btn.gameObject.AddComponent<AnswerButton>();
            answerButton.gameManager = gameManager;
            answerButton.indexReponse = i;
            answerButton.confirmerAuClic = false;
        }

        var boutonContinuerQuestion = CreerBouton(panelQuestion.transform, "CONTINUER", new Vector2(220, 50), new Vector2(0, -200), boutonPrincipal);

        var textePause = CreerTexte(panelPause.transform, "Pause\nVoulez-vous quitter le jeu ?", 26, TextAlignmentOptions.Center, new Vector2(0, 90));
        var boutonQuitter = CreerBouton(panelPause.transform, "Quitter", new Vector2(220, 45), new Vector2(0, 10), boutonSecondaire);
        var boutonRecommencer = CreerBouton(panelPause.transform, "Recommencer", new Vector2(220, 45), new Vector2(0, -50), boutonSecondaire);
        var boutonAnnuler = CreerBouton(panelPause.transform, "Annuler", new Vector2(220, 45), new Vector2(0, -110), boutonPrincipal);

        var texteFin = CreerTexte(panelFin.transform, "Bravo ! Score : 0", 28, TextAlignmentOptions.Center, new Vector2(0, 40));
        var boutonQuitterFin = CreerBouton(panelFin.transform, "Quitter", new Vector2(220, 45), new Vector2(0, -80), boutonSecondaire);

        var texteScore = CreerTexte(hud.transform, "Score : 0", 22, TextAlignmentOptions.Center, new Vector2(-80, 0));
        var texteTirs = CreerTexte(hud.transform, "0/5 TIRS", 22, TextAlignmentOptions.Center, new Vector2(110, 0));

        if (gameManager != null)
        {
            gameManager.panelRegles1 = panelRegles1;
            gameManager.panelRegles2 = panelRegles2;
            gameManager.panelQuestion = panelQuestion;
            gameManager.panelPause = panelPause;
            gameManager.panelFin = panelFin;

            gameManager.texteQuestion = texteQuestion;
            gameManager.texteNumeroQuestion = texteNumeroQuestion;
            gameManager.textesReponses = textesReponses;
            gameManager.fondsReponses = fondsReponses;
            gameManager.texteTirs = texteTirs;
            gameManager.texteFin = texteFin;

            gameManager.ReinitialiserUI();
        }

        if (tirMascotte != null)
            tirMascotte.texteScore = texteScore;

        boutonJouer.onClick.AddListener(() => gameManager?.ContinuerRegles());
        boutonPret.onClick.AddListener(() => gameManager?.ContinuerRegles());
        boutonContinuerQuestion.onClick.AddListener(() => gameManager?.ConfirmerReponse());

        boutonQuitter.onClick.AddListener(() => gameManager?.Quitter());
        boutonRecommencer.onClick.AddListener(() => gameManager?.Recommencer());
        boutonAnnuler.onClick.AddListener(() => gameManager?.Reprendre());
        boutonQuitterFin.onClick.AddListener(() => gameManager?.Quitter());

        panelRegles2.SetActive(false);
        panelQuestion.SetActive(false);
        panelPause.SetActive(false);
        panelFin.SetActive(false);
    }

    Canvas CreerCanvas(string nom)
    {
        var go = new GameObject(nom);
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        go.AddComponent<CanvasScaler>();
        go.AddComponent<GraphicRaycaster>();

        var rect = canvas.GetComponent<RectTransform>();
        rect.sizeDelta = canvasSize;
        rect.localScale = Vector3.one * canvasScale;

        var cam = Camera.main;
        if (cam != null)
        {
            rect.position = cam.transform.position + cam.transform.forward * distanceDevantCamera;
            rect.rotation = Quaternion.LookRotation(rect.position - cam.transform.position);
        }

        return canvas;
    }

    GameObject CreerPanel(Transform parent, string nom, Vector2 size, Vector2 pos, Color couleur)
    {
        var go = new GameObject(nom);
        go.transform.SetParent(parent, false);
        var image = go.AddComponent<Image>();
        image.color = couleur;
        var rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = pos;
        return go;
    }

    TMP_Text CreerTexte(Transform parent, string texte, int size, TextAlignmentOptions align, Vector2 pos)
    {
        var go = new GameObject("TMP_Text");
        go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = texte;
        tmp.fontSize = size;
        tmp.color = texteBlanc;
        tmp.alignment = align;
        var rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(600, 200);
        rect.anchoredPosition = pos;
        return tmp;
    }

    Button CreerBouton(Transform parent, string texte, Vector2 size, Vector2 pos, Color couleur)
    {
        var go = new GameObject("Button");
        go.transform.SetParent(parent, false);
        var image = go.AddComponent<Image>();
        image.color = couleur;
        var button = go.AddComponent<Button>();
        var rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = pos;

        var txt = CreerTexte(go.transform, texte, 22, TextAlignmentOptions.Center, Vector2.zero);
        txt.rectTransform.sizeDelta = size;
        return button;
    }
}