using UnityEngine;

public class CharactorSelection : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public int selectedIndex = 0;
    private bool isStickMoved = false;
    
    void Start()
    {
	Application.targetFrameRate = 60;
    }

    void Update()
    {
        float horizontalInput = this.fixedJoystick.Horizontal;

	if (horizontalInput < -0.5f && !this.isStickMoved && selectedIndex != 0)
	{
	    this.selectedIndex--;
	    this.isStickMoved = true;
	    GetComponent<AudioSource>().Play();
	} else if (horizontalInput > 0.5f && !this.isStickMoved && selectedIndex != 4)
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
