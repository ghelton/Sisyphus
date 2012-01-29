using UnityEngine;
using System.Collections;

public class SisyPhyst : MonoBehaviour {
	public GameObject rock;
	private Transform peakTransform;
	
	public const int DOWN = 0, LEFT = 1, UP = 2, RIGHT = 3;
	public static bool[] directions = {false, false, false, false};
	private static bool[] directionsDown = {false, false, false, false};
//	private static float[] cooldownTime = {0.0f, 0.0f, 0.0f, 0.0f};
	private static KeyCode[] keycodes = {KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow};
	
	
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
	
	
	public float[] statLevelRate = {1.1f, 1.05f};
	
	private const int STAT_STRENGTH  	= 0;
	private const int STAT_SPEED		= 1;
	
	
	
	public float[] stats 		= {1.0f	, 1.0f};
	public float[] maxStats 	= {3.0f	, 3.0f};
	public float[] levelRate 	= {1.05f, 1.01f};
	public float[] statEffect	= {1.25f, 1.5f};
	
	
	public int maxLevels			= 3;
	public float[] strengthLevels 	= {10.0f, 25.0f, 50.0f};
	public float[] speedLevels 		= {12.5f, 27.5f, 55.0f};
	
	private float[][] levelThresholds = {null, null};
	
	private int[] statLevels = {1, 1};
	
	private int jumpsLeft = 1;
	
	private GameObject theDude;
	
	void Awake()
	{
		levelThresholds[STAT_SPEED] 	= speedLevels;
		levelThresholds[STAT_STRENGTH] 	= strengthLevels;
		
		
		int count;
		for( count = statLevels.Length - 1; count >= 0; count-- )
			statLevels[count] = 1;
		
		peakTransform = peak.transform;
	}
	
	public GameObject peak;
	public Vector3[] directionVelocities;
	
	private void statAction( int action, float pumpBy )
	{
		int actionLevel = statLevels[action];
		float desiredIntensity = Mathf.Pow(statEffect[action], actionLevel - 1) * (rigidbody.mass / baseMass);
		
		Vector3 rockPosition = rock.transform.position;
		Vector3 toRock = rockPosition - transform.position;
		
		bool leftOfRock = Vector3.Distance(peakTransform.position, rockPosition) < Vector3.Distance( peakTransform.position, transform.position );
		bool belowRock = toRock.y > 0.0f;
		Debug.Log("StatAction left " + leftOfRock.ToString() + " below rock: " + belowRock.ToString());
		
		if( action == STAT_SPEED )
		{
		 Debug.Log("Go Left");	
//			if( !leftOfRock ) 
//			{
				pumpMass( pumpBy, 0.1f );
				Vector3 theForce =  dashForce * desiredIntensity * toRock;
				if( belowRock && !leftOfRock )
					theForce = theForce + (Vector3.up * desiredIntensity);
				rigidbody.AddForce(theForce, ForceMode.Impulse );
//			}
		}
		else
		{
			
		}
		
//		Vector3 velocity = directionVelocities[direction];
		
//		if( left )
//			velocity.x *= -1.0f;
		
		
//		stats[action] *= levelRate[action];
		
//		float[] statLevelThresholds = levelThresholds[action];
//		if( statLevelThresholds.Length < actionLevel && stats[action] > statLevelThresholds[actionLevel] )
//		{ //levelup
//			statLevels[action]++;
//		}
	}
	
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
//			float theTime = Time.time;
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
		projectedMass = rigidbody.mass;
		
		theDude = GameObject.FindGameObjectWithTag("Avatar");
		if( theDude == null )
			theDude = transform.GetChild(0).gameObject;
		
		baseMass = rigidbody.mass;
		
		lightIntensity = light.intensity;
		lightRange = light.range;
		if( rock == null )
			rock = GameObject.FindGameObjectWithTag("Rock");
	}
	
	private float projectedMass;
	private void pumpMass( float amount, float time )
	{
		rigidbody.mass *= amount;
//		projectedMass *= amount;
		
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
	
	private bool facingRight = true;
	
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
		
		if( toRock != Vector3.zero ) {
			rigidbody.AddForce( toRock, ForceMode.Force );
			
			if( (toRock.x > 0.0f) != facingRight )
			{
//				Debug.Log("Flipping dude");
				iTween.ScaleBy(theDude, new Vector3(-1.0f, 1.0f, 1.0f), 0.47f);
//				iTween.RotateAdd(theDude, new Vector3(180.0f, 0.0f), 0.37f);
				facingRight = !facingRight;
			}
		}
		
		
		
		float overrun = rigidbody.transform.position.x - rock.rigidbody.transform.position.x;
		if( overrun > 0.0f && rigidbody.velocity.magnitude < 10.0f )
		{
			rigidbody.AddForce(-1.0f * (overrun * overrun), -1.0f * overrun, 0.0f, ForceMode.Force);
		}
		
//		if( GetButton( LEFT ) )//Input.GetKey( KeyCode.LeftArrow ) )
//		{
//			rigidbody.AddForce( (rock.transform.position - transform.position), ForceMode.Force );
//		}
	}
	
	public float jumpForce = 1.5f;
	public float leftForce = 3.25f;
	void Update () 
	{	
		toRock = Vector3.zero;
		
//		bool onLeft = transform.position.x < rock.transform.position.x;
		
		
//		if( GetButtonDown( LEFT ) )
//		{
//			statAction( STAT_SPEED, 1.0f );
//		}
//		else if( GetButtonDown( RIGHT ) )
//		{
//			
//		}
		
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
//				pumpMass( 8.1f, 0.4f );
				statAction( STAT_SPEED, 8.1f );
//				rigidbody.AddForce( dashForce * (rock.transform.position - transform.position), ForceMode.Impulse );
			} 
			else if( GetButtonDown( RIGHT ) )//Input.GetKeyDown(KeyCode.RightArrow) )
			{
				rigidbody.AddForce( punchForce * Mathf.Sqrt(rigidbody.mass / baseMass), ForceMode.Impulse );
				pumpMass( 3.4f, 0.2f );
			}
			else if( (Time.time > jumpTime) && GetButtonDown( UP ) )//( KeyCode.UpArrow ) )
			{
				jumpTime = Time.time + 0.47f;
				pumpMass( 15.0f, 0.26f );
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
//		float massOffset = projectedMass / baseMass;
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
			jumpsLeft = statLevels[STAT_SPEED];
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
