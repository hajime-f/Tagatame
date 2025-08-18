using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseDownShow_B : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject showObject_B;

    void Start()
    {
        if (showObject_B != null)
        {
            showObject_B.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (showObject_B != null)
        {
            showObject_B.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (showObject_B != null)
        {
            showObject_B.SetActive(false);
        }
    }
}
