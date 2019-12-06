//-----------------------------------------------------------------------
// <copyright file="Action.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions
{
    using System.Linq;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Patterns;
    using NaughtyAttributes;
    using UnityEngine;

    public abstract class Action : MonoBehaviour, IAction
    {
        [SerializeField]
        private string skillName;

        [SerializeField]
        private Sprite icon;

        [SerializeField]
        private string description;

        [SerializeField]
        private ActionType actionType;

        [SerializeField]
        private int range;

        [SerializeField]
        private PatternType patternType;

        [SerializeField]
        private int experiencePointsToUnlocked;

        [SerializeField]
        private int coolDown;

        private int turnsToReactivate;

        private bool availableToUnlock;

        private int currentExperiencePoints = 0;

        private BoardController boardController;

        private List<Point> board;

        private Unit unit;

        [ShowIf("CanShowCardinal"), SerializeField]
        private List<CardinalDirections> cardinalDirections;

        [ShowIf("CanShowCardinal"), SerializeField]
        private int distance;

        private bool isActive;

        private bool isEnabled = true;

        private IPattern pattern;

        private List<Point> validPositions;

        public int Range { get => this.range; private set => this.range = value; }

        public string SkillName { get => this.skillName; set => this.skillName = value; }

        public Sprite Icon { get => this.icon; set => this.icon = value; }

        public string Description { get => this.description; set => this.description = value; }

        public BoardController BoardController { get => boardController; set => boardController = value; }

        protected List<Point> Board { get => this.board; set => this.board = value; }

        protected List<Point> ValidPositions { get => this.validPositions; set => this.validPositions = value; }

        public Unit Unit { get => unit; private set => unit = value; }

        public int ExperiencePointsToUnlocked { get => experiencePointsToUnlocked; set => experiencePointsToUnlocked = value; }

        public int CurrentExperiencePoints { get => currentExperiencePoints; set => currentExperiencePoints = value; }

        public bool AvailableToUnlock { get => availableToUnlock; set => availableToUnlock = value; }

        public int CoolDown { get => this.coolDown; private set => this.coolDown = value; }

        public ActionType ActionType { get => actionType; private set => actionType = value; }

        public void Awake()
        {
            switch (this.patternType)
            {
                case PatternType.Cross:
                    this.pattern = new CrossPattern(this.range, this.cardinalDirections);
                    break;
                case PatternType.Diamond:
                    this.distance = 0;
                    this.pattern = new DiamondPattern(this.range);
                    break;
                case PatternType.Square:
                    this.distance = 0;
                    this.pattern = new SquarePattern(this.range);
                    break;
            }

            unit = GetComponentInParent<Unit>();
        }

        public virtual List<Point> GetValidTargets(List<Point> board, Point position)
        {
            this.isActive = true;
            this.board = board;
            this.validPositions = PatternProcessor.Process(board, this.pattern, position, this.distance);
            return this.validPositions;
        }

        public virtual bool ValidateAction(Point target)
        {
            if (target == null || validPositions == null)
            {
                return false;
            }

            bool validPosition = this.validPositions.Contains(target);
            this.isActive = validPosition;
            return validPosition;
        }

        public List<Point> GetActionRange(List<Point> board)
        {
            List<Point> boardPoints = PatternProcessor.Process(board, this.pattern, this.unit.GetPosition(), this.distance);
            List<Unit> environmentUnits = unit.UnitsMap.GetUnits(Type.Envrionment);
            List<Point> environmentPositions = new List<Point>();
            environmentUnits.ForEach(u => environmentPositions.Add(u.GetPosition()));
            return boardPoints.Where(p => !environmentPositions.Contains(p)).ToList(); ;
        }

        public abstract void Execute(Point target);

        public bool CanShowCardinal()
        {
            return PatternType.Cross == this.patternType;
        }

        public bool IsActive()
        {
            return this.isActive;
        }

        public void IsActive(bool isActive)
        {
            this.isActive = isActive;
        }

        public bool IsEnabled()
        {
            return this.isEnabled;
        }

        public void IsEnabled(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }

        protected void FaceTargetDirection(Point target)
        {
            CardinalDirections direction = Direction.GetCardinalDirection(this.unit.GetPosition(), target);
            this.unit.FlipUnit(direction);
        }

        public bool IsUnlocked()
        {
            return currentExperiencePoints == ExperiencePointsToUnlocked;
        }

        public void ResetCoolDown()
        {
            this.turnsToReactivate = this.coolDown;
        }

        public void RefreshReactivationTime()
        {
            this.turnsToReactivate--;

            if (this.turnsToReactivate < 0)
            {
                this.turnsToReactivate = 0;
            }
        }

        public int TurnsToReactivate()
        {
            return this.turnsToReactivate;
        }

        public bool IsReadyToUse()
        {
            return this.turnsToReactivate <= 0;
        }
    }
}
