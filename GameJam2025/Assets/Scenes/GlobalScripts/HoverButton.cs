using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    void Start()
    {
        buttonImage = gameObject.GetComponent<Image>();
        buttonImage.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = new Color(1, 1, 1, 0.15f);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = new Color(1, 1, 1, 1);
    }
}
