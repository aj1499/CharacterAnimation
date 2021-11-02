using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerLogic : MonoBehaviour
{
	[SerializeField] private float directionDampTime = .25f;
	[SerializeField] private float rotationDegreePerSecond = 120f;
	[SerializeField] private float movementTresHold = 0.2f;
	
	private Animator anim;
	private CharacterController controller;
	
	[SerializeField] private float speed = 0.0f;
	private float h = 0.0f;
	private float v = 0.0f;
	private Vector3 moveDirection;
	private float charAngle;
	
	// Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
		controller = GetComponent<CharacterController>();
		
		if (anim.layerCount >= 2) {
			anim.SetLayerWeight(1, 1);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (anim)
		{
			v = Input.GetAxis("Vertical");
			h = Input.GetAxis("Horizontal");
			charAngle = 0.0f;
			speed = new Vector2(h, v).sqrMagnitude;
			moveDirection = new Vector3(h, 0, v);
			moveDirection = transform.TransformDirection(moveDirection);
			
			Vector3 axisSign = Vector3.Cross(transform.forward, moveDirection);
			charAngle = Vector3.Angle(transform.forward, moveDirection) * (axisSign.y >= 0 ? 1f : -1f);
			
			
			anim.SetFloat("Speed", speed);
			anim.SetFloat("Horizontal", h, directionDampTime, Time.deltaTime);
			anim.SetFloat("Vertical", v, directionDampTime, Time.deltaTime);
			
			
			if (speed > movementTresHold) {
				if (!isInPivot()) {
					anim.SetFloat("Angle", charAngle, directionDampTime, Time.deltaTime);
				}
			}
			if (speed < movementTresHold && Mathf.Abs(h) < 0.05f) {	// Dead Zone
				anim.SetFloat("Horizontal", 0f);
			}
			
			moveDirection *= speed;
			
			controller.Move(moveDirection * Time.deltaTime);
			
		}
    }
	
	void FixedUpdate()
	{
		if (isInMovement())
		{
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (h < 0f ? -1f : 1f), 0f), Mathf.Abs(h));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			transform.rotation = (transform.rotation * deltaRotation);
		}
	}
	
	bool isInPivot()
	{
		return (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.IdlePivotL") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.IdlePivotR"));
	}
	
	bool isInMovement()
	{
		return anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Movement");
	}
}
