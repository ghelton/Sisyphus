using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour {
	public ParticleEmitter sissyPhystCollideParticle = null;
	public ParticleEmitter sissyPhystStayParticle = null;
	public ParticleEmitter groundCollideParticle = null;
	
	public AudioClip collideSound = null;
	
	
	public Vector3 baseUpperCut = new Vector3(0.0f, 500.0f);
	public Vector3 basePunch	= new Vector3(50.0f, 100.0f);
	public Vector3 baseDropKick	= new Vector3(25.0f, -100.0f);
	
	public GameObject sissyPhyst;
	
	public float attackRange = 4.0f;
	
	// Use this for initialization
	void Start () {
		if( sissyPhyst == null )
			sissyPhyst = GameObject.FindGameObjectWithTag("SisyPhyst");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Vector3.Distance( sissyPhyst.rigidbody.position, rigidbody.position ) < attackRange )
		{
			if( Input.GetKeyDown(KeyCode.UpArrow) )
				rigidbody.AddForceAtPosition( baseUpperCut, sissyPhyst.rigidbody.position, ForceMode.Impulse );
			
		}
	}
	
	void OnTriggerEnter(Collision collider)
	{
		if( collider.gameObject.CompareTag("Cliff") )
		{
			sissyPhyst.GetComponent<SisyPhyst>().Kill();
		}
	}
	
	void OnCollisionEnter( Collision collider )
	{
		Vector3 contactPoint = collider.contacts[0];
		Quaternion contactDirection = Quaternion.LookRotation( contactPoint - collider.gameObject.transform.position );
		switch( collider.gameObject.tag )
		{
		case "Ground":
				if( groundCollideParticle != null )
					GameObject.Instantiate( groundCollideParticle, contactPoint, contactDirection );
			break;
			
		case "SisyPhyst":
				
			break;
			
		}
	}
}
