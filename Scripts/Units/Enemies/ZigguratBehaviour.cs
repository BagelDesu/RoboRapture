//-----------------------------------------------------------------------
// <copyright file="ZigguratBehaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using UnityEngine;

    public class ZigguratBehaviour : Behaviour
    {
        private EnemyUnit unit;

        public void Start()
        {
            this.unit = this.GetComponent<EnemyUnit>();
        }

        public override IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            this.unit.Board = board;
            this.GetAttackPositions(boardController);
            yield return new WaitForSeconds(0.10f);

            this.Attack();
            yield return new WaitForSeconds(3f);
        }

        private List<Point> GetAttackPositions(BoardController boardController)
        {
            this.unit.ActionsHandler.ActivateAction(0);
            this.unit.ActionsHandler.GetActions()[0].BoardController = boardController;
            return this.unit.ActionsHandler.GetValidTargets(this.unit.Board, this.unit.GetPosition());
        }

        private void Attack()
        {
            this.gameObject.GetComponentInChildren<AnimationStateUpdater>().PlayAttackAnimation(1);
            this.unit.ActionsHandler.Execute();
        }
    }
}