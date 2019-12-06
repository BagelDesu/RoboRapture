//-----------------------------------------------------------------------
// <copyright file="Unit.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.Base;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.Units.Actions;
    using UnityEngine;

    /// <summary>
    /// Set the general properties and behaviors for the characters in the game.
    /// </summary>
    public abstract class Unit : MonoBehaviour, ISelectable
    {
        [SerializeField]
        private Sprite picture;

        [SerializeField]
        private Health health;

        [SerializeField]
        private UnitsMap unitsMap;

        [SerializeField]
        private Point position;

        [SerializeField]
        private string unitName;

        [SerializeField]
        private Type unitType;

        [SerializeField]
        private float height = 0.6f;

        private int turn;

        [SerializeField]
        private bool isAffectedByKnockback = true;

        [SerializeField]
        private ActionsHandler actionsHandler;

        private SpriteRenderer spriteRenderer;

        private StateRenderer stateRenderer;

        [SerializeField]
        private StructureType structureType;

        [SerializeField]
        private FXWrapper voFx;

        [SerializeField]
        private FXWrapper idleFx;

        [SerializeField]
        private FXWrapper stepsFx;

        public ActionsHandler ActionsHandler { get => this.actionsHandler; private set => this.actionsHandler = value; }

        public string UnitName { get => this.unitName; private set => this.unitName = value; }

        public UnitsMap UnitsMap { get => this.unitsMap; set => this.unitsMap = value; }

        public float Height { get => this.height; private set => this.height = value; }

        public Health Health { get => this.health; set => this.health = value; }

        public Sprite Picture { get => this.picture; set => this.picture = value; }

        public int Turn { get => turn; set => turn = value; }

        public bool IsAffectedByKnockback { get => isAffectedByKnockback; set => isAffectedByKnockback = value; }

        public StructureType StructureType { get => structureType; private set => structureType = value; }

        public FXWrapper VoFx { get => voFx; private set => voFx = value; }

        public FXWrapper StepsFx { get => stepsFx; private set => stepsFx = value; }

        public FXWrapper IdleFx { get => idleFx; set => idleFx = value; }
        
        public void Awake()
        {
            this.stateRenderer = this.gameObject.GetComponent<StateRenderer>();
            spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        }

        public virtual void SetPosition(Point position)
        {
            this.position = position;
        }

        public virtual Point GetPosition()
        {
            return this.position;
        }

        public Type GetUnitType()
        {
            return this.unitType;
        }

        public void SetUnitType(Type type)
        {
            this.unitType = type;
        }

        public void ChangeState(StateType state)
        {
            this.stateRenderer?.ChangeState(state);
        }

        public UnitType GetSpawnedUnitType()
        {
            if (this is PlayerUnit)
            {
                return ((PlayerUnit)this).PlayerUnitType;
            }
            else if (this is EnemyUnit)
            {
                return ((EnemyUnit)this).SpawnedEnemyType;
            }
            else
            {
                return default;
            }
        }

        public void FlipUnit(CardinalDirections cardinalDirection)
        {
            if (cardinalDirection == CardinalDirections.East 
                || cardinalDirection == CardinalDirections.SouthEast
                || cardinalDirection == CardinalDirections.South
                || cardinalDirection == CardinalDirections.SouthWest)
            {
                spriteRenderer.flipX = (unitType == Type.Player) ? false : true;
            }
            else if (cardinalDirection == CardinalDirections.NorthEast
                || cardinalDirection == CardinalDirections.North
                || cardinalDirection == CardinalDirections.NorthWest
                || cardinalDirection == CardinalDirections.West)
            {
                spriteRenderer.flipX = (unitType == Type.Player) ? true : false;
            }
        }

        public AttackSimulator GetAttackSimulator()
        {
            return this.gameObject.GetComponent<AttackSimulator>();
        }
    }
}
