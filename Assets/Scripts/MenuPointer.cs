using UnityEngine;

public class MenuPointer : MonoBehaviour
{
    public GameObject pointer;                // ポインタ用のUIオブジェクト
    public TypewriterEffect typewriterEffect; // 参照したい TypewriterEffect

    void Update()
    {
        if (typewriterEffect.state == TypewriterEffect.State.ConfirmReturn)
        {
            if (!pointer.activeSelf)
		pointer.SetActive(true);
        }
        else
        {
            if (pointer.activeSelf)
		pointer.SetActive(false);
        }
    }
}
