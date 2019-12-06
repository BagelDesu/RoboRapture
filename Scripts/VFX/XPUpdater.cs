using UnityEngine;

public class XPUpdater : MonoBehaviour
{
    private Collider _collider;
    private Animator _anim;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _anim = GameObject.Find("XP")?.GetComponentInChildren<Animator>(true);
    }

    private void OnParticleCollision(GameObject other)
    {
        _anim?.Play("XPUpdater_Flash");
    }

}
