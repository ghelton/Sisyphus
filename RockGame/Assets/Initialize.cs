using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]
public class Initialize : MonoBehaviour {
	
	void Awake()
	{
		//first
		startingPosition = transform.position;
	}
	
	
	private GameObject rock;
	private Vector3 startingPosition;
	// Use this for initialization
	void Start () {
		//second
		rock = GameObject.FindGameObjectWithTag("Rock");
	}
	
	// Update is called once per frame
	void Update () {
		
		//third
		
		
		float distance = Vector3.Distance(transform.position, rock.transform.position);
		if( distance > 15.0f )
		{
			Vector3 targetPosition = Vector3.Slerp(transform.position
			                                       , Vector3.MoveTowards(rock.transform.position, startingPosition, 15.0f)
			                                       , 0.1f);
				
			gameObject.transform.position = targetPosition;
		}
		
		transform.LookAt(rock.transform.position);
//		GameObject.
//		
//		foreach( GameObject gameObj in GameObject.FindObjectsWithTag(typeof(GameObject)))
//		{
//			float distance = Vector3.Distance(gameObj.transform.position, gameObject.transform.position);
//			if( distance < 10.0f )
//				
//		}
	}
}
