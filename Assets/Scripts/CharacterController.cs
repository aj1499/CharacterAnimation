using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	#region properties (private)
	
	[SerializeField] private float directionDampTime = .25f;
	[SerializeField] private Camera gamecam;
	[SerializeField] private float directionSpeed = 3.0f;
	[SerializeField] private float rotationDegreePerSecond = 120f;
	private Animator anim;
	
	private float speed = 0.0f;
	private float direction = 0.0f;
	private float h = 0.0f;
	private float v = 0.0f;
	private AnimatorStateInfo stateInfo;
	
    private int hashLocomotionId = 0;
	
	#endregion
	
	#region properties (private)
	
	#endregion
	
	#region unity event functions
	
	// Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
		
		if (anim.layerCount >= 2) {
			anim.SetLayerWeight(1, 1);
		}
		
		 hashLocomotionId = Animator.StringToHash("Base Layer.Locomotion");
    }

    // Update is called once per frame
    void Update()
    {
		if (anim)
		{
			stateInfo = anim.GetCurrentAnimatorStateInfo(0);
			
			// Pull values from controller/keyboard
			v = Input.GetAxis("Vertical");
			h = Input.GetAxis("Horizontal");
			
			// Translate controls stick coordinates into world/cam/character space
            StickToWorldspace(this.transform, gamecam.transform, ref direction, ref speed);	
			
			anim.SetFloat("Speed", speed);
			anim.SetFloat("Direcction", h, directionDampTime, Time.deltaTime);
		}
    }
	
	void FixedUpdate()
	{
		// Rotate character model right or left, but only if character is moving in that direction
		if (isInLocomotion() && ((direction >= 0 && h >= 0) || (direction < 0 && h < 0)))
		{
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (h < 0f ? -1f : 1f), 0f), Mathf.Abs(h));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			this.transform.rotation = (this.transform.rotation * deltaRotation);
		}
	}
	
	
	#endregion
	
	#region methods
	
	public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
    {
        Vector3 rootDirection = root.forward;
				
        Vector3 stickDirection = new Vector3(h, 0, v);
		
		speedOut = stickDirection.sqrMagnitude;		

        // Get camera rotation
        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(CameraDirection));

        // Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
		
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.5f, root.position.z), axisSign, Color.red);
		
		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		
		angleRootToMove /= 180f;
		
		directionOut = angleRootToMove * directionSpeed;
	}	
	
	public bool isInLocomotion()
    {
        return stateInfo.nameHash == hashLocomotionId;
    }
	
	#endregion
}
