//-----------------------------------------------------------------------
// <copyright file="RefVariable.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.ScriptableLibrary 
{
    using UnityEngine;

    /// <summary>
    /// Sets a new reference variable of type T.
    /// </summary>
    /// <typeparam name="T">Reference Type</typeparam>
    public abstract class RefVariable<T> : GameEvent
    {
        /// <summary>
        /// Variable of type T.
        /// </summary>
        [SerializeField]
        private T value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>New value to set.</value>
        public T Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
                this.Notify();
            }
        }
    }
}