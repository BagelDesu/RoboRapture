//-----------------------------------------------------------------------
// <copyright file="StateRenderer.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.Helpers;
    using UnityEngine;

    public class StateRenderer : MonoBehaviour
    {
        [SerializeField]
        private Material selected;

        [SerializeField]
        private Material normal;

        [SerializeField]
        private Material disabled;

        [SerializeField]
        private Material target;

        private Renderer render;

        public Material Selected { get => selected; set => selected = value; }

        public Material Normal { get => normal; set => normal = value; }

        public Material Disabled { get => disabled; set => disabled = value; }

        public Material Target { get => target; set => target = value; }

        public void Awake()
        {
            this.render = this.GetComponent<Renderer>();

            if (this.render == null)
            {
                this.render = this.GetComponentInChildren<Renderer>();
            }

            this.UpdateMaterial(this.normal);
        }

        public void OnEnable()
        {
            Selector.CancelledAction += OnActionCancelled;
        }

        public void OnDisable()
        {
            Selector.CancelledAction -= OnActionCancelled;
        }

        public void ChangeNormalAndDisabledMaterials(Material mat)
        {
           normal = mat;
           disabled = mat; 
        }

        public void ChangeState(StateType state)
        {
            switch (state)
            {
                case StateType.Normal:
                    this.UpdateMaterial(this.normal);
                    break;
                case StateType.Selected:
                    this.UpdateMaterial(this.selected);
                    break;
                case StateType.Disabled:
                    this.UpdateMaterial(this.disabled);
                    break;
                case StateType.Targeted:
                    this.UpdateMaterial(this.target);
                    break;
                default:
                    this.UpdateMaterial(this.normal);
                    break;
            }
        }

        public void UpdateMaterial(Health health)
        {
            StateType state = health.IsDead() ? StateType.Disabled : StateType.Normal;
            this.ChangeState(state);
        }

        private void UpdateMaterial(Material material)
        {
            if (this.render == null)
            {
                return;
            }

            this.render.material = material;
        }

        private void OnActionCancelled()
        {
            ChangeState(StateType.Normal);
        }
    }
}
