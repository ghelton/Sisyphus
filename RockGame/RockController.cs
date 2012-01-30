using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour {
	public static int HIGHEST_HEIGHT = 0;
	
	
	public ParticleEmitter sissyPhystCollideParticle = null;
	public ParticleEmitter sissyPhystStayParticle = null;
	public ParticleEmitter groundCollideParticle = null;
	
	public AudioClip collideSound 	= null;
	public AudioClip rollSound 		= null;
	
	public GameObject mountain;
	
	public Vector3 baseUpperCut = new Vector3(0.0f, 500.0f);
	public Vector3 basePunch	= new Vector3(50.0f, 100.0f);
	public Vector3 baseDropKick	= new Vector3(25.0f, -100.0f);
	
	public GameObject sissyPhyst;
	
	public float attackRange = 4.0f;
	
	// Use this for initialization
	void Start () {
		if( sissyPhyst == null )
			sissyPhyst = GameObject.FindGameObjectWithTag("SisyPhyst");
		
		HIGHEST_HEIGHT = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Vector3.Distance( sissyPhyst.rigidbody.position, rigidbody.position ) < attackRange )
		{
			if( Input.GetKeyDown(KeyCode.UpArrow) )
				rigidbody.AddForceAtPosition( baseUpperCut, sissyPhyst.rigidbody.position, ForceMode.Impulse );
			
		}
		
		if( (int)transform.position.y > HIGHEST_HEIGHT )
			HIGHEST_HEIGHT = (int)transform.position.y;
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if( collider.gameObject.CompareTag("Cliff") )
		{
			sissyPhyst.GetComponent<SisyPhyst>().Kill();
			Time.timeScale = 0.1f;
			Camera.mainCamera.gameObject.AddComponent<LoseGUI>();
		}
	}
	
	public float mountainShakeThreshold = 1.5f;
	public Vector3 mountainShakeAmount = new Vector3(0.02f, 0.02f, 0.01f);
	void OnCollisionEnter( Collision collision )
	{
		
		
#if !UNITY_IPHONE && !UNITY_ANDROID
		
		ParticleEmitter particles = null;
		switch( collider.gameObject.tag )
		{
			case "SisyPhyst":
					particles = sissyPhystCollideParticle;
				break;
			
		}
		
		if( collider.gameObject.CompareTag("Ground") )
		{
			particles = groundCollideParticle;
			audio.enabled = true;
			audio.Play();
			
			if( mountain )
			{
	
				float sqrMag = collision.relativeVelocity.sqrMagnitude;
				if( sqrMag > mountainShakeThreshold )
				{
					Debug.Log("Shaking mountain");
					Vector3 shake = sqrMag * (mountainShakeAmount);
					iTween.ShakePosition( mountain, shake, 0.17f * Mathf.Log(sqrMag) );
				}
			}
		}
		
		if( particles != null )
		{
//			Debug.Log("Drawing particles " + particles.name);
			Vector3 contactPoint = collision.contacts[0].point;
			
			Quaternion contactDirection = Quaternion.Euler( collision.contacts[0].normal );
			
			ParticleEmitter.Instantiate( particles, contactPoint, contactDirection );
		}
#endif
	}
	
#if !UNITY_IPHONE && !UNITY_ANDROID
	void OnCollisionStay( Collision collision )
	{
		if( collision.gameObject.Equals(sissyPhyst) )// sissyPhystStayParticle != null )
			ParticleEmitter.Instantiate(sissyPhystStayParticle, collision.contacts[0].point, Quaternion.identity);
	}
#endif
	
//	void OnCollisionExit( Collision collider )
//	{
//		if( collider.gameObject.CompareTag("Ground") )
//			audio.Stop();
//	}
}
