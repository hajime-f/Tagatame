using UnityEngine;

public class Selector : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public int selectedIndex = 0;
    public int moveDiff = 150;
    public bool active = true;
    private bool isStickMoved = false;
    
    private void Start()
    {
	Application.targetFrameRate = 60;
    }

    private void Update()
    {
	if (!active)
	    return;
	
	float verticalInput = this.fixedJoystick.Vertical;

	if (verticalInput < -0.5f && !this.isStickMoved && selectedIndex == 0)
	{
	    this.selectedIndex++;
	    transform.Translate(-moveDiff, 0, 0);
	    this.isStickMoved = true;
	    GetComponent<AudioSource>().Play();
	    
	} else if (verticalInput > 0.5f && !this.isStickMoved && selectedIndex == 1)
	{
	    this.selectedIndex--;
	    transform.Translate(moveDiff, 0, 0);
	    this.isStickMoved = true;
	    GetComponent<AudioSource>().Play();
	}

	if (Mathf.Abs(verticalInput) < 0.2f)
	{
	    this.isStickMoved = false;
	}
	
    }
}
