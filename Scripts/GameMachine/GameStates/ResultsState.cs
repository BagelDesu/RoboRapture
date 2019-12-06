//-----------------------------------------------------------------------
// <copyright file="ResultsState.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.UI;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.GameMachine.States
{
    public class ResultsState : IGameState
    {
        [SerializeField]
        private ResultsPanel resultsPanel;

        [SerializeField]
        private GameplayState gameplayState;

        public override void Begin(GameMachine context)
        {
            Logcat.I($"Starting results state. Has player won? {gameplayState.HasPlayerWon}");
            if (gameplayState.HasPlayerWon)
            {
                resultsPanel.RaiseWin();
            } else
            {
                resultsPanel.RaiseLose();
            }
        }

        public override void Next()
        {
            //// Doing nothing
        }

    }
}