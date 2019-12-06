//-----------------------------------------------------------------------
// <copyright file="PhantasmBehaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Managers;
    using UnityEngine;

    public class PhantasmBehaviour : Behaviour
    {
        private EnemyUnit unit;

        private int movementIndex = 0;

        private int actionIndex = 1;

        public void Start()
        {
            SetUnit();
            ActivateAction(movementIndex);
            ActivateAction(actionIndex);
        }

        public override IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            yield return new WaitForSeconds(2f);
        }

        private void SetUnit()
        {
            this.unit = this.GetComponent<EnemyUnit>();
            LevelManager levelManager = FindObjectsOfType<LevelManager>()[0];
            this.unit.Board = levelManager.GetBoard(TerrainSystem.TerrainNavigationType.BOTH);
        }

        private void ActivateAction(int actionIndex)
        {
            this.unit.ActionsHandler.ActivateAction(actionIndex);
            this.unit.ActionsHandler.GetActions()[actionIndex].GetValidTargets(this.unit.Board, this.unit.GetPosition());
            this.unit.ActionsHandler.Execute(default);
        }
    }
}