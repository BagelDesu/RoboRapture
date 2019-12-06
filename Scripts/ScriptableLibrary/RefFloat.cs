//-----------------------------------------------------------------------
// <copyright file="RefFloat.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.ScriptableLibrary 
{
    using UnityEngine;

    /// <summary>
    /// Set a reference float.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptables/Variables/Float", fileName = "Float_")]
    public class RefFloat : RefVariable<float> 
    {
        /// <summary>
        /// Min Value this reference can get.
        /// </summary>
        private float minValue;

        /// <summary>
        /// Max value this reference can get.
        /// </summary>
        private float maxValue;

        /// <summary>
        /// Gets or sets the value for minValue.
        /// </summary>
        /// <value>New value to set.</value>
        public float MinValue 
        { 
            get 
            {
                return this.minValue;
            }

            set 
            {
                this.minValue = value; 
            }
        }

        /// <summary>
        /// Gets or sets the value for MaxValue.
        /// </summary>
        /// <value>New value to set.</value>
        public float MaxValue 
        { 
            get 
            {
                return this.maxValue;
            }

            set 
            {
                this.maxValue = value; 
            }
        }
    }
}