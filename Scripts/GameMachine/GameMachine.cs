//-----------------------------------------------------------------------
// <copyright file="GameMachine.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.GameMachine
{
    using Edu.Vfs.RoboRapture.Helpers;
    using UnityEngine;

    public class GameMachine : IGameState
    {
        [SerializeField]
        private IGameState[] gameStates;

        private int index;

        public void Awake()
        {
            this.index = 0;
            this.Begin(this);
        }

        public override void Begin(GameMachine context)
        {
            this.gameStates[this.index].gameObject.SetActive(true);
            this.gameStates[this.index].Begin(this);
            Logcat.I(this, $"Starting a new state: {gameStates[index]}");
        }

        public override void Next()
        {
            this.gameStates[this.index].gameObject.SetActive(false);
            this.index++;

            if (this.index >= this.gameStates.Length)
            {
                return;
            }

            this.Begin(this);
        }
    }
}