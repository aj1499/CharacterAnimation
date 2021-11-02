using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	
	[SerializeField] private float mouseSensitivity = 0.0f;
	[SerializeField] private float distanceAway;
	[SerializeField] private float distanceUp;
	[SerializeField] private float smooth;
	[SerializeField] private Transform follow;
	
	private Vector3 targetPosition;
	private Transform parent;
	
	
    // Start is called before the first frame update
    void Start()
    {
		parent = transform.parent;
		Cursor.lockState = CursorLockMode.Locked;
		
		follow = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate();
    }
	
	void Rotate()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		
		parent.Rotate(Vector3.up, mouseX);
	}
	
	void LateUpdate()
	{
		targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
		
		Debug.DrawRay(follow.position, Vector3.up * distanceUp, Color.red);
		Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
		Debug.DrawRay(follow.position, targetPosition, Color.magenta);
		
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
		
		transform.LookAt(follow);
	}
}
