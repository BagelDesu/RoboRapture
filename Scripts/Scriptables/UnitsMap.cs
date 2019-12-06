//-----------------------------------------------------------------------
// <copyright file="UnitsMap.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Scriptables
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UnitsMap", menuName = "Units/UnitsMap")]
    public class UnitsMap : ScriptableObject
    {
        private Dictionary<Point, Unit> unitsDictionary = new Dictionary<Point, Unit>();

        public void Clear()
        {
            this.unitsDictionary.Clear();
        }

        public void Add(Point point, Unit unit)
        {
            
            if (this.unitsDictionary.ContainsKey(point))
            {
                this.unitsDictionary.Remove(point);
            }

            this.unitsDictionary.Add(point, unit);
        }

        public bool Remove(Point point)
        {
            return this.unitsDictionary.Remove(point);
        }

        public bool Contains(Point point)
        {
            return this.unitsDictionary.ContainsKey(point);
        }

        public bool Contains(Point point, Type type)
        {
            return this.unitsDictionary.ContainsKey(point) && this.unitsDictionary[point].GetUnitType() == type;
        }

        public Unit Get(Point point)
        {
            return this.unitsDictionary.ContainsKey(point) ? this.unitsDictionary[point] : null;
        }

        public List<Unit> GetUnits(Type type)
        {
            List<Unit> items = new List<Unit>();
            foreach (Unit item in this.unitsDictionary.Values)
            {
                if (item.GetUnitType() == type)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        public List<Point> GetUnits()
        {
            return new List<Point>(this.unitsDictionary.Keys);
        }

        public void Move(Unit unit, Point newPosition)
        {
            Logcat.I($"Moving unit from {unit.GetPosition()} to {newPosition}");
            if (this.unitsDictionary.ContainsKey(newPosition))
            {
                Destroy(this.unitsDictionary[newPosition].gameObject);
                this.unitsDictionary.Remove(newPosition);
            }
            this.unitsDictionary.Remove(unit.GetPosition());

            if(unit.isActiveAndEnabled)
            {
                this.unitsDictionary.Add(newPosition, unit);
            }
        }
    }
}
