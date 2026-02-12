using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Références gameplay")]
    public TirMascotte tirMascotte;
    public int nombreDeTirs = 5;
    public int pointsParArret = 400;
    public int pointsBonusBonneReponseSansArret = 20;
    public int scoreMinimumPourGagner = 1000;

    [Header("UI - Panneaux")]
    public GameObject panelRegles1;
    public GameObject panelRegles2;
    public GameObject panelQuestion;
    public GameObject panelPause;
    public GameObject panelFin;

    [Header("UI - Question")]
    public TMP_Text texteQuestion;
    public TMP_Text texteNumeroQuestion;
    public TMP_Text[] textesReponses;
    public Image[] fondsReponses;
    public Color couleurSelection = new Color(0.2f, 0.8f, 0.2f, 1f);
    public Color couleurParDefaut = new Color(1f, 1f, 1f, 1f);

    [Header("UI - HUD")]
    public TMP_Text texteTirs;

    [Header("UI - Fin")]
    public TMP_Text texteFin;

    [Header("Entrées (clavier/joystick)")]
    public KeyCode touchePause = KeyCode.Y;
    public KeyCode toucheConfirmer = KeyCode.X;

    [Header("Mascotte")]
    public Animator mascotteAnimator;
    public string triggerSalut = "Greet";
    public string triggerTir = "Kick";
    public AudioSource ambiancePublic;

    [System.Serializable]
    public class Question
    {
        [TextArea] public string intitule;
        public string[] reponses = new string[4];
        public int indexBonneReponse;
    }

    [Header("Questions")]
    public List<Question> questions = new List<Question>();

    int tirsEffectues = 0;
    bool tirActif = false;
    bool dernierTirArrete = false;
    int indexReponseSelectionnee = -1;
    Question questionCourante;

    void Start()
    {
        if (panelRegles1 != null || panelRegles2 != null || panelQuestion != null)
            InitialiserUI();
    }

    void Update()
    {
        if (tirsEffectues > 0 && Input.GetKeyDown(touchePause))
        {
            BasculerPause();
        }

        if (panelQuestion != null && panelQuestion.activeSelf && indexReponseSelectionnee >= 0)
        {
            if (Input.GetKeyDown(toucheConfirmer))
            {
                ConfirmerReponse();
            }
        }
    }

    void InitialiserUI()
    {
        if (panelRegles1 != null) panelRegles1.SetActive(true);
        if (panelRegles2 != null) panelRegles2.SetActive(false);
        if (panelQuestion != null) panelQuestion.SetActive(false);
        if (panelPause != null) panelPause.SetActive(false);
        if (panelFin != null) panelFin.SetActive(false);

        if (tirMascotte != null)
            tirMascotte.ResetScore();

        MettreAJourHUD();
    }

    public void ReinitialiserUI()
    {
        InitialiserUI();
    }

    public void ContinuerRegles()
    {
        if (panelRegles1 != null && panelRegles1.activeSelf)
        {
            panelRegles1.SetActive(false);
            if (panelRegles2 != null) panelRegles2.SetActive(true);
            return;
        }

        if (panelRegles2 != null && panelRegles2.activeSelf)
        {
            panelRegles2.SetActive(false);
            DemarrerPartie();
        }
    }

    void DemarrerPartie()
    {
        tirsEffectues = 0;
        tirActif = false;
        dernierTirArrete = false;

        if (ambiancePublic != null)
            ambiancePublic.Play();

        if (mascotteAnimator != null && !string.IsNullOrWhiteSpace(triggerSalut))
            mascotteAnimator.SetTrigger(triggerSalut);

        LancerProchainTir();
    }

    void LancerProchainTir()
    {
        if (tirsEffectues >= nombreDeTirs)
        {
            FinDePartie();
            return;
        }

        tirsEffectues += 1;
        tirActif = true;
        dernierTirArrete = false;

        MettreAJourHUD();

        if (mascotteAnimator != null && !string.IsNullOrWhiteSpace(triggerTir))
            mascotteAnimator.SetTrigger(triggerTir);

        if (tirMascotte != null)
            tirMascotte.Tirer();
    }

    public bool TryResolveShot(bool arrete)
    {
        if (!tirActif)
            return false;

        tirActif = false;
        dernierTirArrete = arrete;

        if (tirMascotte != null)
            tirMascotte.ReplacerBallon();

        AfficherQuestion();
        return true;
    }

    void AfficherQuestion()
    {
        if (panelQuestion == null || questions.Count == 0)
        {
            LancerProchainTir();
            return;
        }

        questionCourante = questions[Random.Range(0, questions.Count)];
        if (texteNumeroQuestion != null)
            texteNumeroQuestion.text = "QUESTION " + tirsEffectues;
        if (texteQuestion != null)
            texteQuestion.text = questionCourante.intitule;

        for (int i = 0; i < textesReponses.Length; i++)
        {
            if (i < questionCourante.reponses.Length && textesReponses[i] != null)
                textesReponses[i].text = questionCourante.reponses[i];

            if (fondsReponses != null && i < fondsReponses.Length && fondsReponses[i] != null)
                fondsReponses[i].color = couleurParDefaut;
        }

        indexReponseSelectionnee = -1;
        panelQuestion.SetActive(true);
    }

    public void SelectionnerReponse(int index)
    {
        indexReponseSelectionnee = index;

        for (int i = 0; i < fondsReponses.Length; i++)
        {
            if (fondsReponses[i] != null)
                fondsReponses[i].color = (i == index) ? couleurSelection : couleurParDefaut;
        }
    }

    public void ConfirmerReponse()
    {
        if (questionCourante == null || indexReponseSelectionnee < 0)
            return;

        bool bonne = indexReponseSelectionnee == questionCourante.indexBonneReponse;

        if (bonne && tirMascotte != null)
        {
            if (dernierTirArrete)
                tirMascotte.AjouterPoints(pointsParArret); // double l'arrêt (400 + 400)
            else
                tirMascotte.AjouterPoints(pointsBonusBonneReponseSansArret);
        }

        panelQuestion.SetActive(false);
        StartCoroutine(DelaiProchainTir());
    }

    IEnumerator DelaiProchainTir()
    {
        yield return new WaitForSeconds(1f);
        LancerProchainTir();
    }

    void FinDePartie()
    {
        if (panelFin != null)
            panelFin.SetActive(true);

        if (texteFin != null && tirMascotte != null)
        {
            bool gagne = tirMascotte.scoreTotal >= scoreMinimumPourGagner;
            texteFin.text = gagne
                ? "Bravo ! Score : " + tirMascotte.scoreTotal
                : "Perdu. Score : " + tirMascotte.scoreTotal + " / " + scoreMinimumPourGagner;
        }
    }

    void MettreAJourHUD()
    {
        if (texteTirs != null)
            texteTirs.text = tirsEffectues + "/" + nombreDeTirs + " TIRS";
    }

    public void BasculerPause()
    {
        if (panelPause == null)
            return;

        bool activer = !panelPause.activeSelf;
        panelPause.SetActive(activer);
        Time.timeScale = activer ? 0f : 1f;
    }

    public void Reprendre()
    {
        if (panelPause != null) panelPause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quitter()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Recommencer()
    {
        Time.timeScale = 1f;
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.buildIndex);
    }
}