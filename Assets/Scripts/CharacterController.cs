using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	#region properties (private)
	
	[SerializeField] private float directionDampTime = .25f;
	
	private Animator anim;
	
	private float speed = 0.0f;
	private float h = 0.0f;
	private float v = 0.0f;
	
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
    }

    // Update is called once per frame
    void Update()
    {
		if (anim)
		{
			// Pull values from controller/keyboard
			v = Input.GetAxis("Vertical");
			h = Input.GetAxis("Horizontal");
			
			speed = new Vector2(h, v).sqrMagnitude;
			
			anim.SetFloat("Speed", speed);
			anim.SetFloat("Direcction", h, directionDampTime, Time.deltaTime);
		}
    }
	
	#endregion
	
	#region methods
	
	#endregion
}
