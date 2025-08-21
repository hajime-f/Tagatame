using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParameterAllocator : MonoBehaviour
{
    [System.Serializable]
    public class ParamData
    {
        public string name;        // 表示用
        public Slider slider;      // 対応するスライダー
        public int pointCost;      // 1段階上げるのに必要なポイント
        public int valuePerPoint;  // 実際のパラメータ増加量
        public int currentPoints = 0; // このパラメータに割り振ったポイント数
	public int maxPoints;         // 固定上限
	public TextMeshProUGUI textMesh;  // currentPointsを表示するUI
    }

    public ParamData[] parameters;
    public int totalPoints = 200; // 初期ポイント
    private int selectedIndex = 0;
    private bool isStickMoved = false;

    public FixedJoystick fixedJoystick;    
    public GameObject parameterSelector;
    private AudioSource audioSource;
    public GameObject menuNav;

    private float holdTimer = 0f;        // 入力保持用タイマー
    public float repeatDelay = 0.3f;     // 最初に反応するまでの時間
    public float repeatRate = 0.05f;      // 押しっぱなし時の繰り返し間隔

    void Start()
    {
	audioSource = GetComponent<AudioSource>();

	foreach (var param in parameters)
        {
	    param.maxPoints = totalPoints * param.valuePerPoint;
            param.slider.maxValue = param.maxPoints;
            param.slider.value = param.currentPoints * param.valuePerPoint;
            param.textMesh.text = param.currentPoints.ToString();
	    param.slider.interactable = false;
        }
    }

    void Update()
    {
	CancelCreateConfirmation.State state = menuNav.GetComponent<CancelCreateConfirmation>().state;
	if (state == CancelCreateConfirmation.State.ConfirmReturn || state == CancelCreateConfirmation.State.CharactorConfirmation)
	    return;
	
	selectedIndex = parameterSelector.GetComponent<ParameterSelector>().selectedParameter;
	float horizontalInput = this.fixedJoystick.Horizontal;
	
	if (Mathf.Abs(horizontalInput) > 0.5f)
	{
	    holdTimer += Time.deltaTime;
	    
	    if (!isStickMoved) 
	    {
		// スティックを倒した瞬間に1回反応
		AddPoint(horizontalInput > 0 ? 1 : -1, true);
		isStickMoved = true;
		holdTimer = 0f;
	    }
	    else if (holdTimer >= repeatDelay) 
	    {
		// 一定時間経過したら連続入力モード
		if (holdTimer >= repeatDelay + repeatRate) 
		{
		    AddPoint(horizontalInput > 0 ? 1 : -1, false);
		    holdTimer = repeatDelay; // リセットして繰り返す
		}
	    }
	}
	else
	{
	    // スティックがニュートラルに戻ったらリセット
	    isStickMoved = false;
	    holdTimer = 0f;
	}
	
	HighlightSelected();
    }    
    
    void AddPoint(int delta, bool isAudio)
    {
        ParamData param = parameters[selectedIndex];

        if (delta > 0 && totalPoints >= param.pointCost && param.currentPoints < param.maxPoints)
        {
            param.currentPoints += param.valuePerPoint;
            totalPoints -= param.pointCost;
            param.slider.value = param.currentPoints;
	    param.textMesh.text = param.currentPoints.ToString();
	    if (isAudio)
		audioSource.Play();
        }
        else if (delta < 0 && param.currentPoints > 0)
        {
            param.currentPoints -= param.valuePerPoint;
            totalPoints += param.pointCost;
            param.slider.value = param.currentPoints;
	    param.textMesh.text = param.currentPoints.ToString();
	    if (isAudio)
		audioSource.Play();
        }
    }

    void HighlightSelected()
    {
        for (int i = 0; i < parameters.Length; i++)
        {
            ColorBlock colors = parameters[i].slider.colors;
            colors.normalColor = (i == selectedIndex) ? Color.yellow : Color.white;
            parameters[i].slider.colors = colors;
        }
    }
}
