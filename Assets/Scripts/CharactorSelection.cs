using UnityEngine;

public class CharactorSelection : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public int selectedIndex = 0;
    public int selectedMenu = 0;
    private bool isStickMoved = false;
    private AudioSource audioSource;
    public GameObject description;
    public int maxIndex = 4;
    public int maxMenu = 2;
    
    void Start()
    {
	Application.targetFrameRate = 60;
	audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float horizontalInput = this.fixedJoystick.Horizontal;
	float verticalInput = this.fixedJoystick.Vertical;
	
	TypewriterEffect.State state = description.GetComponent<TypewriterEffect>().state;

	if (state == TypewriterEffect.State.ConfirmReturn) {
	    if (verticalInput < -0.5f && !this.isStickMoved && selectedMenu != 0)
	    {
		this.selectedMenu--;
		this.isStickMoved = true;
		audioSource.Play();
	    } else if (verticalInput > 0.5f && !this.isStickMoved && selectedMenu != maxMenu)
	    {
		this.selectedMenu++;
		this.isStickMoved = true;
		audioSource.Play();
	    }
	    if (Mathf.Abs(verticalInput) < 0.2f)
	    {
		this.isStickMoved = false;
	    }	            
	}
	else
	{
	    if (horizontalInput < -0.5f && !this.isStickMoved && selectedIndex != 0)
	    {
		this.selectedIndex--;
		this.isStickMoved = true;
		audioSource.Play();
	    } else if (horizontalInput > 0.5f && !this.isStickMoved && selectedIndex != maxIndex)
	    {
		this.selectedIndex++;
		this.isStickMoved = true;
		audioSource.Play();
	    }
	    if (Mathf.Abs(horizontalInput) < 0.2f)
	    {
		this.isStickMoved = false;
	    }	
	}
    }
}
