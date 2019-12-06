//-----------------------------------------------------------------------
// <copyright file="TurnsUIUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using static TurnSystem.TurnEntities;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Edu.Vfs.RoboRapture.Helpers;
    using System.Collections;

    public class TurnsUIUpdater : MonoBehaviour
    {
        [SerializeField]
        private Panel turnPanel;

        [SerializeField]
        private Panel blockerPanel;

        [SerializeField]
        private TextMeshProUGUI turn;

        [SerializeField]
        private TextMeshProUGUI turnTextToRemain;

        [SerializeField]
        private float displayingTurnForSeconds = 0.5f;

        [SerializeField]
        private Button playerTurnButton;

        [SerializeField]
        private FXWrapper pressedButtonFx;

        [SerializeField]
        private RefTurnEntity currentTurnEntity;

        [SerializeField]
        private Animator animator;

        private void OnEnable()
        {
            this.turnPanel.gameObject.SetActive(false);
            this.blockerPanel.gameObject.SetActive(false);
            PlayerController.AvailableActions += CheckForAvailableActions;
        }

        private void OnDisable()
        {
            PlayerController.AvailableActions -= CheckForAvailableActions;
        }

        public void CheckForAvailableActions(bool availableActions)
        {
            if (!availableActions)
            {
                playerTurnButton.Select();
            }
        }

        public void UpdateTurnInfo()
        {
            string text = this.currentTurnEntity.Value + "'S TURN";
            this.turn.text = text;
            this.turnTextToRemain.text = text;
            this.playerTurnButton.gameObject.SetActive(false);
            StartCoroutine(Transitions());
        }

        private IEnumerator Transitions()
        {
            if (this.currentTurnEntity.Value == PLAYER || this.currentTurnEntity.Value == ENEMY)
            {
                this.turnPanel.gameObject.SetActive(true);
                this.blockerPanel.gameObject.SetActive(true);
                this.turnTextToRemain.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);
                animator?.SetTrigger("Exit");
                yield return new WaitForSeconds(1);
                this.turnPanel.gameObject.SetActive(false);
                this.blockerPanel.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.75f);
                this.playerTurnButton.gameObject.SetActive(this.currentTurnEntity.Value == PLAYER);
            }

            yield return new WaitForSeconds(1);
            this.turnPanel.gameObject.SetActive(false);
            this.blockerPanel.gameObject.SetActive(false);
            this.turnTextToRemain.gameObject.SetActive(false);
            yield return null;
        }

        public void OnEndTurnButtonPressed()
        {
            pressedButtonFx?.Play(this.transform.position);
        }
    }
}
