//-----------------------------------------------------------------------
// <copyright file="DefensiveShotAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;
    using System.Collections;
    using UnityEngine;

    public class DefensiveShotAction : SkillAction
    {
        [SerializeField]
        private float shotSpeed = 0.2f;

        [SerializeField]
        private float shotHeight = 2.5f;

        public override bool ValidateAction(Point target)
        {
            Logcat.I(this, $"Telekinesis blast validating action");
            return base.ValidateAction(target) && this.UnitsMap.Get(target) != this.Unit;
        }

        public override void Execute(Point target)
        {
            if (target == default)
            {
                return;
            }

            this.Target = target;
            FaceTargetDirection(target);
            SkillActionExecuted?.Invoke(this, target);
        }

        public override void Execute()
        {
            if (!ValidateAction(this.Target))
            {
                return;
            }

            StartCoroutine(ShotTrajectory());
        }

        private IEnumerator ShotTrajectory()
        {
            Vector3 destination = PointConverter.ToVector(this.Target);
            float progress = 0;
            var startPos = PointConverter.ToVector(this.Unit.GetPosition());
            SkillActionFX.Play(startPos);
            while (progress <= 1.0)
            {
                progress += Time.deltaTime / shotSpeed;
                var height = Mathf.Sin(Mathf.PI * progress) * shotHeight;
                if (height < 0f)
                {
                    height = 0f;
                }
                SkillActionFX.UpdateParticlesPosition( Vector3.Lerp(startPos, destination, progress) + Vector3.up * height);
                yield return null;
            }

            AttackTarget();

            SkillActionFX.UpdateParticlesPosition(destination);
            SkillActionFX.DestroyInstance();
        }

        private void AttackTarget()
        {
            Unit targetUnit = this.UnitsMap.Get(this.Target);
            if (targetUnit != null)
            {
                SimulateAttack(false, this.Target, this.DeltaHealth, this.Knockback, true);
                KnockbackHandler handler = new KnockbackHandler(this.UnitsMap);
                handler.Execute(this.BoardController, this.Unit.GetPosition(), targetUnit.GetPosition(), this.Knockback);
                targetUnit?.Health.ReduceHealth(this.DeltaHealth);
            }

            this.IsActive(false);
            Logcat.I(this, "Telekinesis blast executed");
        }

        protected override void HighlightTileAttack(bool highlight, Point position)
        {
            base.HighlightTileAttack(highlight, position);
            if (position != this.Unit.GetPosition())
            {
                SimulateAttack(highlight, position, this.DeltaHealth, this.Knockback, true);
            }
        }
    }
}