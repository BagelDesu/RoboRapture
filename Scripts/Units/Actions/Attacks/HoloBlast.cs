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
    using UnityEngine;

    public class HoloBlast : MonoBehaviour
    {
        private readonly float fixedHeight = 1.5f;

        [SerializeField]
        private HoloBlastSplit blast;

        protected UnitsMap unitsMap;

        protected CardinalDirections[] blastSplit;

        protected BoardController boardController;

        protected int secondAttackDamage;

        private int secondKnockback;

        public virtual void SetUp(UnitsMap map, BoardController boardController, Point attackerPosition, CardinalDirections[] blastSplit, int firstAttackDamage, int secondAttackDamage, int knockback)
        {
            this.unitsMap = map;
            this.boardController = boardController;
            this.blastSplit = blastSplit;
            this.secondAttackDamage = secondAttackDamage;
            this.secondKnockback = knockback;
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
            Vector3 otherPosition = collision.gameObject.transform.position;
            otherPosition.y = this.fixedHeight;

            this.SpawnSplit(this.blastSplit[0], otherPosition);
            this.SpawnSplit(this.blastSplit[1], otherPosition);
            HoloBlast.Destroy(this.gameObject);
        }

        private void SpawnSplit(CardinalDirections direction, Vector3 otherPosition)
        {
            Vector3 position = otherPosition + PointConverter.ToVector(Direction.GetDirection(direction));

            Quaternion rotation = RotationHelper.GetRotation(direction);
            HoloBlastSplit bullet = Instantiate(this.blast, position, rotation);
            Logcat.I($"Other position {otherPosition} Splitted bullet {position} rotation {rotation}");
            bullet.SetUp(boardController, unitsMap, this.secondAttackDamage, secondKnockback, new Point((int)otherPosition.x, 0, (int)otherPosition.z));
        }
    }
}