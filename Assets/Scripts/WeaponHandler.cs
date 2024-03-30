using Fusion;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField]
    private int _damage = 10;
    
    [SerializeField]
    private Transform _cameraTrans;

    [SerializeField]
    private LayerMask _hitLayer;

    [SerializeField]
    private HitOptions _hitOptions = HitOptions.IncludePhysX | HitOptions.SubtickAccuracy | HitOptions.IgnoreInputAuthority;
    
    public void Fire()
    {
        if (Runner.LagCompensation.Raycast(_cameraTrans.position, 
                _cameraTrans.forward, 
                Mathf.Infinity,
                Object.InputAuthority,
                out LagCompensatedHit hit,
                _hitLayer,
                _hitOptions))
        {
            if (hit.GameObject.TryGetComponent<PlayerController>(out var hitPlayerController))
            {
                hitPlayerController.TakeDamage(_damage);
            }
        }
    }
}
