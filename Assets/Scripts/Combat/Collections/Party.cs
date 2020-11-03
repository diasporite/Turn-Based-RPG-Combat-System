using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    public class Party : MonoBehaviour
    {
        [SerializeField] CharID[] partyIDs = new CharID[8];

        int currentPartyIndex = 0;
        [SerializeField] List<BattleChar> party = new List<BattleChar>();

        CharacterDatabase database;

        public BattleChar _currentPartyMember => party[currentPartyIndex];

        public BattleChar[] _party => party.ToArray();
        public List<BattleChar> _partyList => party;

        public int _currentPartyIndex { set { currentPartyIndex = value; } }

        public void BuildParty(int lv)
        {
            database = GameManager.instance._charDatabase;

            foreach (var id in partyIDs)
            {
                if (id != CharID.None)
                {
                    party.Add(database.GetChar(id, lv));
                }
            }
        }

        public void PartyEnterBattle(int sideCap)
        {
            for(int i = 0; i < party.Count; i++)
            {
                if (i < sideCap) party[i]._battleStatus = BattleStatus.InBattle;
                else party[i]._battleStatus = BattleStatus.Standby;
            }
        }

        public void PartyGainExperience(int amount)
        {
            foreach(var member in party)
            {
                if(!member._dead) member.GainExperience(amount);
            }
        }

        public void LeaveBattle()
        {
            foreach(var member in party)
            {
                if (member._battleStatus != BattleStatus.Dead)
                    member._battleStatus = BattleStatus.NotInBattle;
            }
        }
    }
}