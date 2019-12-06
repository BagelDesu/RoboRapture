//-----------------------------------------------------------------------
// <copyright file="Spawner.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Debbugging
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.Units;
    using NaughtyAttributes;
    using UnityEngine;

    public class Spawner : MonoBehaviour
    {
        [Dropdown("enemies")]
        public UnitType EnemyType;

        [Label("[1] Whelp")]
        [SerializeField]
        private Unit whelpPrefab;

        [Label("[2] Mopeless")]
        [SerializeField]
        private Unit mopelessPrefab;

        [Label("[3] Udeukedefruke")]
        [SerializeField]
        private Unit udeukedefrukePrefab;

        [Label("[4] Ziggurat")]
        [SerializeField]
        private Unit zigguratPrefab;

        [Label("[5] Testament")]
        [SerializeField]
        private Unit testamentPrefab;

        [Label("[6] Phantasm")]
        [SerializeField]
        private Unit phantasmPrefab;

        [Label("[7] Incarnate")]
        [SerializeField]
        private Unit incarnatePrefab;

        [Label("[8] NeoSatan")]
        [SerializeField]
        private Unit neoSatanPrefab;

        [SerializeField]
        private int xPosition;

        [SerializeField]
        private int yPosition;

        private Unit keyboardUnit;

        private UnitType[] enemies = new UnitType[]
       {
            UnitType.WHELP,
            UnitType.MOPELESS,
            UnitType.UDEUKE,
            UnitType.ZIGGURAT,
            UnitType.TESTAMENT,
            UnitType.PHANTASM,
            UnitType.INCARNATE,
            UnitType.NEOSATAN_HEAD
       };

        [Button("Spawn Enemy")]
        public void Spawn()
        {
            AIPlacementHelper.AddUnit(this.transform, new Point(this.xPosition, 0, this.yPosition), this.GetUnit(this.EnemyType));
        }
        
        public void UpdatePoint(Point point)
        {
            this.xPosition = point.x;
            this.yPosition = point.z;

            if (keyboardUnit != null && !whelpPrefab.UnitsMap.Contains(point))
            {
                AIPlacementHelper.AddUnit(this.transform, new Point(this.xPosition, 0, this.yPosition), keyboardUnit);
                keyboardUnit = null;
            }
        }

        private Unit GetUnit(UnitType type)
        {
            Unit unit = this.whelpPrefab;
            switch (type)
            {
                case UnitType.WHELP:
                    unit = this.whelpPrefab;
                    break;
                case UnitType.MOPELESS:
                    unit = this.mopelessPrefab;
                    break;
                case UnitType.UDEUKE:
                    unit = this.udeukedefrukePrefab;
                    break;
                case UnitType.ZIGGURAT:
                    unit = this.zigguratPrefab;
                    break;
                case UnitType.TESTAMENT:
                    unit = this.testamentPrefab;
                    break;
                case UnitType.PHANTASM:
                    unit = this.phantasmPrefab;
                    break;
                case UnitType.INCARNATE:
                    unit = this.incarnatePrefab;
                    break;
                case UnitType.NEOSATAN_HEAD:
                    unit = this.neoSatanPrefab;
                    break;
            }

            return unit;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                keyboardUnit = GetUnit(UnitType.WHELP);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                keyboardUnit = GetUnit(UnitType.MOPELESS);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                keyboardUnit = GetUnit(UnitType.UDEUKE);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                keyboardUnit = GetUnit(UnitType.ZIGGURAT);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                keyboardUnit = GetUnit(UnitType.TESTAMENT);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                keyboardUnit = GetUnit(UnitType.PHANTASM);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                keyboardUnit = GetUnit(UnitType.INCARNATE);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                keyboardUnit = GetUnit(UnitType.NEOSATAN_HEAD);
            }
        }
    }
}