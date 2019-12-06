//-----------------------------------------------------------------------
// <copyright file="SkillAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Environment;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.TileAuxillary;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class SkillAction : Action
    {
        public static System.Action<SkillAction, Point> SkillActionExecuted;

        [SerializeField]
        private UnitsMap unitsMap;

        [SerializeField]
        private float deltaHealth;

        [SerializeField]
        private string damageDescription;

        [SerializeField]
        private int knockback;

        [SerializeField]
        private string knockbackDescription;

        [SerializeField]
        private FXWrapper skillActionFX;

        private Vector3 skillFxPosition;

        private Point target;

        public Point Target { get => this.target; set => this.target = value; }

        public float DeltaHealth { get => this.deltaHealth; set => this.deltaHealth = value; }

        public int Knockback { get => this.knockback; set => this.knockback = value; }

        public string KnockbackDescription { get => this.knockbackDescription; private set => this.knockbackDescription = value; }

        public UnitsMap UnitsMap { get => this.unitsMap; private set => this.unitsMap = value; }

        public FXWrapper SkillActionFX { get => skillActionFX; private set => skillActionFX = value; }

        public Vector3 SkillFxPosition { get => skillFxPosition; set => skillFxPosition = value; }

        public string DamageDescription { get => damageDescription; private set => damageDescription = value; }

        public void OnEnable()
        {
            this.skillFxPosition = this.transform.position;
            TileHovered.TileHoveredOn += this.OnTileHoveredOn;
            TileHovered.TileHoveredOff += this.OnTileHoveredOff;
            SelectableHovered.UnitHoveredOn += this.UnitSelectedOn;
            SelectableHovered.UnitHoveredOff += this.UnitSelectedOff;
        }

        public void OnDisable()
        {
            TileHovered.TileHoveredOn -= this.OnTileHoveredOn;
            TileHovered.TileHoveredOff -= this.OnTileHoveredOff;
            SelectableHovered.UnitHoveredOn -= this.UnitSelectedOn;
            SelectableHovered.UnitHoveredOff -= this.UnitSelectedOff;
        }

        public void OnTileHoveredOn(Tile tile)
        {
            HighlightTileAttack(true, tile.GetPosition());
        }

        public void OnTileHoveredOff(Tile tile)
        {
            HighlightTileAttack(false, tile.GetPosition());
        }

        public void UnitSelectedOn(Unit unit)
        {
            HighlightTileAttack(true, unit.GetPosition());
        }

        public void UnitSelectedOff(Unit unit)
        {
            HighlightTileAttack(false, unit.GetPosition());
        }

        public override bool ValidateAction(Point target)
        {
            return base.ValidateAction(target);
        }

        public override void Execute(Point target)
        {
            if (target == default)
            {
                return;
            }

            this.target = target;
            FaceTargetDirection(target);
            skillActionFX?.Play(this.skillFxPosition);

            Logcat.I(this, $"Action executed {Unit.UnitName} - {SkillName}, target {target}");
            SkillActionExecuted?.Invoke(this, target);
        }

        public virtual void Execute()
        {
            Logcat.I(this, $"Action executed {Unit.UnitName} - {SkillName}, target {target}");
            SkillActionExecuted?.Invoke(this, this.target);
        }

        protected virtual void HighlightTileAttack(bool highlight, Point position)
        {
            if (this.Unit.GetUnitType() == Type.Player && this.IsActive() && this.ValidPositions != null && this.ValidPositions.Contains(position))
            {
                Logcat.I(this, $"OnTileHovered {highlight} - {position}");
                HashSet<Point> points = new HashSet<Point>();
                points.Add(position);
                BoardController.SwitchTilesFromActiveBoards(points, highlight ? TileStates.TARGET: TileStates.ATTACK);
                TargetUnit(highlight, position);
            }
        }

        protected void TargetUnit(bool highlight, Point position)
        {
            Unit unit = unitsMap.Get(position);
            unit?.ChangeState(highlight? StateType.Targeted : StateType.Normal);
        }

        protected void SimulateAttack(bool highlight, Point position, float deltaHealth, int knockback, bool isSustractive)
        {
            Unit target = UnitsMap.Get(position);
            if (BoardController == null || this.Unit == null || target == null || !this.ValidPositions.Contains(position) || !this.IsActive())
            {
                return;
            }

            if (highlight)
            {
                target.GetAttackSimulator()?.Simulate(this.BoardController, this.Unit.GetPosition(), knockback, deltaHealth, isSustractive);
            }
            else
            {
                target?.GetAttackSimulator()?.Clean();
            }
        }
    }
}
