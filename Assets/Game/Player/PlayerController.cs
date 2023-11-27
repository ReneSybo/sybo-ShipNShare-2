using UnityEngine;

namespace Game.Player
{
	public class PlayerController : MonoBehaviour
	{
		public float Speed = 1f;
		public float MaxSpeed = 10f;
		public float SpeedDampening = 0.98f;
		public float Epsilon = 0.001f;

		public Vector2 CurrentSpeed = Vector2.zero;
		
		public Transform PlayerMesh;

		public Animator _animator;
		
		Transform _playerTransform;

		void Awake()
		{
			_playerTransform = transform;
		}

		void Update()
		{
			float horizontalMovement = Input.GetAxis("Horizontal");
			float verticalMovement = Input.GetAxis("Vertical");
			
			CurrentSpeed *= SpeedDampening;
			CurrentSpeed.x += horizontalMovement * Speed * GameTime.DeltaTime;
			CurrentSpeed.y += verticalMovement * Speed * GameTime.DeltaTime;

			if (Mathf.Abs(CurrentSpeed.x) <= Epsilon)
			{
				CurrentSpeed.x = 0;
			}
			
			if (Mathf.Abs(CurrentSpeed.y) <= Epsilon)
			{
				CurrentSpeed.y = 0;
			}

			Vector3 currentPos = _playerTransform.localPosition;
			currentPos.x += CurrentSpeed.x;
			currentPos.z += CurrentSpeed.y;
			_playerTransform.localPosition = currentPos;
			GlobalVariables.PlayerPos = currentPos;

			// Setting animator values 
			_animator.SetFloat("Speed", CurrentSpeed.magnitude);

			// Only update direction when actually getting a new one /Baldwin
			if(CurrentSpeed.magnitude > 0.01f)
			{
				PlayerMesh.rotation = Quaternion.LookRotation(new Vector3(CurrentSpeed.x, 0, CurrentSpeed.y), Vector3.up);
			}
			

		}
	}
}