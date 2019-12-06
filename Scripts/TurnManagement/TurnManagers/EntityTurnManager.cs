

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using Edu.Vfs.RoboRapture.ScriptableLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Edu.Vfs.RoboRapture.TurnSystem
{    
    public enum TurnEntities
    {
        ENVIRONMENT,
        ENEMY,
        PLAYER
    }
    ///<summary>
    ///-summary of script here-
    ///</summary>
    public class EntityTurnManager : MonoBehaviour
    {
        [SerializeField] TurnEntities[] TurnSequence;
        [SerializeField] RefTurnEntity RefTurnEntity;

        private int CurrentTurn;
        private static Dictionary<TurnEntities, ITurnEntity> EntityCollection = new Dictionary<TurnEntities, ITurnEntity>();

        public TurnEntities ActiveTurnEntity;
        public event Action<TurnEntities> OnTurnEnd;
        public UnityEvent OnTurnManagerStart;

        public bool StartFirstTurnAuto;

        private void Start()
        {
            if(StartFirstTurnAuto)
            {
                StartTurnSystem();
            }
        }

        public static void RegisterEntity(ITurnEntity entity, TurnEntities entityType)
        {
            EntityCollection[entityType] = entity;
        }

        public void StartTurnSystem()
        {

            StartEntityTurn(TurnSequence[0]);
            OnTurnManagerStart?.Invoke();
        }

        private void StartEntityTurn(TurnEntities entity)
        {
            ActiveTurnEntity = entity;
            OnTurnEnd?.Invoke(ActiveTurnEntity);
            RefTurnEntity.Value = entity;
            EntityCollection[entity].StartTurn(this);
        }

        public void NextEntityTurn()
        {

            CurrentTurn++;
            if(CurrentTurn >= TurnSequence.Length || CurrentTurn < 0)
            {
                CurrentTurn = 0;
            }

            StartEntityTurn(TurnSequence[CurrentTurn]);
        }
    }
}
