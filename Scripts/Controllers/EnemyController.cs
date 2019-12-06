//-----------------------------------------------------------------------
// <copyright file="EnemyController.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.TurnSystem;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class EnemyController : UnitsController
    {
        public static Action<Unit> UnitSelected;

        [SerializeField]
        private UnitsMap unitsMap;

        [SerializeField]
        private RefPoint itemSelected;

        [SerializeField]
        private PlayerController playerController; //// TODO check if required

        [SerializeField]
        private RefInt enemyTurn;

        private float enemyTurnInitialDelay = 2.5f;

        private float deltaTime = 0.75f;

        private float idleDeltaTime = 0.5f;

        public void Awake()
        {
            this.enemyTurn.Value = 0;
            EntityTurnManager.RegisterEntity(this, TurnEntities.ENEMY);
        }

        public override void Select(Point point)
        {
            Unit selectedUnit = unitsMap.Get(point);
            if (selectedUnit != null && selectedUnit.GetUnitType() == RoboRapture.Units.Type.Enemy)
            {
                selectedUnit?.VoFx?.Play(selectedUnit.transform.position);
            }
        }

        public void CleanSelection()
        {
            this.itemSelected.Value = new Point(-1, 0, -1);
        }

        public override void StartTurn(EntityTurnManager turnManager)
        {
            this.Units = unitsMap.GetUnits(RoboRapture.Units.Type.Enemy);
            this.CleanSelection();
            this.playerController.CleanSelection();

            if (this.TurnManager == null)
            {
                this.TurnManager = turnManager;
            }

            this.enemyTurn.Value++;
            this.Units.ForEach(item => item.Turn = enemyTurn.Value);

            this.StartCoroutine(this.SetOrder());
        }

        public override void EndTurn()
        {
            if (this.TurnManager == null || this.TurnManager.ActiveTurnEntity != TurnEntities.ENEMY)
            {
                return;
            }

            this.CleanSelection();
            this.playerController.CleanSelection();

            this.UnHighlightUnits();
            TurnManager.NextEntityTurn();
        }

        protected override void Execute(Point point)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerator SetOrder()
        {
            yield return new WaitForSeconds(enemyTurnInitialDelay);

            List<Unit> sorteredUnits = this.Units.OrderBy(u => ((EnemyUnit)u).Speed).ToList();
            foreach (var enemyUnit in sorteredUnits)
            {
                if (enemyUnit == null || enemyUnit.Health == null || enemyUnit.Health.IsDead() || enemyUnit.StructureType == StructureType.BODY_PART || enemyUnit.Health.GetTotalHealth() > 100 || !enemyUnit.gameObject.activeSelf)
                {
                    continue;
                }

                UnitSelected?.Invoke(this.SelectedUnit);
                this.UpdatePreviousUnit();
                this.SelectedUnit = enemyUnit;
                this.SelectedUnit.ChangeState(StateType.Selected);

                EnemyUnit enemy = (EnemyUnit)SelectedUnit;
                enemy.IdleFx?.Play(this.transform.position);
                yield return new WaitForSeconds(idleDeltaTime);

                yield return enemy.Execute(LevelManager.BoardController, LevelManager.GetBoard(TerrainSystem.TerrainNavigationType.BOTH));
                yield return new WaitForSeconds(this.deltaTime);
            }

            UnitSelected?.Invoke(null);
            this.EndTurn();
        }
    }
}
