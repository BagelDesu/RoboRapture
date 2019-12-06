//-----------------------------------------------------------------------
// <copyright file="UdeukedefrukeAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units.Enemies;
    using Edu.Vfs.RoboRapture.Validators;
    using System.Collections.Generic;
    using UnityEngine;

    public class UdeukedefrukeAction : SkillAction
    {
        [SerializeField]
        private Unit udeukedefrukeMaskPrefab;

        [SerializeField]
        private Unit udeukedefrukeIntestinePrefab;

        private Unit mask;

        private List<Unit> intestines;

        private Point maskPosition;

        private bool extrude = true;

        public override void Execute(Point target)
        {
            if (target != default)
            {
                extrude = true;
                this.maskPosition = target;
            }
            else
            {
                extrude = false;
            }
        }

        public override void Execute()
        {
            SkillActionFX?.Play(this.transform.position);
            if (extrude)
            {
                Extrude();
            }
            else
            {
                SuckIn();
            }
        }

        private void Extrude()
        {
            Logcat.I(this, $"Extruding {this.transform}, {this.maskPosition}");
            PlaceMask(); ;
            PlaceIntestines();
        }

        private void PlaceMask()
        {
            mask = AIPlacementHelper.AddUnit(this.transform.parent, maskPosition, udeukedefrukeMaskPrefab);
            mask.GetComponent<UdeukedefrukeBodyBehaviour>().SetUp(this.Unit.Health);
        }

        private void PlaceIntestines()
        {
            this.intestines = new List<Unit>();
            List<Point> middlePoints = PointUtils.GetVerticalMiddlePoints(this.Unit.GetPosition(), this.maskPosition);
            foreach (var item in middlePoints)
            {
                Unit intestine = AIPlacementHelper.AddUnit(this.transform.parent, item, udeukedefrukeIntestinePrefab);
                intestines.Add(intestine);
            }
        }

        private void SuckIn()
        {
            if (mask == null)
            {
                return;
            }

            Point returningPosition = mask.GetPosition();
            bool removed = RemoveBodyParts(mask);
            if (removed)
            {
                PlacementHelper.Move(this.Unit, returningPosition, new MovementActionValidator());
                this.intestines.ForEach(i => RemoveBodyParts(i));
                this.intestines = null;
            }
        }

        private bool RemoveBodyParts(Unit unit)
        {
            if (unit == null)
            {
                Logcat.I(null, "unable to Remove");
                return false;
            }

            unit?.UnitsMap.Remove(unit.GetPosition());
            MonoBehaviour.Destroy(unit.gameObject);
            return true;
        }
    }
}
