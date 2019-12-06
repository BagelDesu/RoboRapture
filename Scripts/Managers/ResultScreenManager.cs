

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.SpawnSystem;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;
using TMPro;
using Edu.Vfs.RoboRapture.AudioSystem;

namespace Edu.Vfs.RoboRapture.ResultsSystem
{ 
    [System.Serializable]
    public struct UnitTypeToText
    {
        public TextMeshProUGUI Text;
        public UnitType Type;
    };

    ///<summary>
    ///-Keeps track of the kills that the player recieves-
    ///</summary>
    public class ResultScreenManager : MonoBehaviour
    {
        [SerializeField]
        private UnitTypeToText[] Texts;

        public Dictionary<UnitType, int> UnitKills {get; private set;} = new Dictionary<UnitType, int>();
        public Dictionary<UnitType, TextMeshProUGUI> TextDictionary {get; private set;} = new Dictionary<UnitType, TextMeshProUGUI>();

        [SerializeField]
        private DebugOneShot ResultUiNormal;

        [SerializeField]
        private DebugOneShot ResultUiEnding;

        [SerializeField]
        private GameObject[] Titles;

        private void Awake()
        {
            foreach (UnitTypeToText item in Texts)
            {
                TextDictionary.Add(item.Type, item.Text);
                UnitKills.Add(item.Type, 0);
            }
        }

        private void OnEnable()
        {
            EnemyUnit.EnemyWithTypeDied += UpdateKills;
        }

        private void OnDisable()
        {
            EnemyUnit.EnemyWithTypeDied -= UpdateKills;
        }

        private void UpdateKills(Point point, UnitType type)
        {
            if(TextDictionary.ContainsKey(type))
            {
                UnitKills[type]++;
            }
        }

        public void UpdateTexts()
        {
            foreach (var item in Titles)
            {
                item.SetActive(true);
            }
            
            StartCoroutine(StartCountUp());
        }

        private IEnumerator StartCountUp()
        {
            yield return new WaitForSeconds(3f);

            foreach (var item in UnitKills)
            {
                if(item.Value > 0)
                {
                    TextDictionary[item.Key].transform.parent.gameObject.SetActive(true);
                    
                    int amount = 0;
                    while(amount < item.Value)
                    {
                        ResultUiNormal.PlayAudio();
                        amount++;
                        TextDictionary[item.Key].text = amount.ToString();
                        yield return new WaitForSeconds(0.2f);
                    }
                    ResultUiEnding.PlayAudio();
                }
            }
        }
    }
}
