//-----------------------------------------------------------------------
// <copyright file="KnockbackHandler.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Knockbacks
{
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.TerrainSystem;
    using Edu.Vfs.RoboRapture.Units;
    using Edu.Vfs.RoboRapture.Validators;
    using UnityEngine;

    public class KnockbackHandler
    {
        private static float waitInSeconds = 0.15f;

        private UnitsMap map;

        private bool isInverse = false;

        public bool IsInverse { private get => isInverse; set => isInverse = value; }

        public KnockbackHandler(UnitsMap map)
        {
            this.map = map;
        }
        
        public void Execute(BoardController boardController, Point attacker, Point target, int knockBack)
        {
            if (map == null || boardController == null || knockBack == 0)
            {
                return;
            }
            if (map.Contains(target) && !map.Get(target).IsAffectedByKnockback)
            {
                return;
            }

            Logcat.D($"Execute attacker {attacker} target {target} knockback {knockBack}");
            boardController.StartCoroutine(ExecuteKnockback(boardController, attacker, target, knockBack));
        }

        public void InverseKnockback(BoardController boardController, Point attacker, Point target, int knockBack)
        {
            isInverse = true;
            Execute(boardController, attacker, target, knockBack);
        }

        private IEnumerator ExecuteKnockback(BoardController boardController, Point attacker, Point target, int knockBack)
        {
            yield return new WaitForSeconds(waitInSeconds);
            ExecuteAction(boardController, attacker, target, knockBack);
        }

        private void ExecuteAction(BoardController boardController, Point attacker, Point target, int knockBack)
        {
            Logcat.D($"ExecuteAction attacker {attacker} target {target} knockback {knockBack}");
            Point knockbackPosition = GetKnockBackPosition(boardController, attacker, target, knockBack);
            if (knockbackPosition == target)
            {
                return;
            }

            //// Logcat.D($"ExecuteAction attacker {attacker} target {target}, knockbackposition {knockbackPosition} before moving");
            bool moveToNewPosition = CheckImpacts(boardController, attacker, target, knockbackPosition);
            if (moveToNewPosition)
            {
                PlaceTarget(boardController, knockbackPosition, attacker, target, knockBack);
            }
        }

        private void PlaceTarget(BoardController boardController, Point knockbackPosition, Point attacker, Point target, int knockBack)
        {
            Logcat.D($"PlaceTarget knockbackPosition {knockbackPosition} attacker {attacker} target {target} knockback {knockbackPosition}");

            Unit unit = map.Get(target);
            //// Logcat.D($"Move unit {unit.GetPosition()}, new position {knockbackPosition}");
            PlacementHelper.Move(unit, knockbackPosition, new KnockbackValidator());

            List<Point> lakes = boardController.GetAllPointsWithTerrainTypeOf(TileType.ICELAKE);
            if (lakes.Contains(knockbackPosition))
            {
                //// Logcat.D($"****Getting extra movement by landing on a lake {knockbackPosition}");
                int lakeKnockback = knockBack + 1;
                if (lakeKnockback > 12)
                {
                       return;
                }

                boardController.StartCoroutine(ExecuteKnockback(boardController, attacker, unit == null ? default : unit.GetPosition(), lakeKnockback));
            }
        }

        private bool CheckImpacts(BoardController boardController, Point unitAttacker, Point unitTarget, Point knockbackPosition)
        {
            Logcat.D($"CheckImpacts attacker {unitAttacker} target {unitTarget}, is there a unit? {map.Get(unitTarget)}, knockbackposition {knockbackPosition} is there a unit? {map.Get(knockbackPosition)}");
            Unit unit = map.Get(knockbackPosition);
            if (unit == null)
            {
                //// Logcat.D("CheckImpacts returning true");
                return true;
            }

            //// Logcat.D($"unit name {unit.UnitName} position {unit.GetPosition()}");
            IKnockback knockbackedItem = unit.gameObject.GetComponent<IKnockback>();
            if (knockbackedItem != null)
            {
                bool availablePosition = knockbackedItem.Handle(map.Get(unitTarget));
                if (knockbackedItem is Iceberg)
                {
                    ExecuteAction(boardController, unitAttacker, unit.GetPosition(), 1);
                }

                //// Logcat.D($"CheckImpacts returning {availablePosition}");
                return availablePosition;
            }

            //// Logcat.D($"CheckImpacts returning {false}");
            return false;
        }

        private Point GetKnockBackPosition(BoardController boardController, Point attackerPosition, Point targetPosition, int knockBack)
        {
            Logcat.D($"GetKnockBackPosition {attackerPosition} targetPosition {targetPosition} knockback {knockBack}");
            List<Point> board = boardController.GetAllPointsWithNavigationTypeOf(TerrainNavigationType.BOTH);
            //// Logcat.D($"board size with valid points {board.Count}");
            CardinalDirections direction = Direction.GetCardinalDirection(isInverse? targetPosition : attackerPosition, isInverse? attackerPosition : targetPosition);
            CrossPattern crossPattern = new CrossPattern(knockBack, new List<CardinalDirections>() { direction });
            List<Point> positions = PatternProcessor.Process(board, crossPattern, targetPosition, 0);
            Point position = (positions == null || positions.Count == 0) ? targetPosition : positions[0];
            //// positions.ForEach(p => Logcat.D($"knockback positions {p}"));
            //// Logcat.D($"Final Knockback position {position}");
            return position;
        }

        public CardinalDirections GetKnockbackDirection(BoardController boardController, Point attackerPosition, Point targetPosition)
        {
            List<Point> board = boardController.GetAllPointsWithNavigationTypeOf(TerrainNavigationType.BOTH);
            CardinalDirections direction = Direction.GetCardinalDirection(isInverse ? targetPosition : attackerPosition, isInverse ? attackerPosition : targetPosition);
            return board.Contains(targetPosition + Direction.GetDirection(direction)) ? direction : CardinalDirections.Center;
        }
    }
}