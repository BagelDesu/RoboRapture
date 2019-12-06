//-----------------------------------------------------------------------
// <copyright file="DamageOnCollision.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Attacks
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class DamageOnCollision : MonoBehaviour
    {
        private int damage;

        private bool destroyComponent = true;

        private int knockback = 1;

        private UnitsMap unitsMap;

        private BoardController boardController;

        private Point attackerPoint;

        public int Damage { get => this.damage; set => this.damage = value; }

        public bool DestroyComponent { get => this.destroyComponent; set => this.destroyComponent = value; }

        public int Knockback { get => knockback; set => knockback = value; }

        public BoardController BoardController { get => boardController; set => boardController = value; }

        public Point AttackerPoint { get => attackerPoint; set => attackerPoint = value; }

        public UnitsMap UnitsMap { get => unitsMap; set => unitsMap = value; }

        private void OnCollisionEnter(Collision other)
        {
            Unit unit = other.gameObject.GetComponent<Unit>();
            Logcat.I($"On collision enter {this.damage}, knockback info attacker {attackerPoint} unit {unit?.GetPosition()} knockback {knockback}");
            if (unit == null)
            {
                return;
            }

            KnockbackHandler handler = new KnockbackHandler(unitsMap);
            handler.Execute(boardController, attackerPoint, unit.GetPosition(), knockback);

            unit.Health.ReduceHealth(this.damage);

            if (this.destroyComponent)
            {
                DamageOnCollision.Destroy(this.gameObject);
            }
        }
    }
}