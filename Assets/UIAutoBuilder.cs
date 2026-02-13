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
    public Color fondPanel = new Color(0.05f, 0.25f, 0.35f, 0.92f);
    public Color fondHUD = new Color(0f, 0.15f, 0.2f, 0.75f);
    public Color panneauCarte = new Color(0.02f, 0.35f, 0.45f, 0.9f);
    public Color panneauAlerte = new Color(0.95f, 0.7f, 0.15f, 0.95f);
    public Color boutonPrincipal = new Color(0.25f, 0.85f, 0.25f, 1f);
    public Color boutonSecondaire = new Color(0.1f, 0.6f, 0.75f, 1f);
    public Color texteBlanc = Color.white;
    public Color texteJaune = new Color(1f, 0.9f, 0.4f, 1f);
    [Header("Style premium")]
    public Color panelTop = new Color(0.1f, 0.45f, 0.55f, 1f);
    public Color panelBottom = new Color(0.03f, 0.2f, 0.3f, 1f);
    public Color cardTop = new Color(0.08f, 0.5f, 0.6f, 1f);
    public Color cardBottom = new Color(0.02f, 0.28f, 0.36f, 1f);
    public Color alertTop = new Color(0.98f, 0.82f, 0.25f, 1f);
    public Color alertBottom = new Color(0.85f, 0.55f, 0.15f, 1f);
    public Color buttonPrimaryTop = new Color(0.45f, 0.95f, 0.35f, 1f);
    public Color buttonPrimaryBottom = new Color(0.15f, 0.7f, 0.25f, 1f);
    public Color buttonSecondaryTop = new Color(0.25f, 0.75f, 0.95f, 1f);
    public Color buttonSecondaryBottom = new Color(0.08f, 0.45f, 0.7f, 1f);
    public Color hudTop = new Color(0.2f, 0.35f, 0.4f, 1f);
    public Color hudBottom = new Color(0.05f, 0.2f, 0.25f, 1f);
    public Color bordure = new Color(0.4f, 0.9f, 1f, 0.9f);
    public Color glow = new Color(0.4f, 0.9f, 1f, 0.35f);
    public int textureSize = 256;
    public int bordureTaille = 5;
    public int rayonCoins = 22;
    public float glowTaille = 12f;

    GameManager gameManager;
    TirMascotte tirMascotte;
    Sprite spritePanel;
    Sprite spriteCarte;
    Sprite spriteAlerte;
    Sprite spriteBoutonPrincipal;
    Sprite spriteBoutonSecondaire;
    Sprite spriteHud;

    void Start()
    {
        gameManager = Object.FindAnyObjectByType<GameManager>();
        tirMascotte = Object.FindAnyObjectByType<TirMascotte>();

        InitialiserSprites();

        var canvas = CreerCanvas("UIRoot");

        var panelRegles1 = CreerPanel(canvas.transform, "Panel_Regles_1", new Vector2(720, 440), Vector2.zero, fondPanel);
        var panelRegles2 = CreerPanel(canvas.transform, "Panel_Regles_2", new Vector2(760, 500), Vector2.zero, fondPanel);
        var panelQuestion = CreerPanel(canvas.transform, "Panel_Question", new Vector2(760, 460), Vector2.zero, fondPanel);
        var panelPause = CreerPanel(canvas.transform, "Panel_Pause", new Vector2(520, 360), Vector2.zero, fondPanel);
        var panelFin = CreerPanel(canvas.transform, "Panel_Fin", new Vector2(520, 300), Vector2.zero, fondPanel);
        var hud = CreerPanel(canvas.transform, "HUD", new Vector2(420, 80), new Vector2(0, 230), fondHUD);

        AppliquerStylePanel(panelRegles1, spritePanel, 0.35f);
        AppliquerStylePanel(panelRegles2, spritePanel, 0.35f);
        AppliquerStylePanel(panelQuestion, spritePanel, 0.35f);
        AppliquerStylePanel(panelPause, spritePanel, 0.35f);
        AppliquerStylePanel(panelFin, spritePanel, 0.35f);
        AppliquerStylePanel(hud, spriteHud, 0.2f);

        var titreRegles = CreerTexte(panelRegles1.transform, "REGLES DU JEU", 32, TextAlignmentOptions.Center, new Vector2(0, 140));
        var texteRegles1 = CreerTexte(panelRegles1.transform, "Arrête un maximum de tirs en bloquant les ballons.\nAprès chaque tir, réponds correctement aux questions de foot.\nAtteins 1000 points en 5 tirs pour gagner.", 24, TextAlignmentOptions.Center, new Vector2(0, 40));
        var boutonJouer = CreerBouton(panelRegles1.transform, "JOUER", new Vector2(220, 55), new Vector2(0, -160), boutonPrincipal);
        AppliquerStyleBouton(boutonJouer, spriteBoutonPrincipal);

        var carteGauche = CreerPanel(panelRegles2.transform, "Carte_Gauche", new Vector2(300, 220), new Vector2(-180, 40), panneauCarte);
        var carteDroite = CreerPanel(panelRegles2.transform, "Carte_Droite", new Vector2(300, 220), new Vector2(180, 40), panneauCarte);
        AppliquerStylePanel(carteGauche, spriteCarte, 0.3f);
        AppliquerStylePanel(carteDroite, spriteCarte, 0.3f);
        var titreGauche = CreerTexte(carteGauche.transform, "Bloquez les ballons !", 22, TextAlignmentOptions.Center, new Vector2(0, 80));
        var texteGauche = CreerTexte(carteGauche.transform, "Arrêtez les tirs avec\nles gants virtuels pour\nmarquer 400 points\npar arrêt !", 20, TextAlignmentOptions.Center, new Vector2(0, 0));
        var titreDroite = CreerTexte(carteDroite.transform, "Répondez aux questions !", 22, TextAlignmentOptions.Center, new Vector2(0, 80));
        var texteDroite = CreerTexte(carteDroite.transform, "Répondez correctement\naprès chaque tir pour\ngagner des points bonus.\n\nTir arrêté + bonne réponse : x2\nTir manqué + bonne réponse : +20", 18, TextAlignmentOptions.Center, new Vector2(0, -10));

        var alerte = CreerPanel(panelRegles2.transform, "Alerte", new Vector2(640, 50), new Vector2(0, -80), panneauAlerte);
        AppliquerStylePanel(alerte, spriteAlerte, 0.25f);
        var texteAlerte = CreerTexte(alerte.transform, "Attention ! Vous avez 5 tirs au total et devez atteindre 1000 points !", 20, TextAlignmentOptions.Center, Vector2.zero);
        texteAlerte.color = new Color(0.2f, 0.15f, 0.05f, 1f);

        var boutonPret = CreerBouton(panelRegles2.transform, "PRET !", new Vector2(220, 55), new Vector2(0, -170), boutonPrincipal);
        AppliquerStyleBouton(boutonPret, spriteBoutonPrincipal);

        var barreQuestion = CreerPanel(panelQuestion.transform, "Barre_Question", new Vector2(520, 55), new Vector2(0, 170), fondHUD);
        AppliquerStylePanel(barreQuestion, spriteHud, 0.2f);
        var texteNumeroQuestion = CreerTexte(barreQuestion.transform, "QUESTION 1", 24, TextAlignmentOptions.Center, Vector2.zero);
        var texteQuestion = CreerTexte(panelQuestion.transform, "Qui a remporté la Coupe du Monde 2018 ?", 26, TextAlignmentOptions.Center, new Vector2(0, 105));

        var boutonsReponses = new Button[4];
        var textesReponses = new TMP_Text[4];
        var fondsReponses = new Image[4];

        for (int i = 0; i < 4; i++)
        {
            var posY = 35 - (i * 60);
            var btn = CreerBouton(panelQuestion.transform, "Reponse " + (i + 1), new Vector2(320, 50), new Vector2(0, posY), boutonSecondaire);
            AppliquerStyleBouton(btn, spriteBoutonSecondaire);
            var tmp = btn.GetComponentInChildren<TMP_Text>();
            tmp.text = "Reponse " + (i + 1);
            boutonsReponses[i] = btn;
            textesReponses[i] = tmp;
            fondsReponses[i] = btn.GetComponent<Image>();

            var answerButton = btn.gameObject.AddComponent<AnswerButton>();
            answerButton.gameManager = gameManager;
            answerButton.indexReponse = i;
            answerButton.confirmerAuClic = false;
            btn.onClick.AddListener(answerButton.OnClick);
        }

        var boutonContinuerQuestion = CreerBouton(panelQuestion.transform, "CONTINUER", new Vector2(230, 55), new Vector2(0, -210), boutonPrincipal);
        AppliquerStyleBouton(boutonContinuerQuestion, spriteBoutonPrincipal);

        var textePause = CreerTexte(panelPause.transform, "Pause\nVoulez-vous quitter le jeu ?", 26, TextAlignmentOptions.Center, new Vector2(0, 90));
        var boutonQuitter = CreerBouton(panelPause.transform, "Quitter", new Vector2(220, 45), new Vector2(0, 10), boutonSecondaire);
        var boutonRecommencer = CreerBouton(panelPause.transform, "Recommencer", new Vector2(220, 45), new Vector2(0, -50), boutonSecondaire);
        var boutonAnnuler = CreerBouton(panelPause.transform, "Annuler", new Vector2(220, 45), new Vector2(0, -110), boutonPrincipal);
        AppliquerStyleBouton(boutonQuitter, spriteBoutonSecondaire);
        AppliquerStyleBouton(boutonRecommencer, spriteBoutonSecondaire);
        AppliquerStyleBouton(boutonAnnuler, spriteBoutonPrincipal);

        var texteFin = CreerTexte(panelFin.transform, "Bravo ! Score : 0", 28, TextAlignmentOptions.Center, new Vector2(0, 40));
        var boutonQuitterFin = CreerBouton(panelFin.transform, "Quitter", new Vector2(220, 45), new Vector2(0, -80), boutonSecondaire);
        AppliquerStyleBouton(boutonQuitterFin, spriteBoutonSecondaire);

        var texteScore = CreerTexte(hud.transform, "Score : 0", 22, TextAlignmentOptions.Center, new Vector2(-80, 0));
        var texteTirs = CreerTexte(hud.transform, "0/5 TIRS", 22, TextAlignmentOptions.Center, new Vector2(110, 0));

        AppliquerStyleTexte(titreRegles, 0.1f, texteJaune);
        AppliquerStyleTexte(texteRegles1, 0.08f, texteBlanc);
        AppliquerStyleTexte(titreGauche, 0.08f, texteJaune);
        AppliquerStyleTexte(texteGauche, 0.06f, texteBlanc);
        AppliquerStyleTexte(titreDroite, 0.08f, texteJaune);
        AppliquerStyleTexte(texteDroite, 0.06f, texteBlanc);
        AppliquerStyleTexte(texteNumeroQuestion, 0.06f, texteJaune);
        AppliquerStyleTexte(texteQuestion, 0.06f, texteBlanc);
        AppliquerStyleTexte(textePause, 0.06f, texteBlanc);
        AppliquerStyleTexte(texteFin, 0.06f, texteBlanc);
        AppliquerStyleTexte(texteScore, 0.06f, texteBlanc);
        AppliquerStyleTexte(texteTirs, 0.06f, texteBlanc);

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
        canvas.worldCamera = Camera.main;
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

        CreerEventSystemSiAbsent();

        return canvas;
    }

    void CreerEventSystemSiAbsent()
    {
        if (Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
            return;

        var go = new GameObject("EventSystem");
        go.AddComponent<UnityEngine.EventSystems.EventSystem>();
        go.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
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

    TextMeshProUGUI CreerTexte(Transform parent, string texte, int size, TextAlignmentOptions align, Vector2 pos)
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
        AppliquerStyleTexte(txt, 0.08f, texteBlanc);
        return button;
    }

    void InitialiserSprites()
    {
        spritePanel = CreerSprite(panelTop, panelBottom);
        spriteCarte = CreerSprite(cardTop, cardBottom);
        spriteAlerte = CreerSprite(alertTop, alertBottom);
        spriteBoutonPrincipal = CreerSprite(buttonPrimaryTop, buttonPrimaryBottom);
        spriteBoutonSecondaire = CreerSprite(buttonSecondaryTop, buttonSecondaryBottom);
        spriteHud = CreerSprite(hudTop, hudBottom);
    }

    Sprite CreerSprite(Color top, Color bottom)
    {
        return CreerSpriteArrondi(textureSize, textureSize, top, bottom, bordure, bordureTaille, rayonCoins);
    }

    Sprite CreerSpriteArrondi(int width, int height, Color top, Color bottom, Color borderColor, int borderSize, int radius)
    {
        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        float radiusSqr = radius * radius;
        int maxX = width - 1;
        int maxY = height - 1;

        for (int y = 0; y < height; y++)
        {
            float t = maxY == 0 ? 0f : (float)y / maxY;
            Color baseColor = Color.Lerp(bottom, top, t);

            for (int x = 0; x < width; x++)
            {
                float alpha = 1f;
                int dx = 0;
                int dy = 0;

                if (x < radius)
                    dx = radius - x;
                else if (x > maxX - radius)
                    dx = x - (maxX - radius);

                if (y < radius)
                    dy = radius - y;
                else if (y > maxY - radius)
                    dy = y - (maxY - radius);

                if (dx > 0 || dy > 0)
                {
                    if ((dx * dx + dy * dy) > radiusSqr)
                        alpha = 0f;
                }

                Color c = baseColor;
                if (borderSize > 0 && (x < borderSize || x > maxX - borderSize || y < borderSize || y > maxY - borderSize))
                    c = Color.Lerp(baseColor, borderColor, 0.8f);

                c.a *= alpha;
                tex.SetPixel(x, y, c);
            }
        }

        tex.Apply();

        var rect = new Rect(0, 0, width, height);
        var pivot = new Vector2(0.5f, 0.5f);
        var border = new Vector4(radius, radius, radius, radius);
        return Sprite.Create(tex, rect, pivot, 100f, 0, SpriteMeshType.FullRect, border);
    }

    void AppliquerStylePanel(GameObject panel, Sprite sprite, float glowAlpha)
    {
        if (panel == null)
            return;

        var image = panel.GetComponent<Image>();
        if (image == null || sprite == null)
            return;

        image.sprite = sprite;
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        AjouterGlow(panel.transform, sprite, glowAlpha, glowTaille);
    }

    void AppliquerStyleBouton(Button button, Sprite sprite)
    {
        if (button == null)
            return;

        var image = button.GetComponent<Image>();
        if (image == null || sprite == null)
            return;

        image.sprite = sprite;
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        AjouterGlow(button.transform, sprite, 0.25f, 8f);
    }

    void AjouterGlow(Transform parent, Sprite sprite, float alpha, float sizeOffset)
    {
        if (parent == null || sprite == null)
            return;

        var parentRect = parent.GetComponent<RectTransform>();
        if (parentRect == null)
            return;

        var glowObj = new GameObject("Glow");
        glowObj.transform.SetParent(parent, false);
        glowObj.transform.SetAsFirstSibling();

        var img = glowObj.AddComponent<Image>();
        img.sprite = sprite;
        img.type = Image.Type.Sliced;
        var c = glow;
        c.a = alpha;
        img.color = c;

        var rect = glowObj.GetComponent<RectTransform>();
        rect.sizeDelta = parentRect.sizeDelta + new Vector2(sizeOffset, sizeOffset);
        rect.anchoredPosition = Vector2.zero;
    }

    void AppliquerStyleTexte(TextMeshProUGUI tmp, float outlineWidth, Color outlineColor)
    {
        if (tmp == null)
            return;

        tmp.outlineWidth = outlineWidth;
        tmp.outlineColor = outlineColor;
    }
}