//-----------------------------------------------------------------------
// <copyright file="UdeukedefrukeBehaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.AudioSystem;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.TileAuxillary;
    using UnityEngine;

    public class UdeukedefrukeBehaviour : Behaviour
    {
        private bool isWithHeadAndMask = true;

        private EnemyUnit unit;

        private Point headPoint;

        [SerializeField]
        private Sprite headPicture;

        [SerializeField]
        private Sprite bothPicture;

        [SerializeField]
        private string BothDescription = "The Skin and Bones";

        [SerializeField]
        private string HeadDescription = "The Bones";

        [SerializeField]
        private DebugOneShot intestinesAudio;

        private BoardController boardController;

        private UdeukedefrukeAnimationStateUpdater animationUpdater;

        public bool IsWithHeadAndMask { get => isWithHeadAndMask; set => isWithHeadAndMask = value; }

        public Point HeadPoint { get => headPoint; set => headPoint = value; }

        private void Awake()
        {
            unit = this.GetComponent<EnemyUnit>();
            animationUpdater = this.gameObject.GetComponentInChildren<UdeukedefrukeAnimationStateUpdater>();
        }

        public override IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            this.boardController = boardController;
            this.unit.Board = board;
            this.headPoint = this.unit.GetPosition();

            if (HasToRelocate() && !this.isWithHeadAndMask)
            {
                SuckIn();
            }

            List<Point> validPositions = GetMovementValidPositions();
            this.boardController?.SwitchTilesFromActiveBoards(new HashSet<Point>(validPositions), TileStates.HIGHLIGHT);
            yield return new WaitForSeconds(1f);

            if (isWithHeadAndMask)
            {
                Move();
                yield return new WaitForSeconds(1f);
                Extrude();
                yield return new WaitForSeconds(1f);
            }

            this.boardController?.ClearAllActiveBoardsDecorations();
            yield return new WaitForSeconds(1f);
        }

        public void SuckIn()
        {
            if (!isWithHeadAndMask)
            {
                ExecuteAction(false);
                this.animationUpdater.SuckIn();
                this.unit.IsAffectedByKnockback = true;
                intestinesAudio?.StopAudio();
            }
        }

        private List<Point> GetMovementValidPositions()
        {
            this.unit.ActionsHandler.ActivateAction(0);
            return this.unit.ActionsHandler.GetActions()[0].GetValidTargets(this.unit.Board, this.unit.GetPosition());
        }

        private void Move()
        {
            this.unit.ActionsHandler.GetActions()[0].Execute(default);
        }

        private void Extrude()
        {
            Logcat.I(this, $"Extruding skin position {this.headPoint} head position {this.unit.GetPosition()}");
            if (headPoint == this.unit.GetPosition())
            {
                return;
            }

            ExecuteAction(true);
            this.unit.IsAffectedByKnockback = false;
            intestinesAudio?.PlayAudio();
        }

        private void ExecuteAction(bool extrude)
        {
            this.isWithHeadAndMask = !extrude;
            this.UpdateUnitData();
            this.unit.ActionsHandler.ActivateAction(1);
            this.unit.ActionsHandler.GetActions()[1].BoardController = boardController;
            this.unit.ActionsHandler.GetValidTargets(this.unit.Board, this.unit.GetPosition());
             this.unit.ActionsHandler.Execute(extrude ? headPoint : default);
            this.unit.ActionsHandler.Execute();
            animationUpdater.Extrude();
        }

        private void UpdateUnitData()
        {
            if (isWithHeadAndMask)
            {
                this.unit.Description = this.BothDescription;
                this.unit.Picture = this.bothPicture;
            }
            else
            {
                this.unit.Description = this.HeadDescription;
                this.unit.Picture = headPicture;
            }
        }

        private bool HasToRelocate()
        {            
            List<Unit> playerUnits = this.unit.UnitsMap.GetUnits(Type.Player);
            List<Unit> playerUnitsBehind = playerUnits?.Where(u => u.GetPosition().x >= this.headPoint.x).ToList();
            return playerUnits == null || playerUnits.Count == 0? false : playerUnitsBehind.Count == playerUnits.Count;
        }
    }
}