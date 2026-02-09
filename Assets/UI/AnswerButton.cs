using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Script prototype pour animer et changer l'apparence d'un bouton de réponse au survol
[RequireComponent(typeof(Button))]
public class AnswerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.15f;
    public float animationSpeed = 8f;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;

    private Vector3 baseScale;
    private Vector3 targetScale;
    private Image backgroundImage;
    private TextMeshProUGUI label;

    void Awake()
    {
        baseScale = transform.localScale;
        targetScale = baseScale;
        backgroundImage = GetComponent<Image>();
        if (backgroundImage == null)
        {
            // try to find image on child
            backgroundImage = GetComponentInChildren<Image>();
        }

        label = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = baseScale * hoverScale;
        if (backgroundImage != null)
            backgroundImage.color = hoverColor;
        if (label != null)
            label.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = baseScale;
        if (backgroundImage != null)
            backgroundImage.color = normalColor;
        if (label != null)
            label.color = Color.black;
    }
}
