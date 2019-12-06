//-----------------------------------------------------------------------
// <copyright file="EnemyUnit.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using UnityEngine;

    public class EnemyUnit : Unit
    {
        public static Action<Point> EnemyDied;

        public static Action<Point, SpawnSystem.UnitType> EnemyWithTypeDied;

        [SerializeField]
        private int speed;

        [SerializeField]
        private string description;

        [SerializeField]
        private EnemyType enemyType;

        [SerializeField]
        private EnemyCategory enemyCategory;

        [SerializeField]
        private Enemies.Behaviour behaviour;

        private List<Point> board;

        private Unit target;

        [SerializeField]
        private SpawnSystem.UnitType spawnedEnemyType;

        public int Speed { get => this.speed; private set => this.speed = value; }

        public string Description { get => this.description; set => this.description = value; }

        public EnemyType EnemyType { get => this.enemyType; private set => this.enemyType = value; }

        public List<Point> Board { get => this.board; set => this.board = value; }

        public EnemyCategory EnemyCategory { get => this.enemyCategory; set => this.enemyCategory = value; }

        public Unit Target { get => target; set => target = value; }

        public Enemies.Behaviour Behaviour { get => behaviour; set => behaviour = value; }

        public UnitType SpawnedEnemyType { get => spawnedEnemyType; private set => spawnedEnemyType = value; }

        public int Range()
        {
            return this.ActionsHandler == null ? 0 : this.ActionsHandler.GetRange();
        }

        public IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            if (behaviour == null)
            {
                yield return null;
            }

            yield return StartCoroutine(behaviour.Execute(boardController, board));
        }

        public void NotifyDyingEvent()
        {
            EnemyDied?.Invoke(this.GetPosition());
            EnemyWithTypeDied?.Invoke(this.GetPosition(), spawnedEnemyType);
        }
    }
}