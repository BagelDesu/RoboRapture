//-----------------------------------------------------------------------
// <copyright file="IncarnateBehaviour.cs" company="VFS">
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
    using Edu.Vfs.RoboRapture.Units.Actions.Enemy;
    using UnityEngine;

    public class IncarnateBehaviour : Behaviour
    {
        private int movementActionIndex = 0;

        private int afterbirthActionIndex = 1;

        private int rebirthActionIndex = 2;

        private EnemyUnit unit;

        private SpawnSystem.UnitType lastEnemyKilled;

        private bool wasAnEnemyKilled = false;

        private AnimationStateUpdater animationStateUpdater;

        private IncarnateRebirthAttackAction rebirthAction;

        private void OnEnable()
        {
            unit = this.GetComponent<EnemyUnit>();
            rebirthAction = this.GetComponentInChildren<IncarnateRebirthAttackAction>();
            animationStateUpdater = this.GetComponentInChildren<AnimationStateUpdater>();
            EnemyUnit.EnemyWithTypeDied += EnemyDied;
        }

        private void OnDisable()
        {
            EnemyUnit.EnemyWithTypeDied -= EnemyDied;
        }

        public override IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            this.unit.Board = board;

            List<Point> validPositions = GetMovementValidPositions();
            boardController?.SwitchTilesFromActiveBoards(new HashSet<Point>(validPositions), TileStates.HIGHLIGHT);
            yield return new WaitForSeconds(0.5f);
            Move();
            yield return new WaitForSeconds(1f);

            Logcat.I(this, $"Incarnate behaviour. Was an enemy killed? {wasAnEnemyKilled} was an excluded enemy? {rebirthAction.IsAnExcludedEnemy()}, enemy {lastEnemyKilled}");
            if (wasAnEnemyKilled && !rebirthAction.IsAnExcludedEnemy())
            {
                Attack(rebirthActionIndex);
                wasAnEnemyKilled = false;
            }
            else
            {
                Attack(afterbirthActionIndex);
            }

            yield return new WaitForSeconds(1f);
        }

        private List<Point> GetMovementValidPositions()
        {
            this.unit.ActionsHandler.ActivateAction(movementActionIndex);
            return this.unit.ActionsHandler.GetActions()[movementActionIndex].GetValidTargets(this.unit.Board, this.unit.GetPosition());
        }

        private void Move()
        {
            this.unit.ActionsHandler.GetActions()[movementActionIndex].Execute(default);
        }

        private void Attack(int index)
        {
            this.unit.ActionsHandler.ActivateAction(index);
            this.unit.ActionsHandler.Execute(this.unit.GetPosition());
            this.animationStateUpdater.PlayAttackAnimation(this.unit.ActionsHandler.ActionsIndex.Value);
        }

        private void EnemyDied(Point point, SpawnSystem.UnitType enemy)
        {
            this.lastEnemyKilled = enemy;
            this.wasAnEnemyKilled = true;
        }
    }
}
