//-----------------------------------------------------------------------
// <copyright file="Selector.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using System;
    using Edu.Vfs.RoboRapture.Base;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class Selector : MonoBehaviour
    {
        private Camera mainCamera;

        [SerializeField]
        private RefPoint itemSelected;

        public static Action CancelledAction;

        private void Start()
        {
            this.mainCamera = Camera.main;
            this.itemSelected.Value = new Point(-1, -1, -1);
        }

        private void OnDisable()
        {
            this.itemSelected.Value = new Point(-1, -1, -1);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the mouse was clicked over a UI element
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Logcat.I(this, "Clicked on the UI");
                    return;
                }

                Ray ray = this.mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    MonoBehaviour[] list = hit.collider.gameObject.GetComponents<MonoBehaviour>();
                    foreach (MonoBehaviour item in list)
                    {
                        if (item is ISelectable)
                        {
                            this.itemSelected.Value = ((ISelectable)item).GetPosition();
                            Logcat.I(this, $"Position: {((ISelectable)item).GetPosition()}");
                            break;
                        }
                    }
                }
                else
                {
                    CancelledAction?.Invoke();
                    Logcat.I(this, $"Cancelled action event throwed");
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelledAction?.Invoke();
                Logcat.I(this, $"Cancelled action event throwed");
            }
        }
    }
}
