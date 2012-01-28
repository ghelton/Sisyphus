using UnityEngine;
using System.Collections;

public class LoseGUI : MonoBehaviour {
	
	private int oldHighScore = -1;
	
	// Use this for initialization
	void Start () {
		oldHighScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
		
		if( oldHighScore < RockController.HIGHEST_HEIGHT )
			PlayerPrefs.SetInt("HighScore", RockController.HIGHEST_HEIGHT);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUI.Label( new Rect( (Screen.width / 2) - 50.0f, (Screen.height / 2) - 25.0f, 100.0f, 50.0f ), "Oh noes uz died" );
		
		if( oldHighScore < RockController.HIGHEST_HEIGHT )
			GUI.Label( new Rect( (Screen.width / 2) - 50.0f
			                    , (Screen.height / 2) - 25.0f
			                    , 100.0f, 50.0f )
			          , "But you got a new high score of " + RockController.HIGHEST_HEIGHT );
	}
}
