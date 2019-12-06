//-----------------------------------------------------------------------
// <copyright file="PlacementState.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
using Edu.Vfs.RoboRapture.Controllers;
using Edu.Vfs.RoboRapture.Managers;
using Edu.Vfs.RoboRapture.UI;
using System;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.GameMachine.States
{
    public class PlacementState : IGameState
    {
        public static Action PlacementPhaseStarted;

        public static Action PlacementPhaseEnded;

        public static bool isInPlacementPhase = true;

        private GameMachine context;

        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private EnemyController enemyController;

        [SerializeField]
        private TurnsUIUpdater turnsUI;

        [SerializeField]
        private RectTransform experiencePointsUI;
        
        [SerializeField]
        private PlacementManager placementManager;

        [SerializeField]
        private GameObject levelProgress;

        public void Awake()
        {
            PlacementManager.PlacementCompleted += Next;
        }

        public void OnDisable()
        {
            PlacementManager.PlacementCompleted -= Next;
        }

        public override void Begin(GameMachine context)
        {
            this.context = context;
            playerController.gameObject.SetActive(false);
            enemyController.gameObject.SetActive(false);
            turnsUI.gameObject.SetActive(false);
            experiencePointsUI.gameObject.SetActive(false);
            placementManager.gameObject.SetActive(true);
            levelProgress?.gameObject.SetActive(false);
            PlacementPhaseStarted?.Invoke();
        }

        public override void Next()
        {
            PlacementPhaseEnded?.Invoke();
            placementManager.gameObject.SetActive(false);
            isInPlacementPhase = false;
            this.context.Next();
        }
    }
}
