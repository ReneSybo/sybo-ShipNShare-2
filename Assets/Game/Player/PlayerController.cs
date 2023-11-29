using Game.Events;
using UnityEngine;
using AudioType = Game.Audio.AudioType;

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

		float _upperBodyLayer;
		public Vector3 CurrentSpeed;
		Transform _playerTransform;
		Vector3 _currentMeshForward;

		Vector3 _initialForward;
		
		void Awake()
		{
			_playerTransform = transform;
			_currentMeshForward = PlayerMesh.forward;
			_initialForward = _currentMeshForward;
			
			GameEvents.ProjectileSpawned.AddListener(OnProjectileSpawned);
			GameEvents.PlayerHurt.AddListener(OnPlayerHurt);
			GameEvents.RoundStarted.AddListener(OnRoundStarted);
			GameEvents.RoundOver.AddListener(OnRoundEnded);
			GameEvents.GameStarted.AddListener(OnGameStart);
			GameEvents.CutsceneState.AddListener(OnCutsceneState);
			
			_animator.SetLayerWeight(1, _upperBodyLayer);
			_animator.SetFloat(AnimationSpeed, 0);
			
			enabled = false;
		}

		void OnGameStart()
		{
			GlobalVariables.Score = 0;
			GlobalVariables.ScoreLostToTime = 0;
		}

		void OnCutsceneState(bool active)
		{
			enabled = !active;
			if (!active)
			{
				_upperBodyLayer = 0;
				_animator.SetLayerWeight(1, _upperBodyLayer);
				_animator.SetFloat(AnimationSpeed, 0);
				_animator.SetFloat(AnimationDirection, Vector3.SignedAngle(Weapon.CurrentShotDirection, _currentMeshForward, Vector3.up));
			}
		}

		void OnRoundEnded()
		{
			enabled = false;
		}

		void OnRoundStarted()
		{
			enabled = true;
		}

		void OnPlayerHurt()
		{
			if (GlobalVariables.PlayerHealth <= 0)
			{
				GameEvents.GameEnded.Dispatch();
				_animator.SetFloat(AnimationSpeed, 0);
				GlobalVariables.PlayerHealth = GlobalVariables.PlayerStartHealth;
				GlobalVariables.PlayerMaxHealth = GlobalVariables.PlayerStartHealth;
				GlobalVariables.Money = 0;
				enabled = false;
				
				_playerTransform.localPosition = new Vector3(-1.99f, 0f, 3f);
				_currentMeshForward = _initialForward;
				PlayerMesh.forward = _initialForward;
				CurrentSpeed = Vector3.zero;
			}
			
			GameEvents.PlayAudio.Dispatch(AudioType.PlayerHit);
		}

		void OnProjectileSpawned(bool fromPrimary)
		{
			_upperBodyLayer = 1f;
			_animator.SetTrigger(fromPrimary ? AnimationShotPrimary : AnimationShotSecondary);
		}

		void Update()
		{
			UpdateCurrentSpeed();
			UpdatePlayerPosition();
			UpdateCharacterMesh();

			_upperBodyLayer -= GameTime.DeltaTime * 0.5f;
			if (_upperBodyLayer < 0)
			{
				_upperBodyLayer = 0;
			}
			
			_animator.SetLayerWeight(1, _upperBodyLayer);
			_animator.SetFloat(AnimationDirection, Vector3.SignedAngle(Weapon.CurrentShotDirection, _currentMeshForward, Vector3.up));
		}

		void UpdateCurrentSpeed()
		{
			Vector3 movement = GetMovement(out bool moved);
			Vector3 movementToAdd = movement.normalized;
			
			CurrentSpeed *= SpeedDampening;
			if (moved)
			{
				float speed = Speed + Upgrades.Instance.GetValue(UpgradeType.Speed);
				CurrentSpeed.x += movementToAdd.x * Mathf.Abs(movement.x) * speed * GameTime.DeltaTime;
				CurrentSpeed.y = 0;
				CurrentSpeed.z += movementToAdd.z * Mathf.Abs(movement.z) * speed * GameTime.DeltaTime;
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