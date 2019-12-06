using UnityEngine;

public class CrackedMesh : MonoBehaviour
{

    [SerializeField]
    private float _lifetime = 10f;
    private float _elapsedTime = 0f;

    private Renderer[] _shards;

    private void Start()
    {
        _shards = GetComponentsInChildren<Renderer>();
        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        foreach (Renderer mesh in _shards)
        {
            mesh.materials[0].SetFloat("_Scaling", Mathf.Lerp(0f, 1f, (_lifetime - _elapsedTime) / _lifetime));
        }
        
        _elapsedTime += Time.deltaTime;
    }
}
