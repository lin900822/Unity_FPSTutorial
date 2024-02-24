using Fusion;
using UnityEngine;

public enum InputButton
{
    Jump,
    Fire
}

public struct InputData : INetworkInput
{
    public NetworkButtons Button;
    public Vector2        MoveInput;
    public Angle          Pitch;
    public Angle          Yaw;
}
