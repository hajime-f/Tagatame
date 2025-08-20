using UnityEngine;

public class MenuSelector : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public int selectedMenu = 0;
    private bool isStickMoved = false;
    private AudioSource audioSource;
    public GameObject description;
    public int maxMenu = 1;

    void Start()
    {
	Application.targetFrameRate = 60;
	audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
	float verticalInput = this.fixedJoystick.Vertical;
	
	TypewriterEffect.State state = description.GetComponent<TypewriterEffect>().state;

	if (state == TypewriterEffect.State.ConfirmReturn || state == TypewriterEffect.State.CharactorConfirmation) {
	    if (verticalInput < -0.5f && !this.isStickMoved && selectedMenu != maxMenu)
	    {
		this.selectedMenu++;
		transform.Translate(-86, 0, 0);
		this.isStickMoved = true;
		audioSource.Play();
	    } else if (verticalInput > 0.5f && !this.isStickMoved && selectedMenu != 0)
	    {
		this.selectedMenu--;
		transform.Translate(86, 0, 0);
		this.isStickMoved = true;
		audioSource.Play();
	    }
	    
	    if (Mathf.Abs(verticalInput) < 0.2f)
	    {
		this.isStickMoved = false;
	    }	            
	}
        
    }
}
