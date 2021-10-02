using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public DynamicJoystick Joystick;
	public CharacterController CharacterController;

	public Animator Animator;

	public bool CanMove = true;
	public float Sensitivity = 1f;
	public float MovementSpeed = 1f;
	public float RotationSpeed = 30f;
	Vector3 direction;
	Vector3 velocity;
	bool isGrounded;
	public float gravity = -9.81f;
	public Transform GroundCheck;
	public float GroundDistance = 0.4f;
	public LayerMask GroundMask;

	void Update()
	{
		Movement();
		//GravitiyAffection();
	}

	void Movement()
	{
		if (CanMove && Input.GetMouseButton(0))
		{
			direction = new Vector3(Joystick.Direction.x, 0, Joystick.Direction.y) * Sensitivity;
			if (direction.magnitude > 0.05f)
			{
				Animator.SetBool("Walking", true);

				CharacterController.Move(direction * MovementSpeed * Time.deltaTime);

				Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
				Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
				transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, RotationSpeed * Time.deltaTime);
			}
			else
				Animator.SetBool("Walking", false);
		}
		else
		{
			Animator.SetBool("Walking", false);
		}
	}

	void GravitiyAffection()
    {
		if (isGrounded && velocity.y < 0f)
		{
			velocity.y = -2f;
		}
		isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

		velocity.y += gravity * Time.deltaTime;
		CharacterController.Move(velocity * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider col)
	{
		var buildingArea = col.gameObject.GetComponent<BuildingArea>();
		if (buildingArea)
		{
			BuildingManager.Instance.ChangeSelectedBuildingArea(buildingArea);
		}
	}

	private void OnTriggerExit(Collider col)
	{
		var buildingArea = col.gameObject.GetComponent<BuildingArea>();
		if (buildingArea && buildingArea == BuildingManager.Instance.SelectedBuildingArea)
		{
			BuildingManager.Instance.ChangeSelectedBuildingArea(null);
		}
	}
}
