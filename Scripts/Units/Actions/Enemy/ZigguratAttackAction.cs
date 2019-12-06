//-----------------------------------------------------------------------
// <copyright file="ZigguratAttackAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.Validators;
    using UnityEngine;

    public class ZigguratAttackAction : SkillAction
    {
        [SerializeField]
        private GameObject particlesIdlePrefab;

        [SerializeField]
        private float particlesIdleHeight = 1f;

        [SerializeField]
        private float delayToAttack = 2f;

        [SerializeField]
        private float damageToZigguratWhenMinionAttacked = 1;

        [SerializeField]
        private GameObject pentagramPrefab;

        private List<GameObject> particles;

        private List<Unit> immaculatedUnits;

        private GameObject pentagram;

        public void Start()
        {
            this.immaculatedUnits = new List<Unit>();
        }

        public override List<Point> GetValidTargets(List<Point> board, Point position)
        {
            base.GetValidTargets(board, position);
            this.SpawnIdleParticles();
            return this.ValidPositions;
        }

        public void Immolation()
        {
            if (this.Unit == null || (this.Unit != null && !this.Unit.Health.IsDead()) || this.immaculatedUnits == null || (this.immaculatedUnits != null && this.immaculatedUnits.Count <= 0))
            {
                return;
            }

            this.immaculatedUnits?.ForEach(unit => unit?.GetComponent<OnHealthValueChanged>()?.Dettach(this.ReduceZigguratHealth));
            this.immaculatedUnits?.ForEach(unit => PlacementHelper.Remove(unit, new RemoveUnitValidator(unit)));
            this.immaculatedUnits = null;
        }

        public override void Execute()
        {
            if (!this.ArePlayerUnitsInRange())
            {
                this.Immaculate();
                return;
            }

            this.Unit.UnitsMap.GetUnits(Type.Player).ForEach(unit => this.SpawnAttackParticles(unit));
            this.Invoke("ReduceHealth", this.delayToAttack);
        }

        private bool ArePlayerUnitsInRange()
        {
            foreach (var item in this.Unit.UnitsMap.GetUnits(Type.Player))
            {
                if (this.ValidPositions.Contains(item.GetPosition()))
                {
                    return true;
                }
            }

            return false;
        }

        private void SpawnIdleParticles()
        {
            this.particles?.ForEach(particle => AIPlacementHelper.RemoveEffect(particle));
            this.particles = new List<GameObject>();
            this.ValidPositions.ForEach(position => this.particles.Add(AIPlacementHelper.AddEffect(this.particlesIdlePrefab, this.gameObject.transform, position, this.particlesIdleHeight)));
        }

        private void SpawnAttackParticles(Unit unit)
        {
            if (!this.ValidPositions.Contains(unit.GetPosition()))
            {
                return;
            }

            this.Target = unit.GetPosition();
            this.FaceTargetDirection(this.Target);
            this.SkillActionFX?.Play(PointConverter.ToVector(this.Target));
        }

        private void ReduceHealth()
        {
            this.Unit.UnitsMap.GetUnits(Type.Player).ForEach(unit => this.ReduceHealthToTarget(unit));
        }

        private void ReduceHealthToTarget(Unit unit)
        {
            if (!this.ValidPositions.Contains(unit.GetPosition()))
            {
                return;
            }

            KnockbackHandler handler = new KnockbackHandler(this.Unit.UnitsMap);
            handler.InverseKnockback(this.BoardController, this.Unit.GetPosition(), unit.GetPosition(), this.Knockback);
        }

        private void Immaculate()
        {
            Unit spawned = SpawnManager.Instance?.SpawnUnitAtRandom(UnitType.TESTAMENT);
            if (spawned == null)
            {
                return;
            }

            spawned?.gameObject.SetActive(false);
            this.immaculatedUnits.Add(spawned);
            pentagram = AIPlacementHelper.AddEffect(pentagramPrefab, this.gameObject.transform, spawned.GetPosition(), 1);
            Invoke("EnableUnit", 1.25f);
        }

        private void EnableUnit()
        {
            Unit lastSpawnedUnit = this.immaculatedUnits[immaculatedUnits.Count -1];
            lastSpawnedUnit.gameObject.SetActive(true);
            lastSpawnedUnit?.GetComponent<OnHealthValueChanged>()?.Attach(this.ReduceZigguratHealth);
            Destroy(pentagram);
        }

        private void ReduceZigguratHealth()
        {
            if (this.Unit == null)
            {
                return;
            }

            this.Unit.Health.ReduceHealth(this.damageToZigguratWhenMinionAttacked);
        }
    }
}