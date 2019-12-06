//-----------------------------------------------------------------------
// <copyright file="PlacementUI.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Managers;
    using Edu.Vfs.RoboRapture.Scriptables;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlacementUI : MonoBehaviour
    {
        public static Action<int> PlacementSelectedUnit;

        [SerializeField]
        private UnitsMap map;

        [SerializeField]
        private MessageUI phaseLabel;

        [SerializeField]
        private TextMeshProUGUI placeYourUnitsText;

        [SerializeField]
        private LayoutGroup container;

        [SerializeField]
        private PlacementUnitUI[] unitsUIButtons;

        [SerializeField]
        private PhantasmUI[] phantasmsUI;

        [SerializeField]
        private FXWrapper buttonClicked;

        [SerializeField]
        private float delay = 5;

        private List<int> unitsPositioned;

        public void OnPlacementCompleted()
        {
            this.phaseLabel.SetText(MessageUI.PhaseText.Gameplay);
            placeYourUnitsText.gameObject.SetActive(false);
            this.container.gameObject.SetActive(false);
            this.Invoke("DisablePlacement", this.delay);
        }

        public void SelectButton(int index)
        {
            if (unitsPositioned != null && unitsPositioned.Contains(index))
            {
                return;
            }

            PlacementSelectedUnit?.Invoke(index);
            this.UpdateUI(unitsPositioned);
            this.unitsUIButtons[index].SetActive();
            UpdatePhantasms(index);
            buttonClicked?.Play(this.transform.position);
        }

        private void DisablePlacement()
        {
            this.phaseLabel.Activate(false);
            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            PlacementManager.UnitsPlaced += this.UpdateUI;
            PlacementManager.PlacementCompleted += this.OnPlacementCompleted;
            this.unitsUIButtons?.ToList().ForEach(item => item.SetNormal());
            this.phaseLabel.SetText(MessageUI.PhaseText.Placement);
            this.container.gameObject.SetActive(false);
            this.placeYourUnitsText.gameObject.SetActive(false);
            this.Invoke("ActivateUnits", this.delay);
        }

        private void OnDisable()
        {
            PlacementManager.UnitsPlaced -= this.UpdateUI;
            PlacementManager.PlacementCompleted -= this.OnPlacementCompleted;
        }

        private void ActivateUnits()
        {
            this.container.gameObject.SetActive(true);
            this.placeYourUnitsText.gameObject.SetActive(true);
            this.phaseLabel.Activate(false);
        }
        
        private void UpdateUI(List<int> unitsPositioned)
        {
            this.unitsPositioned = unitsPositioned;
            if (this.unitsUIButtons == null || this.unitsUIButtons.Length == 0 || this.unitsPositioned == null)
            {
                return;
            }

            for (int i = 0; i < this.unitsUIButtons.Length; i++)
            {
                if (this.unitsPositioned.Contains(i))
                {
                    this.unitsUIButtons[i].SetPlaced();
                    UpdatePhantasms(-1);
                }
                else
                {
                    this.unitsUIButtons[i].SetNormal();
                }
            }

        }

        private void UpdatePhantasms(int index)
        {
            phantasmsUI.ToList().ForEach(p => p.gameObject.SetActive(false));

            if (index >= 0 && index < phantasmsUI.Length)
            {
                phantasmsUI[index].gameObject.SetActive(true);
            }
        }
    }
}