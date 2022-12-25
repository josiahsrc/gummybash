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
	public float recoveryRate = 15f;
	public float maxControl = 0.75f;
	public float force = 30f;
	public bool useControlledVelocity = false;
	public bool randomJoystick = false;
	public float randomRate = 5f;

	public float angularSmoothing = 15f;

	private Vector3 _lastDir = -Vector3.forward;
	[ShowNonSerializedField] private float _control = 0f;
	private Vector3 _velocity = Vector3.zero;

	[HideInInspector]
	public Vector3 smoothJoystick = Vector3.zero;

	private void Start()
	{
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
			RigidbodyConstraints.FreezeRotationY;
		_control = maxControl;
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

	private void FixedUpdate()
	{
		_control = Mathf.Lerp(_control, maxControl, Time.deltaTime * recoveryRate);

		var joystick = enableComputerMovement ? ComputerMovement() : GetMovementFromPlayer();
		joystick = joystick.normalized;
		if (joystick.sqrMagnitude > 0.001f)
			_lastDir = joystick.normalized;
		if (randomJoystick)
		{
			var dirX = Mathf.PerlinNoise(Time.time * randomRate, 0f);
			var dirY = Mathf.PerlinNoise(0f, Time.time * randomRate);
			dirX = Mathf.Lerp(-1f, 1f, dirX);
			dirY = Mathf.Lerp(-1f, 1f, dirY);
			joystick = ((new Vector3(dirX + 0.05f, 0, dirY + 0.05f) + joystick) / 2).normalized;
		}
		smoothJoystick = Vector3.Lerp(smoothJoystick, joystick, Time.deltaTime * smoothJoystickRate);

		var targetRotation = Quaternion.LookRotation(_lastDir, Vector3.up);
		body.rotation = Quaternion.Slerp(body.rotation, targetRotation, angularSmoothing * Time.fixedDeltaTime);

		if (useControlledVelocity)
		{
			var movement = joystick * speed;
			movement = Vector3.SmoothDamp(rb.velocity, movement, ref _velocity, movementSmoothing);
			var nextVel = Vector3.Lerp(rb.velocity, movement, _control);
			nextVel.y = rb.velocity.y;
			rb.velocity = nextVel;
		}
		else
		{
      // only apply a force proportional to the speed cap
      var velocity = rb.velocity;
      var velDir = velocity.normalized;
      var joyDir = joystick.normalized;
      var dot = Vector3.Dot(velDir, joyDir);
      
      // TODO: Figure this out
      var speedCap = speed * _control;
      var speedDiff = speedCap - velocity.magnitude;
      var force = speedDiff * dot;
      rb.AddForce(joystick * force, ForceMode.Acceleration);
		}
	}
}
