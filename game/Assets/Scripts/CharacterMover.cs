using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CharacterMover : MonoBehaviour
{
	public bool enableComputerMovement = false;
	public Player player;
	public Rigidbody rb;
	public float speed = 10;

	public float angularSmoothing = 15f;
	public float movementSmoothing = .1f;
  public float movementRecovery = 3f;

	private Vector3 _lastDir = -Vector3.forward;
	private Vector3 _vel = Vector3.zero;
  private float _movemventSmoothing = 0;

	private void Start()
	{
    _movemventSmoothing = movementSmoothing;
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
			RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;
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

	private void Update()
	{
    _movemventSmoothing = Mathf.Lerp(_movemventSmoothing, movementSmoothing, Time.deltaTime * movementRecovery);

		var joystick = enableComputerMovement ? ComputerMovement() : GetMovementFromPlayer();
		joystick = joystick.normalized;
    if (joystick.sqrMagnitude > 0.001f)
			_lastDir = joystick.normalized;

		var movement = joystick * speed;
		movement = Vector3.SmoothDamp(rb.velocity, movement, ref _vel, _movemventSmoothing);

		var targetRotation = Quaternion.LookRotation(_lastDir, Vector3.up);
		var rotation = Quaternion.Slerp(transform.rotation, targetRotation, angularSmoothing * Time.deltaTime);
		transform.rotation = rotation;

		rb.velocity = movement;
	}

  public void AddForce(Vector3 force)
  {
    _movemventSmoothing = force.magnitude;
    rb.AddForce(force, ForceMode.Impulse);
  }
}
