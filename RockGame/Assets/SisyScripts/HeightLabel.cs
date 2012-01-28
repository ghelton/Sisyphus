using UnityEngine;
using System.Collections;

public class HeightLabel : MonoBehaviour {
	
	public bool highScore = false;
	public GUIText heightGuiText;
	private string baseText = "Height: ";
	public Transform rock;
	// Use this for initialization
	void Start () 
	{
		if( heightGuiText == null )
			heightGuiText = gameObject.GetComponent<GUIText>();
		
		baseText = heightGuiText.text;
		
		if( rock == null )
			rock = GameObject.FindGameObjectWithTag("Rock").transform;
		
		
	}
	
	private int lastHeight = -1;
	private string lastHeightString;
	// Update is called once per frame
	void Update () {
		int newHeight = Mathf.Max(Mathf.RoundToInt( rock.position.y ), RockController.HIGHEST_HEIGHT);
		if( newHeight != lastHeight )
		{
			heightGuiText.fontStyle = (newHeight > RockController.HIGHEST_HEIGHT) ? FontStyle.BoldAndItalic : FontStyle.Normal;
			heightGuiText.text = baseText + newHeight.ToString();
		}
	}
}
