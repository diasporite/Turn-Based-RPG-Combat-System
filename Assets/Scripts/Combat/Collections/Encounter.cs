using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    public class Encounter : MonoBehaviour
    {
        [SerializeField] CharID[] enemyIDs = new CharID[8];

        int currentEnemyIndex = 0;
        [SerializeField] List<BattleChar> enemies = new List<BattleChar>();

        CharacterDatabase charDatabase;

        public BattleChar _currentPartyMember => enemies[currentEnemyIndex];

        public BattleChar[] _enemies => enemies.ToArray();

        public int _currentEnemyIndex { set => currentEnemyIndex = value; }

        public void InitEncounter(int lv)
        {
            charDatabase = GameManager.instance._charDatabase;

            foreach (var id in enemyIDs)
            {
                if (id != CharID.None)
                {
                    enemies.Add(charDatabase.GetChar(id, lv));
                }
            }
        }
    }
}