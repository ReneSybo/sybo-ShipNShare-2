using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public float Distance;
    
    Transform _cameraTransform;
    Vector3 _direction;

    void Awake()
    {
        _cameraTransform = transform;
        _direction = _cameraTransform.forward;
    }

    void LateUpdate()
    {
        _cameraTransform.position = Player.position + (_direction * Distance);
    }

    [ContextMenu("Look At Player")]
    void LookAt()
    {
        if (Player != null)
        {
            Transform camTransform = transform;
            camTransform.LookAt(Player);
            camTransform.position = Player.position + (camTransform.forward * Distance);
        }
    }
}
