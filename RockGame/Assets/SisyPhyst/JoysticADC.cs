using UnityEngine;
using System.Collections;


//using System.IO.

public class JoysticADC : MonoBehaviour {
	public Joystick joystick;
//	private Joystick ohJoy;
	// Use this for initialization
//	void Start () {
////		ohJoy = joystick.GetComponent<Joystick>();
//	}
	
	void Start()
	{
#if !UNITY_IPHONE && !UNITY_ANDROID
//		joystick = GameObject.FindGameObjectWithTag("Control");
//		joystick.active = false;
		Destroy(gameObject);
#endif
	}
	
	public float threshold = 0.2f;
	void Update()
	{
		CheckKeys();
	}
	
	void LateUpdate()
	{
		CheckKeys();
	}
	// Update is called once per frame
	void CheckKeys () 
	{
		SisyPhyst.directions[SisyPhyst.RIGHT] = (joystick.position.x > threshold);
		SisyPhyst.directions[SisyPhyst.LEFT] = ( joystick.position.x < -threshold );
		SisyPhyst.directions[SisyPhyst.UP] = ( joystick.position.y > threshold );
		SisyPhyst.directions[SisyPhyst.DOWN] = ( joystick.position.y < -threshold );
	}
}
