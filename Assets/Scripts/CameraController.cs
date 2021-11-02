using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	#region properties (private)
	
	[SerializeField] private float distanceAway;
	[SerializeField] private float distanceUp;
	[SerializeField] private float smooth;
	[SerializeField] private Transform follow;
	
	private Vector3 targetPosition;
	
	#endregion
	
	#region properties (private)
	
	#endregion
	
	#region unity event functions
	
    // Start is called before the first frame update
    void Start()
    {
		
		follow = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	#endregion
	
	#region methods
	
	void LateUpdate()
	{
		// setting the target position
		targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
		
		Debug.DrawRay(follow.position, Vector3.up * distanceUp, Color.red);
		Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
		Debug.DrawRay(follow.position, targetPosition, Color.magenta);
		
		// making a smooth transition from it's current position a the position it wants to be in
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
		
		// make camera follow player
		transform.LookAt(follow);
	}
	
	#endregion
}
