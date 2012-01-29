using UnityEngine;
using System.Collections;

public class SisyPhyst : MonoBehaviour {
	public GameObject rock;
	
	public const int DOWN = 0, UP = 1, LEFT = 2, RIGHT = 3;
	public static bool[] directions = {false, false, false, false};
	private static bool[] directionsDown = {false, false, false, false};
//	private static float[] cooldownTime = {0.0f, 0.0f, 0.0f, 0.0f};
	private static KeyCode[] keycodes = {KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow};
//	public Vector3 basePunch	= new Vector3( 50.0f, 100.0f);
//	KeyCode.
	
	public Vector3 downForce 		= new Vector3( 50.0f, -15.0f  );
	public Vector3 punchForce 		= new Vector3(750.0f, 15.0f   );
	public Vector3 uppercutForce 	= new Vector3( 25.0f, 1000.0f );
	
	public float braceDistance	= 5.0f;
	public float walkForceMult	= 150.0f;
	public float dashForce		= 1500.0f;
	
	private float baseMass 		= 100.0f;
//	private float massOffset	= 1.0f;
	
	private float massTime		= 0.0f;
	private float massDuration	= 0.0f;
	
	
	
	private bool GetButton( int direction )
	{
//		if( direction == LEFT && Input.touchCount > 2 )
//			return true;
		return directions[direction] || Input.GetKey( keycodes[direction] );
	}
	
//	private bool touchDown = false;
	private bool GetButtonDown( int direction )
	{
//		if( !touchDown && direction == LEFT && Input.touchCount > 0 )
//		{
//			touchDown = true;
//			return true;
//		}
//		else
//			touchDown = false;
		bool rtnVal = directions[direction];
		if( rtnVal ) //true and not reported yet )
		{
			float theTime = Time.time;
			if( directionsDown[direction]  ) //was already down || theTime > cooldownTime[direction]
				rtnVal = false;
			else
			{
				directionsDown[direction] = true; //going down for the first time
//				cooldownTime[direction] = Time.time + 0.267f; //just over one frame ideally
			}
		}
		else 
			directionsDown[direction] = false; //reset for next pull
		
		return rtnVal || Input.GetKeyDown( keycodes[direction] );
	}
	// Use this for initialization
	private float lightIntensity, lightRange;
	
	private GameObject ground;
	void Start () {
		baseMass = rigidbody.mass;
		
		lightIntensity = light.intensity;
		lightRange = light.range;
		if( rock == null )
			rock = GameObject.FindGameObjectWithTag("Rock");
	}
	
	private void pumpMass( float amount, float time )
	{
		rigidbody.mass *= amount;
		
		if( massTime < Time.time )
		{
			massTime = Time.time + time;
			massDuration = time;
		}
		else
		{
			massTime += time;
			massDuration += time;
		}
	}
	
	private Vector3 toRock;
	private float jumpTime = 0.0f;
	// Update is called once per frame
	void FixedUpdate()
	{
		float massTimeLeft = massTime - Time.time;
		if( massTimeLeft > 0.0f )
		{
			rigidbody.mass = Mathf.SmoothStep( rigidbody.mass, baseMass, 1.0f - (massTimeLeft / massDuration) );
		}
		else if( rigidbody.mass != baseMass )
		{
			rigidbody.mass = baseMass;
//			massOffset = 1.0f;
			massDuration = 0.0f;
		}
		
		if( GetButton( DOWN ) ) //Input.GetKey( KeyCode.DownArrow ) )
			pumpMass( 1.015f, 0.07f );
		
		if( toRock != Vector3.zero )
			rigidbody.AddForce( toRock, ForceMode.Force );
		
		
		float overrun = rigidbody.transform.position.x - rock.rigidbody.transform.position.x;
		if( overrun > 0.0f && rigidbody.velocity.magnitude < 10.0f )
		{
			rigidbody.AddForce(-1.0f * (overrun * overrun), -1.0f * overrun, 0.0f, ForceMode.Force);
		}
		
		if( GetButton( LEFT ) )//Input.GetKey( KeyCode.LeftArrow ) )
		{
			rigidbody.AddForce( (rock.transform.position - transform.position), ForceMode.Force );
		}
	}
	
	public float jumpForce = 1.5f;
	public float leftForce = 3.25f;
	void Update () 
	{	
		toRock = Vector3.zero;
		
//		bool onLeft = transform.position.x < rock.transform.position.x;
		
		
		if( GetButtonDown( DOWN ) )//( KeyCode.DownArrow ) )
		{
			pumpMass( 1.1f, 0.27f );
			if( rigidbody.velocity.x >= 0.0f )
				rigidbody.AddForce( downForce, ForceMode.Impulse );
			else
				rigidbody.AddForce( downForce.x * -0.25f, downForce.y, downForce.z, ForceMode.Impulse );
		}
		else
		{
			if( GetButtonDown( LEFT ) )//Input.GetKeyDown( KeyCode.LeftArrow ) )
			{
				pumpMass( 8.1f, 0.4f );
				rigidbody.AddForce( dashForce * (rock.transform.position - transform.position), ForceMode.Impulse );
			} 
			else if( GetButtonDown( RIGHT ) )//Input.GetKeyDown(KeyCode.RightArrow) )
			{
				rigidbody.AddForce( punchForce * Mathf.Sqrt(rigidbody.mass / baseMass), ForceMode.Impulse );
				pumpMass( 3.4f, 0.2f );
			}
			else if( (Time.time > jumpTime) && GetButtonDown( UP ) )//( KeyCode.UpArrow ) )
			{
				jumpTime = Time.time + 0.47f;
				pumpMass( 105.0f, 0.26f );
				rigidbody.AddForce( uppercutForce, ForceMode.Impulse );
			}
			else
			{
				toRock = rock.transform.position - transform.position;
//				Debug.Log("ToRock " + toRock);
				if( toRock.magnitude < braceDistance )
				{
					if( toRock.x > 0.0f )
					{
						pumpMass(1.005f, 0.07f);
						toRock *= (rigidbody.mass);
					}
					else if(rigidbody.velocity.x < 0.0f)
					{
						if( toRock.y > -1.25f  )
							toRock = (Vector3.up * rigidbody.mass * jumpForce) * walkForceMult;
						
						toRock += Vector3.left * rigidbody.mass * leftForce;
					}
				}
				else if( onGround )
				{
					toRock.Normalize();
					toRock *= (rigidbody.mass + rock.rigidbody.mass);
				}
				else
				{
					toRock.Normalize();
					toRock *= walkForceMult;
				}
			}
		}
		
		float massOffset = rigidbody.mass / baseMass;
		if( lastMassAdjust != massOffset )
		{
			lastMassAdjust = massOffset;
			massOffset = Mathf.Sqrt(massOffset);
			light.intensity = lightIntensity * massOffset;
			light.range = lightRange * massOffset;
		}
	}
	
	public void Kill()
	{
			Destroy(this); //kill our duder
			rigidbody.constraints = RigidbodyConstraints.None;
	}
	
	private bool onGround = false;
	
	void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.CompareTag("Rock") )
        {    
            onGround = true;
            audio.Play();
        }
        //
    }
	
	
	void OnCollisionExit(Collision collision)
	{
		if( collision.gameObject.CompareTag("Rock") )
			onGround = false;
	}
	
	private float lastMassAdjust = 1.0f;
}
