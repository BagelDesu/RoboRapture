//-----------------------------------------------------------------------
// <copyright file="IGameState.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.GameMachine
{ 
    using UnityEngine;

    public abstract class IGameState : MonoBehaviour
    {
        public abstract void Begin(GameMachine context);

        public abstract void Next();
    }
}