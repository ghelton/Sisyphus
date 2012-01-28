using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour {
	public static int HIGHEST_HEIGHT = 0;
	
	public ParticleEmitter sissyPhystCollideParticle = null;
	public ParticleEmitter sissyPhystStayParticle = null;
	public ParticleEmitter groundCollideParticle = null;
	
	public AudioClip collideSound 	= null;
	public AudioClip rollSound 		= null;
	
	
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
	
	void OnCollisionEnter( Collision collider )
	{
		
		ParticleEmitter particles = null;
		switch( collider.gameObject.tag )
		{
			case "Ground":
					particles = groundCollideParticle;
					audio.Play();
//					audio.enabled = true;
				break;
				
			case "SisyPhyst":
					particles = sissyPhystCollideParticle;
				break;
			
		}
		
		if( collider.gameObject.CompareTag("Ground") )
			particles = groundCollideParticle;
		
		if( particles != null )
		{
			Debug.Log("Drawing particles " + particles.name);
			Vector3 contactPoint = collider.contacts[0].point;
			
			Quaternion contactDirection = Quaternion.LookRotation( collider.contacts[0].normal );
			
			ParticleEmitter.Instantiate( particles, contactPoint, contactDirection );
		}
	}
	
	void OnCollisionExit( Collision collider )
	{
		if( collider.gameObject.CompareTag("Ground") )
			audio.Stop();
	}
}
