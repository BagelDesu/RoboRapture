//-----------------------------------------------------------------------
// <copyright file="NeoSatanHeadPukeAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using UnityEngine;

    public class NeoSatanHeadPukeAction : SkillAction
    {
        [SerializeField]
        private FXWrapper pukeSFx;

        private Point strongestTarget;

        private List<Point> targets;

        private CardinalDirections cardinalDirection;

        private CardinalDirections cardinalDirectionToRotate;

        public override List<Point> GetValidTargets(List<Point> board, Point position)
        {
            Point neoSatanPosition = this.Unit.GetPosition();
            base.GetValidTargets(board, position);
            this.ValidPositions = this.ValidPositions.Where(p => p.x >= (neoSatanPosition.x - 1) && p.x <= (neoSatanPosition.x + 1)
                                                                || p.z >= (neoSatanPosition.z - 1) && p.z <= (neoSatanPosition.z + 1)).ToList();

            strongestTarget = AIUtils.GetStrongestTarget(this.UnitsMap);
            targets = this.GetRowsToAttack(strongestTarget);
            Logcat.I(this, $"NeoSatanHeadPukeAction target {strongestTarget}");
            return targets;
        }

        public override void Execute()
        {
            Logcat.I(this, $"NeoSatanHeadPukeAction Puking");
            Logcat.I(this, $"NeoSatanHeadPukeAction strongest target {strongestTarget}: {Unit.UnitsMap.Get(strongestTarget)?.UnitName}");
            Logcat.I(this, $"NeoSatanHeadPukeAction puke direction {cardinalDirection}");
            targets.ForEach(p => Logcat.I(this, $"NeoSatanHeadPukeAction targets {p}"));
            PlayParticles(strongestTarget);
            targets.ForEach(p => this.AttackPosition(p));
        }

        private List<Point> GetRowsToAttack(Point target)
        {
            Point neoSatanPosition = this.Unit.GetPosition();
            cardinalDirection = Direction.GetCardinalDirection(this.Unit.GetPosition(), target);
            List<Point> rowsToAttack = new List<Point>();
            switch (cardinalDirection)
            {
                case CardinalDirections.South:
                case CardinalDirections.East:
                case CardinalDirections.SouthWest:
                case CardinalDirections.NorthEast:
                    cardinalDirectionToRotate = CardinalDirections.South;
                    rowsToAttack = this.ValidPositions.Where(p => p.z < neoSatanPosition.z && p.x >= neoSatanPosition.x - 1 && p.x <= neoSatanPosition.x + 1).ToList();
                    break;
                case CardinalDirections.North:
                case CardinalDirections.West:
                case CardinalDirections.NorthWest:
                case CardinalDirections.SouthEast:
                    cardinalDirectionToRotate = CardinalDirections.West;
                    rowsToAttack = this.ValidPositions.Where(p => p.x < neoSatanPosition.x && p.z >= neoSatanPosition.z - 1 && p.z <= neoSatanPosition.z + 1).ToList();
                    break;
            }

            return rowsToAttack;
        }

        private void AttackPosition(Point target)
        {
            Unit targetUnit = this.Unit.UnitsMap.Get(target);
            if (targetUnit == null)
            {
                return;
            }

            if ((targetUnit is EnemyUnit) && ((EnemyUnit)targetUnit).GetSpawnedUnitType() == UnitType.NEOSATAN_LEG)
            {
                return;
            }

            KnockbackHandler handler = new KnockbackHandler(this.UnitsMap);
            handler.Execute(this.BoardController, this.Unit.GetPosition(), targetUnit.GetPosition(), this.Knockback);
            targetUnit?.Health.ReduceHealth(this.DeltaHealth);
            //// Logcat.I($"attacking position {target}, is a unit in the position? {targetUnit != null}");
        }

        private void PlayParticles(Point target)
        {
            Quaternion direction = RotationHelper.GetRotation(cardinalDirectionToRotate);
            Unit.FlipUnit(cardinalDirection);
            SkillActionFX?.Play(this.Unit.transform.position);
            SkillActionFX.UpdateParticlesRotation(direction);
            pukeSFx?.Play(this.transform.position);
        }
    }
}