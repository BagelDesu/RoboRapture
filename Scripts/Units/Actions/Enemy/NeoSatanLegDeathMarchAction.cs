//-----------------------------------------------------------------------
// <copyright file="NeoSatanLegDeathMarchAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Units.Enemies;
    using UnityEngine;
    using static Edu.Vfs.RoboRapture.Patterns.CardinalDirections;

    public class NeoSatanLegDeathMarchAction : SkillAction
    {
        public static System.Action LegStomping;

        private List<EnemyUnit> legs;

        private bool areLegsOnTheBoard = false;

        [SerializeField]
        private int highlightedTilesNumber = 4;

        [SerializeField]
        private RefInt raptureRow;

        private int offset = 0;

        private System.Random random = new System.Random();

        private List<Point> targets;

        private float movingTime = 0.75F;

        private float height = 20;

        public bool AreLegsOnTheBoard { get => areLegsOnTheBoard; set => areLegsOnTheBoard = value; }

        public new void OnEnable()
        {
            legs = new List<EnemyUnit>(GetComponentsInChildren<EnemyUnit>(true));
            legs.ForEach(leg => leg.gameObject.transform.parent = null);
            legs.ForEach(leg => leg.gameObject.SetActive(true));
            InitializeLegs();
            legs.ForEach(leg => leg?.GetComponent<OnHealthValueChanged>()?.Attach(ReduceNeoSatanHealth));
        }

        public override List<Point> GetValidTargets(List<Point> board, Point position)
        {
            this.ValidPositions = base.GetValidTargets(board, position);
            this.ValidPositions.Remove(this.Unit.GetPosition());
            this.ValidPositions = this.ValidPositions?.Where(p => p.x > this.raptureRow.Value + this.offset).ToList();
            targets = GetTargetTiles();
            return AreLegsOnTheBoard ? new List<Point>() : targets;
        }

        private void InitializeLegs()
        {
            StartCoroutine(PlacementHelper.FallingMovement(legs[0], PointConverter.ToVector(legs[0].GetPosition()), PointConverter.ToVector(legs[0].GetPosition() + Direction.GetDirection(NorthEast)) + new Vector3(0, 15, 0), 0.02f));
            StartCoroutine(PlacementHelper.FallingMovement(legs[1], PointConverter.ToVector(legs[1].GetPosition()), PointConverter.ToVector(legs[1].GetPosition() + Direction.GetDirection(NorthWest)) + new Vector3(0, 15, 0), 0.02f));
            StartCoroutine(PlacementHelper.FallingMovement(legs[2], PointConverter.ToVector(legs[2].GetPosition()), PointConverter.ToVector(legs[2].GetPosition() + Direction.GetDirection(SouthEast)) + new Vector3(0, 15, 0), 0.02f));
            StartCoroutine(PlacementHelper.FallingMovement(legs[3], PointConverter.ToVector(legs[3].GetPosition()), PointConverter.ToVector(legs[3].GetPosition() + Direction.GetDirection(SouthWest)) + new Vector3(0, 15, 0), 0.02f));
        }

        private List<Point> GetTargetTiles()
        {
            List<Point> tiles = new List<Point>();
            if (this.ValidPositions == null)
            {
                return tiles;
            }

            for (int i = 0; i < highlightedTilesNumber; i++)
            {
                int index = random.Next(this.ValidPositions.Count);
                Point point = this.ValidPositions[index];
                this.ValidPositions.Remove(point);
                tiles.Add(point);
            }

            return tiles;
        }

        public new void Execute()
        {
        }

        public void RiseToTheSky()
        {
            foreach (var leg in legs)
            {
                leg.gameObject.GetComponentInChildren<NeoSatanLegAnimationStateUpdater>().Rise();
                Vector3 finalPosition = leg.gameObject.transform.position;
                finalPosition.y += this.height;
                StartCoroutine(PlacementHelper.FallingMovement(leg, leg.transform.position, finalPosition, movingTime));
            }
        }

        public IEnumerator FallFromTheSky()
        {
            if (targets == null)
            {
                yield return null;
            }

            for (int i = 0; i < legs.Count; i++)
            {
                legs[i].gameObject.GetComponentInChildren<NeoSatanLegAnimationStateUpdater>().Fall();
                Vector3 initialPosition = PointConverter.ToVector(targets[i]);
                initialPosition.y += this.height;
                LegStomping?.Invoke();
                yield return Attack(legs[i], targets[i], initialPosition);
            }
            yield return null;
        }

        private IEnumerator Attack(EnemyUnit leg, Point playerTarget, Vector3 initialPosition)
        {
            Logcat.I(this, $"NeoSatanLegDeathMarchAction Destroying target {UnitsMap.Get(playerTarget)?.UnitName} at position {playerTarget}");
            Unit target = UnitsMap.Get(playerTarget);
            SkillActionFX?.Play(PointConverter.ToVector(playerTarget));
            this.DestroyTarget(target);
            yield return StartCoroutine(PlacementHelper.FallingMovement(leg, initialPosition, PointConverter.ToVector(playerTarget), movingTime));
            yield return null;
        }

        private void DestroyTarget(Unit target)
        {
            if (target == null)
            {
                return;
            }

            target.Health.DeadByOneHit();
            target?.gameObject.GetComponentInChildren<AnimationStateUpdater>()?.DeathEnded();
        }

        private  void KnockbackToDirection(KnockbackHandler handler, Point legPosition, Patterns.CardinalDirections direction)
        {
            Point tilePosition = legPosition + Direction.GetDirection(direction);
            if (!Board.Contains(tilePosition))
            {
                return;
            }

            handler.Execute(this.BoardController, legPosition, tilePosition, this.Knockback);
        }

        private void ReduceNeoSatanHealth()
        {
            this.Unit.Health.ReduceHealth(1);
        }

        public void DestroyLegs(Health health)
        {
            if (!health.IsDead())
            {
                return;
            }

            this.legs?.ForEach(leg => leg?.GetComponent<OnHealthValueChanged>()?.Dettach(this.ReduceNeoSatanHealth));
            this.legs?.ForEach(leg => leg?.GetComponent<NeoSatanLegBehaviour>()?.DestroyLeg());
        }
    }
}