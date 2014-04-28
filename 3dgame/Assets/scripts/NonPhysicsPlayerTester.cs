using UnityEngine;
using System.Collections;


public class NonPhysicsPlayerTester : MonoBehaviour
{

	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f;
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;




	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();


		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{

		if( hit.normal.y == 1f )
			return;



	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
		if (col.gameObject.tag == "DEATH")
			Application.LoadLevel (Application.loadedLevel);
		if (col.gameObject.name == "ENDLEVEL2")
			Application.LoadLevel ("level3");
		if (col.gameObject.name == "ENDLEVEL3")
			Application.LoadLevel ("level4");
		if (col.gameObject.name == "STARTOVER")
			Application.LoadLevel ("level2");

	}



	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion



	void Update()
	{

		_velocity = _controller.velocity;

		if( _controller.isGrounded )
			_velocity.y = 0;

		if( Input.GetKey( KeyCode.D ) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else if( Input.GetKey( KeyCode.A ) )
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Idle" ) );
		}



		if( _controller.isGrounded && (Input.GetKeyDown( KeyCode.Space ) || Input.GetKeyDown ( KeyCode.W )) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}


		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; 
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );


		_velocity.y += gravity * Time.deltaTime;

		_controller.move( _velocity * Time.deltaTime );
	}

}
