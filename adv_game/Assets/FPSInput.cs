using UnityEngine;
using System.Collections;

// basic WASD-style movement control
// commented out line demonstrates that transform.Translate instead of charController.Move doesn't have collision detection

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour {
	public float speed = 6.0f;
	public float gravity = -9.8f;
	public float turnSpeed = 0.05F;

	private CharacterController charController;
	private Animator charAnimator;

	Vector3 input = Vector3.zero;
    Quaternion influencedByCamera;
	
	void Start() {
		charController = GetComponent<CharacterController>();
		charAnimator = GetComponent<Animator>();
	}
	
	void Update() {
		//transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime);
		float deltaX = Input.GetAxis("Horizontal") * speed;
		float deltaZ = Input.GetAxis("Vertical") * speed;
		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, speed);

		movement.y = gravity;

		movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement);
		charController.Move(movement);

		if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			charAnimator.Play("Walking");

			input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            influencedByCamera = Quaternion.LookRotation(Quaternion.Euler(0, charController.transform.rotation.eulerAngles.y, 0) * (input * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, influencedByCamera, turnSpeed);
		}
		else {
			if(Input.GetButton("Jump"))
			{
				charAnimator.Play("Swinging");
			}
			//else
				//charAnimator.Play("Idle");
		}
	}
}
