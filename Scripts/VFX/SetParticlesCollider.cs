using UnityEngine;

public class SetParticlesCollider : MonoBehaviour
{
    private ParticleSystem _ps;
    private Collider _collider;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        _collider = GameObject.Find("XPUpdater").GetComponent<Collider>();
        _ps.trigger.SetCollider(0, _collider);
    }
}
