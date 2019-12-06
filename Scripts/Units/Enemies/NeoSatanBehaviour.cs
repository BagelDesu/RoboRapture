//-----------------------------------------------------------------------
// <copyright file="NeoSatanBehaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.TileAuxillary;
    using Edu.Vfs.RoboRapture.Units.Actions;
    using Edu.Vfs.RoboRapture.Units.Actions.Enemy;
    using UnityEngine;

    public class NeoSatanBehaviour : Behaviour
    {
        public static System.Action RoarAttack;

        public static System.Action DeathMarch;

        public static System.Action SatanDamaged;

        public static System.Action PukeAttack;

        [SerializeField]
        private GameObject skullDecal;

        private EnemyUnit unit;

        private int headMovementIndex = 0;

        private int pukeActionIndex = 1;

        private int screamActionIndex = 2;

        private int deathMarchActionIndex = 3;

        private AnimationStateUpdater headAnimationUpdater;

        private NeoSatanLegDeathMarchAction deathMarch;

        private List<Point> legsPositions = new List<Point>();

        private List<Point> validPositions = null;

        private DecalBuilder decalBuilder;

        public List<Point> LegsPositions { get => legsPositions; private set => legsPositions = value; }

        public void OnEnable()
        {
            this.unit = this.GetComponent<EnemyUnit>();
            decalBuilder = new DecalBuilder(skullDecal);
            deathMarch = this.GetComponentInChildren<NeoSatanLegDeathMarchAction>();
            headAnimationUpdater = this.gameObject.GetComponentInChildren<AnimationStateUpdater>();
        }

        public override IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            this.unit.Board = board;

            //// HEAD MOVING
            validPositions = GetValidPositions(boardController, headMovementIndex);
            boardController?.SwitchTilesFromActiveBoards(new HashSet<Point>(validPositions), TileStates.HIGHLIGHT);
            yield return new WaitForSeconds(0.5f);
            boardController?.ClearAllActiveBoardsDecorations();
            Move(headMovementIndex);
            //yield return new WaitForSeconds(0.75f);

            if (!deathMarch.AreLegsOnTheBoard)
            {
                //// HEAD PUKING
                validPositions = GetValidPositions(boardController, pukeActionIndex);
                if (validPositions.Count > 0)
                {
                    PukeAttack?.Invoke();
                    Attack(pukeActionIndex);
                }

                yield return new WaitForSeconds(1f);

                //// LEGS HIGHLIGHTING TILES
                deathMarch.RiseToTheSky();
                legsPositions = GetValidPositions(boardController, deathMarchActionIndex);
                decalBuilder.Instanciate(legsPositions);
                legsPositions?.ForEach(p => SwitchDescription(boardController, p, DescriptionTypes.LEG));
            }
            else
            {
                //// HEAD SCREAMING
                RoarAttack?.Invoke();
                validPositions = GetValidPositions(boardController, screamActionIndex);
                Attack(screamActionIndex);

                yield return new WaitForSeconds(4.5f);

                //// LEGS ATTACKING
                DeathMarch?.Invoke();
                yield return deathMarch.FallFromTheSky();
                legsPositions?.ForEach(p => SwitchDescription(boardController, p, DescriptionTypes.NORMAL));
                decalBuilder.DestroyInstances();
            }

            deathMarch.AreLegsOnTheBoard = !deathMarch.AreLegsOnTheBoard;
            yield return null;
        }

        public void OnDamageReceived()
        {
            if (unit != null && !unit.Health.HasIncreasedHealth)
            {
                SatanDamaged?.Invoke();
            }
        }

        private void Attack(int indexAction)
        {
            if (indexAction != deathMarchActionIndex)
            {
                this.unit.ActionsHandler.Execute(default);
                headAnimationUpdater.PlayAttackAnimation(indexAction);
                return;
            }

            ((SkillAction)this.unit.ActionsHandler.GetActions()[indexAction]).Execute();
        }

        private List<Point> GetValidPositions(BoardController boardController, int indexAction)
        {
            this.unit.ActionsHandler.ActivateAction(indexAction);
            this.unit.ActionsHandler.GetActions()[indexAction].BoardController = boardController;
            return this.unit.ActionsHandler.GetActions()[indexAction].GetValidTargets(this.unit.Board, this.unit.GetPosition());
        }

        private void Move(int indexAction)
        {
            this.unit.ActionsHandler.GetActions()[indexAction].Execute(default);
        }

        private void SwitchDescription(BoardController boardController, Point position, TileAuxillary.DescriptionTypes description)
        {
            if (boardController == null || position == default)
            {
                return;
            }

            foreach (var tile in boardController.GetAllTilesFromVisibleBoard())
            {
                if (tile.GetPosition() == position)
                {
                    tile.SwitchDescription(description);
                }
            }
        }
    }
}