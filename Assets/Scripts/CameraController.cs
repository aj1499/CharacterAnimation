using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	#region properties (private)
	
	[SerializeField] private float distanceAway;
	[SerializeField] private float distanceUp;
	[SerializeField] private float smooth;
	[SerializeField] private Transform followXform;
	[SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, 0f);
	
	[SerializeField] private float camSmoothDampTime = 0.1f;
	private Vector3 velocityCamSmooth = Vector3.zero;	
	
	private Vector3 targetPosition;
	private Vector3 lookDir;
	
	#endregion
	
	#region properties (private)
	
	#endregion
	
	#region unity event functions
	
    // Start is called before the first frame update
    void Start()
    {
		
		followXform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	#endregion
	
	#region methods
	
	void LateUpdate()
	{
		Vector3 characterOffset = followXform.position + offset;
		
		lookDir = characterOffset - this.transform.position;
		lookDir.y = 0;
		lookDir.Normalize();
		Debug.DrawRay(this.transform.position, lookDir, Color.green);
		
		// setting the target position
		targetPosition = characterOffset + followXform.up * distanceUp - lookDir * distanceAway;
		
		// Debug.DrawRay(followXform.position, Vector3.up * distanceUp, Color.red);
		// Debug.DrawRay(followXform.position, -1f * followXform.forward * distanceAway, Color.blue);
		Debug.DrawRay(followXform.position, targetPosition, Color.magenta);
		
		compensateForWalls(characterOffset, ref targetPosition);
		
		// making a smooth transition from it's current position a the position it wants to be in
		smoothPosition(this.transform.position, targetPosition);
		
		// make camera followXform player
		transform.LookAt(followXform);
	}
	
	private void smoothPosition(Vector3 fromPos, Vector3 toPos)
	{
		// Making a smooth transition between camera's current position and the position it wants to be in
		this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}
	
	private void compensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
	{
		Debug.DrawLine(fromObject, toTarget, Color.cyan);
		// compensate for walls between camera
		RaycastHit wallHit = new RaycastHit();
		if (Physics.Linecast(fromObject, toTarget, out wallHit))
		{
			Debug.DrawLine(wallHit.point, Vector3.left, Color.red);
			toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
		}
	}
	
	#endregion
}
