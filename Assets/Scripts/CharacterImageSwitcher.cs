using UnityEngine;
using UnityEngine.UI;

public class CharacterImageSwitcher : MonoBehaviour
{
    public int selectedIndex = 0;
    public GameObject fixedJoystick;

    public Image characterImage;  // 表示するUIのImage
    public Sprite chack_0;
    public Sprite lockie_0;
    public Sprite mad_0;
    public Sprite komugi_0;
    public Sprite lucky_0;

    void Update()
    {
        selectedIndex = fixedJoystick.GetComponent<CharactorSelection>().selectedIndex;

        switch (selectedIndex)
        {
            case 0:
                characterImage.sprite = chack_0;
                break;
            case 1:
                characterImage.sprite = lockie_0;
                break;
            case 2:
                characterImage.sprite = mad_0;
                break;
            case 3:
                characterImage.sprite = komugi_0;
                break;
            case 4:
                characterImage.sprite = lucky_0;
                break;
            default:
                characterImage.sprite = null; // 該当しない場合は非表示にするならnull
                break;
        }
    }
}
