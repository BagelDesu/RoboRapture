//-----------------------------------------------------------------------
// <copyright file="GameplayState.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
using Edu.Vfs.RoboRapture.Controllers;
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.Managers;
using Edu.Vfs.RoboRapture.TurnSystem;
using Edu.Vfs.RoboRapture.UI;
using System;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.GameMachine.States
{
    public class GameplayState : IGameState
    {
        public static Action GameplayStateStarted;

        private GameMachine context;

        [SerializeField]
        private EntityTurnManager turnManager;

        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private EnemyController enemyController;

        [SerializeField]
        private TurnsUIUpdater turnsUI;

        [SerializeField]
        private RectTransform experiencePointsUI;

        [SerializeField]
        private GameObject levelProgress;

        [SerializeField]
        private float delayToStartGame = 5;

        private bool hasPlayerWon;

        public bool HasPlayerWon { get => hasPlayerWon; set => hasPlayerWon = value; }

        private void Awake()
        {
            GameManager.GameEnded += GameEnded;
        }

        private void OnDisable()
        {
            GameManager.GameEnded -= GameEnded;
        }

        public override void Begin(GameMachine context)
        {
            this.context = context;
            Invoke("StartGame", delayToStartGame);
        }

        private void StartGame()
        {
            turnManager.StartTurnSystem();
            playerController.gameObject.SetActive(true);
            enemyController.gameObject.SetActive(true);
            turnsUI.gameObject.SetActive(true);
            experiencePointsUI.gameObject.SetActive(true);
            levelProgress?.gameObject.SetActive(true);
            GameplayStateStarted?.Invoke();
        }

        public override void Next()
        {
            this.context.Next();
        }

        private void GameEnded(bool hasPlayerWon)
        {
            Logcat.I(this, $"Game ended, has player won? {hasPlayerWon}");
            this.hasPlayerWon = hasPlayerWon;
            Next();
        }
    }
}
