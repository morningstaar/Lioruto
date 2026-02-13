using UnityEngine;

public class MascotteAutonome : MonoBehaviour
{
    public Rigidbody rbBallon;
    public Transform cibleBut;
    public GameObject canvasQuiz; // Référence directe plus fiable que le .Find
    public float vitesseCourse = 4.0f; // Un peu plus rapide pour le dynamisme

    public GameManager gameManager;    
    private Animator animator;
    private bool enTrainDeCourir = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        // On cache le quiz proprement au début via le script au cas où
        if(canvasQuiz) canvasQuiz.SetActive(false);
        
        Invoke("DemarrerCourse", 2f);
    }

    void DemarrerCourse()
    {
        enTrainDeCourir = true;
        if(animator) animator.SetBool("isRunning", true); 
    }

    void Update()
    {
        if (enTrainDeCourir && rbBallon != null)
        {
            float distance = Vector3.Distance(transform.position, rbBallon.transform.position);

            // CORRECTION : "Tant que je suis loin, je cours"
            if (distance > 1.6f) 
            {
                transform.position = Vector3.MoveTowards(transform.position, rbBallon.transform.position, vitesseCourse * Time.deltaTime);
                // Optionnel : Faire en sorte que Naruto regarde le ballon
                transform.LookAt(new Vector3(rbBallon.transform.position.x, transform.position.y, rbBallon.transform.position.z));
            }
            else 
            {
                TerminerCourseEtTirer();
            }
        }
    }

    void TerminerCourseEtTirer()
    {
        enTrainDeCourir = false;
        if(animator) {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("Kick");
        }
        
        // On attend le moment de l'impact (0.9s selon ton animation)
        Invoke("AppliquerForceBallon", 0.9f);
    }

    void AppliquerForceBallon()
    {
        rbBallon.isKinematic = false;
        // Direction vers le but
        Vector3 direction = (cibleBut.position - rbBallon.transform.position).normalized;
        // Ajout d'une petite force vers le haut pour l'effet de tir (0.2f sur Y)
        direction += Vector3.up * 0.2f; 
        
        // rbBallon.AddForce(direction * 22f, ForceMode.Impulse);
        gameManager.LancerProchainTir();
        
        Invoke("OuvrirQuiz", 1.5f);
    }

    void OuvrirQuiz()
    {
        if(canvasQuiz) canvasQuiz.SetActive(true);
    }
}