//-----------------------------------------------------------------------
// <copyright file="GameEvent.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.ScriptableLibrary 
{
    using System;
    using NaughtyAttributes;
    using UnityEngine;

    /// <summary>
    /// Defines a Game Event.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptables/GameEvent", fileName = "Event_")]
    public class GameEvent : ScriptableObject
    {
        /// <summary>
        /// Sets the listeners to be registered to this Game Event.
        /// </summary>
        public event Action Listeners;

        /// <summary>
        /// Notify all listeners when a new change occurs.
        /// </summary>
        [Button]
        public void Notify()
        {
            if (this.Listeners == null)
            {
                return;
            }
            
            this.Listeners.Invoke();
        }
    }
}