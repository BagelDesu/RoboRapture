//-----------------------------------------------------------------------
// <copyright file="PlacementEffects.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.Units;
    using Edu.Vfs.RoboRapture.Validators;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlacementEffects
    {
        private float timePerTile = 0.14f;

        private float jumpingTime = 0.5f;

        private float jumpingHeight = 3.5f;

        public IEnumerator LerpMovementPath(MonoBehaviour caller, Unit unit, List<Point> path)
        {
            Logcat.I($"Lerp Movement Path moving unit {unit.GetPosition()}");
            path.ForEach(p => Logcat.I($"Lerp Path {p}"));

            for (int i = 1; i < path.Count; i++)
            {
                yield return LerpMovement(unit, unit.gameObject.transform.position, PointConverter.ToVector(path[i]));
            }
            yield return null;
        }

        public IEnumerator JumpCoroutine(Unit unit, Point pointDestination)
        {
            Logcat.I($"Jump Coroutine, initial position {unit.GetPosition()}, final position {pointDestination}");
            Vector3 destination = PointConverter.ToVector(pointDestination);
            destination.y = unit.Height;

            float JumpProgress = 0;
            var startPos = unit.transform.position;
            while (JumpProgress <= 1.0)
            {
                JumpProgress += Time.deltaTime / jumpingTime;
                var height = Mathf.Sin(Mathf.PI * JumpProgress) * jumpingHeight;
                if (height < 0f)
                {
                    height = 0f;
                }
                unit.transform.position = Vector3.Lerp(startPos, destination, JumpProgress) + Vector3.up * height;
                yield return null;
            }

            unit.transform.position = destination;
            PlacementHelper.Move(unit, pointDestination, new MovementActionValidator());
        }

        private IEnumerator LerpMovement(Unit unit, Vector3 initialPosition, Vector3 finalPosition)
        {
            Logcat.I($"Lerp Movement initial position {initialPosition} final position {finalPosition}");
            this.FlipUnit(unit, initialPosition, finalPosition);
            float elapsedTime = 0;
            unit.StepsFx?.Play(unit.transform.position);
            unit.transform.position = initialPosition;

            while (elapsedTime < timePerTile)
            {
                float xPosition = Mathf.Lerp(unit.transform.position.x, finalPosition.x, elapsedTime / timePerTile);
                float zPosition = Mathf.Lerp(unit.transform.position.z, finalPosition.z, elapsedTime / timePerTile);
                unit.transform.position = new Vector3(xPosition, unit.gameObject.transform.position.y, zPosition);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            PlacementHelper.Move(unit, PointConverter.ToPoint(finalPosition), new MovementActionValidator());
            yield return null;
        }

        private void FlipUnit(Unit unit, Vector3 initialPosition, Vector3 finalPosition)
        {
            Point initial = PointConverter.ToPoint(initialPosition);
            Point final = PointConverter.ToPoint(finalPosition);
            initial.y = 0;
            final.y = 0;
            CardinalDirections direction = Direction.GetCardinalDirection(initial, final);

            //// Logcat.I($"Lerp - Flipping Unit. Unit type {unit.GetUnitType()} to {direction}. Initial position {initial}, final position {final}");
            unit.FlipUnit(direction);
        }

    }
}