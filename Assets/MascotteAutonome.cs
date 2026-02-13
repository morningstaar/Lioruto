
using UnityEngine;

public class MascotteAutonome : MonoBehaviour
{
    public Rigidbody rbBallon;
    public Transform pointDeTir;
    public Transform cibleBut;
    public Animator animator;
   
    public float vitesseCourse = 3.0f;
    public float distanceArret = 0.6f;
    private bool enTrainDeCourir = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetBool("isRunning", false);
    }

    public void DemarrerSequence()
    {
        enTrainDeCourir = true;
        if (animator != null)
            animator.SetBool("isRunning", true);
    }

    void Update()
    {
        if (enTrainDeCourir)
        {
            Vector3 cible = pointDeTir != null ? pointDeTir.position : rbBallon.transform.position;
            Vector3 cibleSol = new Vector3(cible.x, transform.position.y, cible.z);
            float distance = Vector3.Distance(transform.position, cibleSol);

            if (distance > distanceArret)
            {
                transform.position = Vector3.MoveTowards(transform.position, cibleSol, vitesseCourse * Time.deltaTime);
                if (cibleSol != transform.position)
                    transform.rotation = Quaternion.LookRotation((cibleSol - transform.position).normalized);
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
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("Kick");
        }
       
        Invoke("AppliquerForceBallon", 0.2f);
    }

    void AppliquerForceBallon()
    {
        if (rbBallon == null || cibleBut == null)
            return;

        rbBallon.isKinematic = false;
        Vector3 direction = (cibleBut.position - rbBallon.transform.position).normalized;
        rbBallon.AddForce(direction * 22f, ForceMode.Impulse);
    }
}
