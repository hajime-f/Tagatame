using UnityEngine;

public class ParameterSelector : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public int selectedParameter = 0;
    public bool active = true;
    private bool isStickMoved = false;
    private AudioSource audioSource;
    private int maxParameter = 6;
    
    void Start()
    {
	audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
	if (!active)
	    return;
	
	float verticalInput = this.fixedJoystick.Vertical;
	
	if (verticalInput < -0.5f && !this.isStickMoved && selectedParameter != maxParameter)
	{
	    this.selectedParameter++;
	    transform.Translate(-120, 0, 0);
	    this.isStickMoved = true;
	    audioSource.Play();
	} else if (verticalInput > 0.5f && !this.isStickMoved && selectedParameter != 0)
	{
	    this.selectedParameter--;
	    transform.Translate(120, 0, 0);
	    this.isStickMoved = true;
	    audioSource.Play();
	}
	
	if (Mathf.Abs(verticalInput) < 0.2f)
	    this.isStickMoved = false;
    }
}
