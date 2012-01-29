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
		int rockPos = Mathf.RoundToInt( rock.position.y );
		int newHeight = highScore ? Mathf.Max(rockPos, RockController.HIGHEST_HEIGHT) : rockPos;
		if( newHeight != lastHeight )
		{
			heightGuiText.fontStyle = (highScore && newHeight > RockController.HIGHEST_HEIGHT) ? FontStyle.BoldAndItalic : FontStyle.Normal;
			heightGuiText.text = baseText + newHeight.ToString();
		}
	}
}
