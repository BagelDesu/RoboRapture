//-----------------------------------------------------------------------
// <copyright file="FXWrapper.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using Edu.Vfs.RoboRapture.Converters;
    using NaughtyAttributes;
    using UnityEngine;

    public class FXWrapper : MonoBehaviour
    {
        [ShowIf("ShowAttributes")]
        [SerializeField]
        private GameObject particles;

        [ShowIf("ShowAttributes")]
        [SerializeField]
        private string particleSFX;

        [ShowIf("ShowAttributes")]
        [SerializeField]
        private float particlesDuration;

        [ShowIf("ShowAttributes")]
        [SerializeField]
        private bool destroyParticles;

        [ShowIf("ShowAttributes")]
        [SerializeField]
        private float offset = 0.3f;

        private Vector3 particlesPosition;

        private bool hasPlayed = false; //// TODO check

        private GameObject particlesInstance;

        public virtual void Play(Vector3 position)
        {
            this.particlesPosition = position;

            PlaySound();
            PlayParticles();
            
            this.hasPlayed = true;
        }

        private void PlaySound()
        {
            if (!this.particleSFX.Equals(string.Empty))
            {
                AkSoundEngine.PostEvent($"Play_{particleSFX}", this.gameObject);
            }
        }

        private void PlayParticles()
        {
            if (particles == null)
            {
                return;
            }

            Vector3 position = new Vector3(particlesPosition.x, particlesPosition.y + offset, particlesPosition.z);
            particlesInstance = Instantiate(this.particles, position, transform.rotation);
            particlesInstance.GetComponent<Particle>()?.SetPoint(PointConverter.ToPoint(position));

            if (this.destroyParticles)
            {
                MonoBehaviour.Destroy(particlesInstance, this.particlesDuration);
            }
        }

        public bool HasPlayed()
        {
            return this.hasPlayed;
        }

        public bool ShowAttributes()
        {
            return true;
        }

        public void UpdateParticlesPosition(Vector3 position)
        {
            particlesInstance.gameObject.transform.position = position;
        }

        public void UpdateParticlesRotation(Quaternion rotation)
        {
            particlesInstance.gameObject.transform.rotation = rotation;
        }

        public void DestroyInstance()
        {
            MonoBehaviour.Destroy(particlesInstance);
        }
    }
}