using UnityEngine;
using System.Collections;

public class LoseGUI : MonoBehaviour {
	
	private int oldHighScore = -1;
	
	// Use this for initialization
	void Start () {
		oldHighScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		
	}
}
