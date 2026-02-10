using UnityEngine;

public class DetecteurBut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ballon"))
        {
            Debug.Log("BUT !");
            TirMascotte scriptMascotte = Object.FindAnyObjectByType<TirMascotte>();
            if (scriptMascotte != null)
            {
                scriptMascotte.ReplacerBallon();
            }
        }
    }
}