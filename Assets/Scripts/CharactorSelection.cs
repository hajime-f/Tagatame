using UnityEngine;

public class CharactorSelection : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public int selectedIndex = 0;
    private bool isStickMoved = false;
    public GameObject bButton;
    
    void Start()
    {
	Application.targetFrameRate = 60;
    }

    void Update()
    {
        float horizontalInput = this.fixedJoystick.Horizontal;
	var bScript = bButton.GetComponent<OnMouseDownShow_B>();

	if (horizontalInput < -0.5f && !this.isStickMoved && selectedIndex != 0 && !bScript.isReturned)
	{
	    this.selectedIndex--;
	    this.isStickMoved = true;
	    GetComponent<AudioSource>().Play();
	} else if (horizontalInput > 0.5f && !this.isStickMoved && selectedIndex != 4 && !bScript.isReturned)
	{
	    this.selectedIndex++;
	    this.isStickMoved = true;
	    GetComponent<AudioSource>().Play();
	}

	if (Mathf.Abs(horizontalInput) < 0.2f)
	{
	    this.isStickMoved = false;
	}	
    }
}
