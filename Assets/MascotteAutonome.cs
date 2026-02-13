
using UnityEngine;

public class MascotteAutonome : MonoBehaviour
{
    public Rigidbody rbBallon;
    public Transform pointDeTir;
    public Transform cibleBut;
    public Animator animator;
   
    public float vitesseCourse = 3.0f;
    private bool enTrainDeCourir = false;

    void Start()
    {
        // On attend 2 secondes avant de commencer à courir
        Invoke("DemarrerCourse", 2f);
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", true);
    }

    void DemarrerCourse()
    {
        enTrainDeCourir = true;
        animator.SetBool("isRunning", true); 
    }

    void Update()
    {
        if (enTrainDeCourir)
        {
            // Naruto avance vers le point où le ballon doit être tiré
            float distance = Vector3.Distance(transform.position, rbBallon.transform.position);

            if (distance < 0.5f) // S'il est encore loin du ballon
            {
                TerminerCourseEtTirer();
                // transform.position = Vector3.MoveTowards(transform.position, rbBallon.transform.position, vitesseCourse * Time.deltaTime);
            }
            else // S'il est arrivé au ballon
            {
                TerminerCourseEtTirer();
            }
        }
    }

    void TerminerCourseEtTirer()
    {
        enTrainDeCourir = false;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Kick"); // Lance l'animation de tir
       
        // On attend la fin de l'animation de jambe pour donner la force (0.2s)
        Invoke("AppliquerForceBallon", 0.2f);
    }

    void AppliquerForceBallon()
    {
        rbBallon.isKinematic = false;
        Vector3 direction = (cibleBut.position - rbBallon.transform.position).normalized;
        rbBallon.AddForce(direction * 22f, ForceMode.Impulse);
       
        // Affiche le quiz après le tir
        Invoke("OuvrirQuiz", 1.5f);
    }

    void OuvrirQuiz()
    {
        GameObject.Find("CanvasQuiz").SetActive(true);
    }
}
