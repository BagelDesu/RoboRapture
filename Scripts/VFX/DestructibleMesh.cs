using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleMesh : MonoBehaviour
{
    [SerializeField] private CrackedMesh crackedMesh;

    public void SplitMesh()
    {
        // Debug.Log("Mesh splitting");
        Instantiate(crackedMesh, transform.position, Quaternion.identity);
    }

}
