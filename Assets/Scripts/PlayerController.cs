using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{   
    [SerializeField]
    private NetworkCharacterControllerPrototype _characterController;
    [SerializeField]
    private MeshRenderer[] _visuals;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _speed = 5f;
    [Networked]
    private Angle _yaw { get; set; }
    [Networked]
    private Angle _pitch { get; set; }
    [Networked]
    private NetworkButtons _previousButton { get; set; }
    
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            _camera.enabled                               = true;
            _camera.GetComponent<AudioListener>().enabled = true;

            foreach (var visual in _visuals)
            {
                visual.enabled = false;
            }
        }
        else
        {
            _camera.enabled                               = false;
            _camera.GetComponent<AudioListener>().enabled = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out InputData data))
        {
            var buttonPressed = data.Button.GetPressed(_previousButton);
            _previousButton = data.Button;

            var moveInput = new Vector3(data.MoveInput.x, 0, data.MoveInput.y);
            _characterController.Move(transform.rotation * moveInput * _speed * Runner.DeltaTime);

            HandlePitchYaw(data);

            if (buttonPressed.IsSet(InputButton.Jump))
            {
                _characterController.Jump();
            }
        }

        transform.rotation = Quaternion.Euler(0, (float)_yaw, 0);

        var cameraEulerAngle = _camera.transform.localRotation.eulerAngles;
        _camera.transform.rotation = Quaternion.Euler((float)_pitch, cameraEulerAngle.y, cameraEulerAngle.z);
    }

    private void HandlePitchYaw(InputData data)
    {
        _yaw   += data.Yaw;
        _pitch += data.Pitch;

        if (_pitch >= 180 && _pitch <= 270)
        {
            _pitch = 271;
        }
        else if (_pitch <= 180 && _pitch >= 90)
        {
            _pitch = 89;
        }
    }
}