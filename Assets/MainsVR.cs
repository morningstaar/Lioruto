using UnityEngine;

public class MainVR : MonoBehaviour
{
    public TirMascotte scriptTir; 
    public GameObject canvasQuiz; // Glisse ton CanvasQuiz ici

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ballon"))
        {
            Debug.Log("ARRÊT ! Score +400");
            
            // 1. On donne les points
            // if (scriptTir != null) scriptTir.AjouterPoints(400);

            // // 2. On affiche le Quiz
            // if (canvasQuiz != null) canvasQuiz.SetActive(true);

            // // 3. On fige le ballon
            // collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}