using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {
	public float moveSpeed;
	private Vector3 moveDirection;
	public float turnSpeed;
	[SerializeField]
	private PolygonCollider2D[] colliders;
	private int currentColliderIndex = 0;
	public void SetColliderForSprite( int spriteNum )
	{
		colliders[currentColliderIndex].enabled = false;
		currentColliderIndex = spriteNum;
		colliders[currentColliderIndex].enabled = true;
	}
	void OnTriggerEnter2D( Collider2D other)
	{
		Debug.Log ("Hit " + other.gameObject);
	}
	private void EnforceBounds()
	{
		// 1
		Vector3 newPosition = transform.position; 
		Camera mainCamera = Camera.main;
		Vector3 cameraPosition = mainCamera.transform.position;
		
		// 2
		float xDist = mainCamera.aspect * mainCamera.orthographicSize; 
		float xMax = cameraPosition.x + xDist;
		float xMin = cameraPosition.x - xDist;
		
		// 3
		if ( newPosition.x < xMin || newPosition.x > xMax ) {
			newPosition.x = Mathf.Clamp( newPosition.x, xMin, xMax );
			moveDirection.x = -moveDirection.x;
		}
		// TODO vertical bounds
		
		// 4
		transform.position = newPosition;
	}
	// Use this for initialization
	void Start () {
		moveDirection = Vector3.right;

	
	}
	
	// Update is called once per frame
	void Update () {
		// 1
		Vector3 currentPosition = transform.position;
		// 2
		if( Input.GetButton("Fire1") ) {
			// 3
			Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			// 4
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();
		}
		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );
		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		transform.rotation = 
			Quaternion.Slerp( transform.rotation, 
			                 Quaternion.Euler( 0, 0, targetAngle ), 
			                 turnSpeed * Time.deltaTime );
		EnforceBounds();

	}
}
