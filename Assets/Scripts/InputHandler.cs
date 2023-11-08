using System;
using Fusion;
using UnityEngine;

public class InputHandler : NetworkBehaviour, IBeforeUpdate
{
    [SerializeField] 
    private float _mouseSensitivity = 10f;
    
    public Angle PitchInput { get; private set; }
    public Angle YawInput   { get; private set; }

    public override void Spawned()
    {
        if (Runner.LocalPlayer != Object.InputAuthority) return;
        
        var events = Runner.GetComponent<NetworkEvents>();
        events.OnInput.AddListener(OnInput);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (Runner.LocalPlayer != Object.InputAuthority) return;
        
        var events = Runner.GetComponent<NetworkEvents>();
        events.OnInput.RemoveListener(OnInput);
    }
    
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var inputData = new InputData();
        
        if (Input.GetKey(KeyCode.W)) { inputData.MoveInput += Vector2.up; }
        if (Input.GetKey(KeyCode.S)) { inputData.MoveInput += Vector2.down; }
        if (Input.GetKey(KeyCode.A)) { inputData.MoveInput += Vector2.left; }
        if (Input.GetKey(KeyCode.D)) { inputData.MoveInput += Vector2.right; }
        
        inputData.Pitch = PitchInput;
        inputData.Yaw   = YawInput;
        PitchInput      = 0;
        YawInput        = 0;

        inputData.Button.Set(InputButton.Jump, Input.GetKey(KeyCode.Space));
        
        input.Set(inputData);
    }

    public void BeforeUpdate()
    {
        PitchInput += Input.GetAxis("Mouse Y") * _mouseSensitivity * (-1);
        YawInput += Input.GetAxis("Mouse X") * _mouseSensitivity;
    }
}