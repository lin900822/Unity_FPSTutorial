using Fusion;
using UnityEngine;

public enum InputButton
{
    Jump
}

public struct InputData : INetworkInput
{
    public NetworkButtons Button;
    public Vector2        MoveInput;
    public Angle          Pitch;
    public Angle          Yaw;
}
