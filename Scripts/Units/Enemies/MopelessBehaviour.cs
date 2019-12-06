//-----------------------------------------------------------------------
// <copyright file="MopelessBehaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.TileAuxillary;
    using UnityEngine;

    public class MopelessBehaviour : Behaviour
    {
        private EnemyUnit unit;

        private void Awake()
        {
            unit = this.GetComponent<EnemyUnit>();
        }

        public override IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            this.unit.Board = board;
            List<Point> validPositions = GetMovementValidPositions();

            TryToAttack(boardController);
            boardController?.SwitchTilesFromActiveBoards(new HashSet<Point>(validPositions), TileStates.HIGHLIGHT);
            yield return new WaitForSeconds(1.5f);

            Move(this.unit.Target == null ? default : this.unit.Target.GetPosition());
            boardController?.ClearAllActiveBoardsDecorations();

            this.unit.Target = null;
            yield return new WaitForSeconds(1f);
        }

        private List<Point> GetMovementValidPositions()
        {
            this.unit.ActionsHandler.ActivateAction(0);
            return this.unit.ActionsHandler.GetActions()[0].GetValidTargets(this.unit.Board, this.unit.GetPosition());
        }

        private void TryToAttack(BoardController boardController)
        {
            Dictionary<Point, List<Point>> allPaths = AIUtils.GetAllPaths(this.unit.Board, this.unit.UnitsMap, this.unit.GetPosition());
            Unit targetInLine = AIUtils.GetUnitInLine(this.unit.UnitsMap, this.unit.GetPosition(), allPaths);
            if (targetInLine != null)
            {
                this.unit.Target = targetInLine;
                //// Logcat.I(this, $"Target identified {targetInLine.GetPosition()}");
                Attack(boardController);
            }
        }

        private void Move(Point enemyToAvoid)
        {
            this.unit.ActionsHandler.GetActions()[0].Execute(enemyToAvoid);
        }

        private void Attack(BoardController boardController)
        {
            if (this.unit.Target == null)
            {
                return;
            }

            this.unit.ActionsHandler.ActivateAction(1);
            this.unit.ActionsHandler.GetActions()[1].BoardController = boardController;
            this.unit.ActionsHandler.GetValidTargets(this.unit.Board, this.unit.GetPosition());
            this.unit.ActionsHandler.ValidateAction(this.unit.Target.GetPosition());
            this.unit.ActionsHandler.Execute(this.unit.Target.GetPosition());
            this.gameObject.GetComponentInChildren<AnimationStateUpdater>().PlayAttackAnimation(1);
        }
    }
}