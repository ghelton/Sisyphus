using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]
public class Initialize : MonoBehaviour {
	
	
	public Shader simpleShader;
	public Material[] swapToSimpleShadersOnIphone;
	private Shader[] oldShaders;
	
	
	public Vector3 cameraOffset = new Vector3( 10.0f, 5.0f, -10.0f );
	public float angleDown = 5.0f;
	
	private GameObject rock;
#if UNITY_IPHONE || UNITY_ANDROID
	void Awake ()
	{
		if( simpleShader == null )
			simpleShader = Shader.Find("Mobile/Diffuse");
		
		if( simpleShader != null )
		{
			oldShaders = new Shader[swapToSimpleShadersOnIphone.Length];
			for( int count = swapToSimpleShadersOnIphone.Length - 1; count >= 0; count-- ) {
				oldShaders[count] = swapToSimpleShadersOnIphone[count].shader;
				swapToSimpleShadersOnIphone[count].shader = simpleShader;
			}
		}
		
		RenderSettings.fog = false;
	}
	
#endif
//	private Vector3 startingPosition;
	// Use this for initialization
	void Start () {
		//second
		rock = GameObject.FindGameObjectWithTag("Rock");
		
		transform.position = transform.position + cameraOffset;
//		transform.rotation.SetLookRotation( rock.transform.position
		transform.Rotate(angleDown, 0.0f, 0.0f);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//third
		
		transform.position = rock.transform.position - cameraOffset;
		
//		float distance = Vector3.Distance(transform.position, rock.transform.position);
//		if( distance > 15.0f )
//		{
//			Vector3 targetPosition = Vector3.Slerp(transform.position
//			                                       , Vector3.MoveTowards(rock.transform.position, startingPosition, 15.0f)
//			                                       , 0.1f);
//				
//			gameObject.transform.position = targetPosition;
//		}
		
//		transform.LookAt(rock.transform.position);
//		GameObject.
//		
//		foreach( GameObject gameObj in GameObject.FindObjectsWithTag(typeof(GameObject)))
//		{
//			float distance = Vector3.Distance(gameObj.transform.position, gameObject.transform.position);
//			if( distance < 10.0f )
//				
//		}
	}
	
#if UNITY_IPHONE || UNITY_ANDROID
	void OnDestroy()
	{
		for( int count = swapToSimpleShadersOnIphone.Length - 1; count >= 0; count-- )
			swapToSimpleShadersOnIphone[count].shader = oldShaders[count];
		RenderSettings.fog = true;
	}
#endif
}
