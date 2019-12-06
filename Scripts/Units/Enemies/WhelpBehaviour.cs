//-----------------------------------------------------------------------
// <copyright file="WhelpBehaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{ 
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.TileAuxillary;
    using UnityEngine;

    public class WhelpBehaviour : Behaviour
    {
        [SerializeField]
        private RefInt spawnedWhelps;

        private bool deadTracked = false;

        private EnemyUnit unit;

        public override IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            if (this.unit == null)
            {
                yield return null;
            }

            unit.Board = board;

            //MOVE, highlight tiles, move
            List<Point> validPositions = GetMovementValidPositions();
            boardController?.SwitchTilesFromActiveBoards(new HashSet<Point>(validPositions), TileStates.HIGHLIGHT);

            yield return new WaitForSeconds(0.5f);

            Move();
            boardController?.ClearAllActiveBoardsDecorations();
            yield return new WaitForSeconds(1f);

            //ATTACK
            Attack();
            yield return new WaitForSeconds(1f);
        }

        public void WhelpDestroyed()
        {
            if (this.unit.Health.IsDead() && !deadTracked && this.spawnedWhelps != null)
            {
                this.spawnedWhelps.Value--;
                deadTracked = true;
            }
        }

        private void Awake()
        {
            unit = this.GetComponent<EnemyUnit>();
            if (this.spawnedWhelps != null)
            {
                spawnedWhelps.Value++;
            }
        }
        
        private List<Point> GetMovementValidPositions()
        {
            this.unit.ActionsHandler.ActivateAction(0);
            return this.unit.ActionsHandler.GetActions()[0].GetValidTargets(this.unit.Board, this.unit.GetPosition());
        }

        private void Move()
        {
            this.unit.ActionsHandler.GetActions()[0].Execute(new Point(0, 0, 0));
        }

        private void Attack()
        {
            if (this.unit.Target == null)
            {
                return;
            }

            this.unit.ActionsHandler.ActivateAction(1);
            this.unit.ActionsHandler.Execute(this.unit.Target.GetPosition());
            this.gameObject.GetComponentInChildren<AnimationStateUpdater>().PlayAttackAnimation(this.unit.ActionsHandler.ActionsIndex.Value);

            this.unit.Target = null;
        }
    }
}