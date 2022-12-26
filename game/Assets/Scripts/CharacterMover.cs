using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CharacterMover : MonoBehaviour
{
	public bool enableComputerMovement = false;
	public Player player;
	public Rigidbody rb;
	public Transform body;
	public float smoothJoystickRate = 20f;
	public Collider col = null;
	public float speed = 3f;
	public float movementSmoothing = 0.1f;
	public float recoveryTime = 1f;
	public float force = 30f;
	public bool useControlledVelocity = false;
	public bool randomJoystick = false;
	public float randomRate = 5f;
	public float randomControler = 0.3f;
	public GroundChecker groundChecker = null;

	public float angularSmoothing = 15f;

	private Vector3 _lastDir = -Vector3.forward;
	[ShowNonSerializedField] private float _controlTime = 0f;
	private Vector3 _velocity = Vector3.zero;

	[ReadOnly]
	public Vector3 smoothJoystick = Vector3.zero;

	private float randomOffset;

	private void Start()
	{
		randomOffset = Random.Range(0, 40f);
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
			RigidbodyConstraints.FreezeRotationY;
	}

	private Vector3 GetMovementFromPlayer()
	{
		var user = player.user;
		if (user == null)
			return Vector3.zero;

		var updatedAtIsoString = user.updatedAt;
		var updatedAt = System.DateTime.Parse(updatedAtIsoString);
		var now = System.DateTime.Now;
		var diff = now - updatedAt;
		if (diff.TotalSeconds > 2)
			return Vector3.zero;

		var joystickX = user.joystickX;
		var joystickY = user.joystickY;
		var joystick = new Vector3(joystickX, 0, joystickY);
		return joystick;
	}

	private Vector3 ComputerMovement()
	{
		var joystick = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		return joystick;
	}

	public void BreakControlsTemporarily()
	{
		_controlTime = recoveryTime;
	}

	private void FixedUpdate()
	{
		_controlTime -= Time.deltaTime;

		var joystick = enableComputerMovement ? ComputerMovement() : GetMovementFromPlayer();
		joystick = joystick.normalized;
		if (joystick.sqrMagnitude > 0.001f)
			_lastDir = joystick.normalized;
		if (randomJoystick)
		{
			var dirX = Mathf.PerlinNoise(Time.time * randomRate + randomOffset, 0f);
			var dirY = Mathf.PerlinNoise(0f, Time.time * randomRate + randomOffset);
			dirX = Mathf.Lerp(-1f, 1f, dirX);
			dirY = Mathf.Lerp(-1f, 1f, dirY);
			joystick = (new Vector3(dirX + 0.05f, 0, dirY + 0.05f) + joystick * randomControler).normalized;
		}
		smoothJoystick = Vector3.Lerp(smoothJoystick, joystick, Time.deltaTime * smoothJoystickRate);

		var targetRotation = Quaternion.LookRotation(_lastDir, Vector3.up);
		body.rotation = Quaternion.Slerp(body.rotation, targetRotation, angularSmoothing * Time.fixedDeltaTime);

		var groundCheck = !groundChecker || groundChecker.IsGrounded;
		var controlCheck = _controlTime < 0;
		if (groundCheck && controlCheck)
		{
			if (useControlledVelocity)
			{
				var movement = joystick * speed;
				movement = Vector3.SmoothDamp(rb.velocity, movement, ref _velocity, movementSmoothing);
				movement.y = rb.velocity.y;
				rb.velocity = movement;
			}
			else
			{
				var up = transform.up;
				var velocity = rb.velocity;
				var velDir = velocity.normalized;
				var joyDir = joystick.normalized;
				var dot = Vector3.Dot(velDir, joyDir);
				var orthog = Vector3.Cross(up, velDir).normalized;
				var speed = velocity.magnitude;

				var force = joyDir * this.force;
				if (dot > 0 && speed >= this.speed)
					force = Vector3.Project(force, orthog);

				rb.AddForce(force, ForceMode.Acceleration);
			}
		}
	}
}
