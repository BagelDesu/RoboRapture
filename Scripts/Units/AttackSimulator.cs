//-----------------------------------------------------------------------
// <copyright file="AttackSimulator.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.UI;
    using UnityEngine;

    public class AttackSimulator : MonoBehaviour
    {
        [SerializeField]
        private GameObject knockbackArrow;

        [SerializeField]
        private ChicletsUI chicletsUI;

        [SerializeField]
        private HealthBarDisplayer displayer;

        private KnockbackHandler handler;

        private Unit unit;

        private KnockbackDecalBuilder builder;

        private void OnEnable()
        {
            unit = GetComponent<Unit>();
            handler = new KnockbackHandler(unit.UnitsMap);
            Selector.CancelledAction += OnActionCancelled;
        }

        private void OnDisable()
        {
            Selector.CancelledAction -= OnActionCancelled;
            Clean();
        }

        private void OnActionCancelled()
        {
            Clean();
        }

        public void Simulate(BoardController boardcontroller, Point attackerPosition, int knockback, float damage, bool isSustractive)
        {
            //// Logcat.W(this, $"Simulate attacker position {attackerPosition}, knockback {knockback}, damage {damage}, is sustractive? {isSustractive}");
            if (knockback != 0 && this.unit.IsAffectedByKnockback)
            {
                builder?.DestroyInstances();
                builder = new KnockbackDecalBuilder(knockbackArrow);
                handler.IsInverse = knockback < 0;
                CardinalDirections direction = handler.GetKnockbackDirection(boardcontroller, attackerPosition, unit.GetPosition());
                builder.Instanciate(unit.GetPosition(), direction);
            }

            displayer.Show(true);
            chicletsUI?.SimulateAttack(unit.Health, (int) damage, isSustractive);
        }

        public void Clean()
        {
            chicletsUI?.UpdateHealth(unit.Health);
            builder?.DestroyInstances();
        }
    }
}