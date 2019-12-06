//-----------------------------------------------------------------------
// <copyright file="Projectile.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Attacks
{
    using UnityEngine;

    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float velocity;

        [SerializeField]
        private float lifeTime;

        private Rigidbody projectileRigidbody;

        private void Start()
        {
            this.projectileRigidbody = this.GetComponent<Rigidbody>();
            this.projectileRigidbody.velocity = this.transform.forward * this.velocity;

            Projectile.Destroy(this.gameObject, this.lifeTime);
        }
    }
}
