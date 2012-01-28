using UnityEngine;
using System.Collections;

public class HeightLabel : MonoBehaviour {
	public GUIText heightGuiText;
	private string baseText;
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
		int newHeight = Mathf.RoundToInt( rock.position.y );
		if( newHeight != lastHeight )
		{
			heightGuiText.text = baseText + newHeight.ToString();
		}
	}
}
