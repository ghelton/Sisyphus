using UnityEngine;
using System.Collections;

public class SisyPhyst : MonoBehaviour {
	public GameObject rock;
	
//	public Vector3 basePunch	= new Vector3( 50.0f, 100.0f);
	
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
	
	// Use this for initialization
	private float lightIntensity, lightRange;
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
	
	private float jumpTime = 0.0f;
	// Update is called once per frame
	void Update () {
//		if( Time.time > jumpTime )
//		{
//			rigidbody.AddForce(0.0f, 0.5f, 0.0f, ForceMode.Impulse);
//			jumpTime += 2.0f;
//		}
		
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
		
		float overrun = rigidbody.transform.position.x - rock.rigidbody.transform.position.x;
		if( overrun > 0.0f )
		{
			Vector3 adjustedVelocity = rigidbody.velocity;
			adjustedVelocity.x -= Mathf.Sqrt(overrun);
			
			rigidbody.velocity = adjustedVelocity;
		}
		
		if( Input.GetKeyDown( KeyCode.DownArrow ) )
		{
			pumpMass( 20.0f, 0.27f );
		}
		else if( Input.GetKeyUp( KeyCode.DownArrow ) )
		{
			rigidbody.mass /= 20.0f;
		}
		else if( Input.GetKey( KeyCode.DownArrow ) )
			rigidbody.AddForce( downForce, ForceMode.Force );
		else
		{
			if( Input.GetKeyDown( KeyCode.F ) )
			{
				pumpMass( 1.1f, 0.4f );
				rigidbody.AddForce( dashForce * (rock.transform.position - transform.position), ForceMode.Impulse );
			}
			else if( Input.GetKeyDown(KeyCode.RightArrow) )
			{
				pumpMass( 1.4f, 0.2f );
				rigidbody.AddForce( punchForce, ForceMode.Impulse );
			}
			else if( Input.GetKeyDown( KeyCode.UpArrow ) )
			{
				pumpMass( 1.6f, 0.6f );
				rigidbody.AddForce( uppercutForce, ForceMode.Impulse );
			}
			else
			{
				Vector3 toRock = rock.rigidbody.position - rigidbody.position;
				if( toRock.magnitude > braceDistance )
				{
					toRock *= walkForceMult;
				}
				else{
					toRock.Normalize();
					toRock *= rigidbody.mass * 10.0f;
				}
				
				rigidbody.AddForce( toRock, ForceMode.Force );
			}
		}
		
		float massOffset = rigidbody.mass / baseMass;
		if( lastMassAdjust != massOffset )
		{
			light.intensity = lightIntensity * massOffset;
			light.range = lightRange * massOffset;
			lastMassAdjust = massOffset;
		}
	}
	
	public void Kill()
	{
			Destroy(this); //kill our duder
			rigidbody.constraints = RigidbodyConstraints.None;
	}
	
	private float lastMassAdjust = 1.0f;
}
