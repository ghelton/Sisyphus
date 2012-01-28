using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour {
	
	public Vector3 baseUpperCut = new Vector3(0.0f, 500.0f);
	public Vector3 basePunch	= new Vector3(50.0f, 100.0f);
	public Vector3 baseDropKick	= new Vector3(25.0f, -100.0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if( Input.GetKeyDown(KeyCode.UpArrow) )
			rigidbody.AddForce( baseUpperCut, ForceMode.Impulse );
		if( Input.GetKeyDown(KeyCode.RightArrow) )
			rigidbody.AddForce( basePunch, ForceMode.Impulse );
		if( Input.GetKeyDown(KeyCode.DownArrow) )
			rigidbody.AddForce( baseDropKick, ForceMode.Impulse );
	}
}
