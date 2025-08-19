using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseDownShow_B : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject description;
    public GameObject showObject_B;
    public bool isCanceled = false;
    public bool isReturned = false;
    public bool isCanceled_return = false;

    void Start()
    {
        if (showObject_B != null)
	    showObject_B.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (showObject_B != null)
	    showObject_B.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (showObject_B != null)
	    showObject_B.SetActive(false);

	if (isReturned)
	{
	    isReturned = false;
	    isCanceled_return = true;
	}
	
	var typewriter = description.GetComponent<TypewriterEffect>();
	if (typewriter != null)
        {
            if (typewriter.isTyping && !isCanceled_return)
            {
                isCanceled = true;   // タイプライター中に押された
            }
            else
            {
                isReturned = true;   // タイプライター停止中に押された
            }
        }
    }
}
