using UnityEngine;
using TMPro;

public class updatePoint : MonoBehaviour
{

    private TextMeshProUGUI textMesh;
    private int totalPoints;
    public GameObject parameterAllocator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        totalPoints = parameterAllocator.GetComponent<ParameterAllocator>().totalPoints;
	textMesh.text = totalPoints.ToString();
    }
}
