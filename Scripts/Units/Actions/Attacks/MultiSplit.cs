//-----------------------------------------------------------------------
// <copyright file="HoloBlast.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Attacks
{
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.Scriptables;
    using NaughtyAttributes;
    using UnityEngine;

    public class MultiSplit : HoloBlast
    {
        [SerializeField]
        private int maxSplits = 15;

        [ReadOnly]
        public static int InstantiatedSplits = 0;

        [SerializeField]
        private MultiSplit split;

        private int firstAttackDamage;

        private int knockback;

        private Point attackerPosition;

        private float fixedPosition = 1.5f;

        public override void SetUp(UnitsMap map, BoardController boardController, Point attackerPosition, CardinalDirections[] blastSplit, int firstAttackDamage, int secondAttackDamage, int knockback)
        {
            this.unitsMap = map;
            this.boardController = boardController;
            this.blastSplit = blastSplit;
            this.firstAttackDamage = firstAttackDamage;
            this.secondAttackDamage = secondAttackDamage;
            this.knockback = knockback;
            this.attackerPosition = attackerPosition;

            DamageOnCollision bullet = this.GetComponent<DamageOnCollision>();
            bullet.UnitsMap = map;
            bullet.BoardController = boardController;
            bullet.AttackerPoint = attackerPosition;
            bullet.Knockback = knockback;
            bullet.Damage = firstAttackDamage;
            bullet.DestroyComponent = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();
            Vector3 otherPosition = collision.gameObject.transform.position;
            if (otherPosition == PointConverter.ToVector(attackerPosition))
            {
                return;
            }

            if (unit == null)
            {
                MultiSplit.Destroy(this.gameObject);
                return;
            }

            this.SpawnSplit(unitsMap, boardController, blastSplit[0], unit.GetPosition());
            this.SpawnSplit(unitsMap, boardController, blastSplit[1], unit.GetPosition());
            MultiSplit.Destroy(this.gameObject);
        }

        private void SpawnSplit(UnitsMap map, BoardController boardController, CardinalDirections direction, Point attackerPosition)
        {
            if (InstantiatedSplits >= maxSplits)
            {
                return;
            }

            InstantiatedSplits++;
            Logcat.I(this, $"Multisplit instances {InstantiatedSplits}");

            CardinalDirections[] directions = PlayerUtils.HoloBlastSplitDirections(direction);
            Vector3 position = PointConverter.ToVector(attackerPosition) + PointConverter.ToVector(Direction.GetDirection(direction));
            position.y = fixedPosition;
            Quaternion rotation = RotationHelper.GetRotation(direction);
            MultiSplit instance = Instantiate(split, position, rotation);
            instance.SetUp(this.unitsMap, boardController, attackerPosition, directions, secondAttackDamage, secondAttackDamage, knockback);
        }
    }
}