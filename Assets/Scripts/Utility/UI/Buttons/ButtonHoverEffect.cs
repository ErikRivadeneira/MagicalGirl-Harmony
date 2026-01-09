using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip enterSound;
    [SerializeField] AudioClip clickSound;
    [SerializeField] Image buttonImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        source.PlayOneShot(clickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        source.PlayOneShot(enterSound);
        buttonImage.color = new Color(0,0, 0, 0.7f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = new Color(0, 0, 0, 0);
    }
}
