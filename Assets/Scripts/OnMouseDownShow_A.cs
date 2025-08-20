using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseDownShow_A : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject showObject_A;
    public GameObject triangle;
    public bool isPressed = false;

    void Start()
    {
        if (showObject_A != null)
        {
            showObject_A.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (showObject_A != null)
        {
            showObject_A.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (showObject_A != null)
        {
            showObject_A.SetActive(false);
	    this.isPressed = true;
        }
    }
}
