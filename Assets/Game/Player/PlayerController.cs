using Game.Events;
using UnityEngine;

namespace Game.Player
{
	public class PlayerController : MonoBehaviour
	{
		static readonly int AnimationSpeed = Animator.StringToHash("Speed");
		static readonly int AnimationDirection = Animator.StringToHash("Direction");
		static readonly int AnimationShotPrimary = Animator.StringToHash("FireR");
		static readonly int AnimationShotSecondary = Animator.StringToHash("FireL");
		
		public float Speed = 1f;
		public float SpeedDampening = 0.98f;
		public Transform PlayerMesh;
		public Animator _animator;
		public Weapon Weapon;
		
		public Vector3 CurrentSpeed;
		Transform _playerTransform;
		Vector3 _currentMeshForward;

		void Awake()
		{
			_playerTransform = transform;
			_currentMeshForward = _playerTransform.forward;
			
			GameEvents.ProjectileSpawned.AddListener(OnProjectileSpawned);
		}

		void OnProjectileSpawned(bool fromPrimary)
		{
			_animator.SetTrigger(fromPrimary ? AnimationShotPrimary : AnimationShotSecondary);
		}

		void Update()
		{
			UpdateCurrentSpeed();
			UpdatePlayerPosition();
			UpdateCharacterMesh();
			
			_animator.SetFloat(AnimationDirection, Vector3.SignedAngle(Weapon.CurrentShotDirection, _currentMeshForward, Vector3.up));
		}

		void UpdateCurrentSpeed()
		{
			Vector3 movement = GetMovement(out bool moved);
			Vector3 movementToAdd = movement.normalized;
			
			CurrentSpeed *= SpeedDampening;
			if (moved)
			{
				CurrentSpeed.x += movementToAdd.x * Mathf.Abs(movement.x) * Speed * GameTime.DeltaTime;
				CurrentSpeed.y = 0;
				CurrentSpeed.z += movementToAdd.z * Mathf.Abs(movement.z) * Speed * GameTime.DeltaTime;
			}
		}

		void UpdatePlayerPosition()
		{
			Vector3 currentPos = _playerTransform.localPosition;
			currentPos.x += CurrentSpeed.x;
			currentPos.z += CurrentSpeed.z;
			_playerTransform.localPosition = currentPos;
			GlobalVariables.PlayerPos = currentPos;
		}
		
		void UpdateCharacterMesh()
		{
			float currentSpeedMagnitude = CurrentSpeed.magnitude;
			
			// Setting animator values 
			_animator.SetFloat(AnimationSpeed, currentSpeedMagnitude);

			// Only update direction when actually getting a new one /Baldwin
			if(Mathf.Abs(currentSpeedMagnitude) > 0.01f)
			{
				_currentMeshForward = Vector3.Slerp(_currentMeshForward, CurrentSpeed.normalized, GameTime.DeltaTime * 10f);
				PlayerMesh.forward = _currentMeshForward;
			}
		}

		Vector3 GetMovement(out bool moved)
		{
			Vector3 movementToAdd = Vector3.zero;
			moved = false;
			
			float horizontalMovement = Input.GetAxis("Horizontal");
			if (horizontalMovement != 0)
			{
				movementToAdd.x = horizontalMovement;
				moved = true;
			}

			float verticalMovement = Input.GetAxis("Vertical");
			if (verticalMovement != 0)
			{
				movementToAdd.z = verticalMovement;
				moved = true;
			}

			return movementToAdd;
		}
	}
}