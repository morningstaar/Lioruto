using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public GameManager gameManager;
    public int indexReponse;
    public bool confirmerAuClic = true;

    public void OnClick()
    {
        if (gameManager == null)
            return;

        gameManager.SelectionnerReponse(indexReponse);
        if (confirmerAuClic)
            gameManager.ConfirmerReponse();
    }
}