using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseDownShow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject showObject;

    void Start()
    {
        if (showObject != null)
        {
            showObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (showObject != null)
        {
            showObject.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (showObject != null)
        {
            showObject.SetActive(false);
	    GetComponent<AudioSource>().Play();
        }
    }
}
