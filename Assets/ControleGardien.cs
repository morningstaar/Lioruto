using UnityEngine;

public class ControleGardien : MonoBehaviour
{
    public float vitesse = 10f;
    public float limiteLaterale = 3.5f; 

    void Update()
    {
        float mouvement = Input.GetAxis("Horizontal") * vitesse * Time.deltaTime;
        
        // On bouge sur le Z
        Vector3 nouvellePos = transform.position + new Vector3(0, 0, mouvement);

        // CORRECTIF : On bloque le Z au lieu du X !
        // On laisse le X tranquille (il gardera sa position de départ, ex: 10)
        nouvellePos.z = Mathf.Clamp(nouvellePos.z, -limiteLaterale, limiteLaterale);

        transform.position = nouvellePos;
    }
}