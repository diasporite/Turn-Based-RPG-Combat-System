using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    public class GroupStats : MonoBehaviour
    {
        [SerializeField] BattlerStats[] stats;

        Dictionary<BattleChar, BattlerStats> statsDict = null;

        public BattlerStats[] _stats => stats;

        public Dictionary<BattleChar, BattlerStats> _statsDict => statsDict;

        public BattlerStats GetStatPanel(int index)
        {
            return stats[index];
        }

        public void InitPanels(BattleChar[] battlers)
        {
            if (statsDict == null) statsDict = new Dictionary<BattleChar, BattlerStats>();

            for (int i = 0; i < stats.Length; i++)
            {
                if (i < battlers.Length)
                {
                    if (!statsDict.ContainsKey(battlers[i]))
                    {
                        statsDict.Add(battlers[i], stats[i]);
                    }

                    stats[i].InitPanel(battlers[i]);
                }
            }
        }

        public void UpdateStats(BattleChar battler)
        {
            statsDict[battler].UpdateStats();
        }

        public void SwapPanelCharacter(int index, BattleChar newChar)
        {
            BattlerStats statPanel = GetStatPanel(index);
            BattleChar oldChar = statPanel._battler;

            statPanel.InitPanel(newChar);

            statsDict.Add(newChar, statPanel);
            statsDict.Remove(oldChar);
        }

        public void HighlightPanel(BattleChar battler, bool enteringTurn)
        {
            statsDict[battler].HighlightPanel(enteringTurn);
        }

        public void HighlightPanel(int index, bool enteringTurn)
        {
            stats[index].HighlightPanel(enteringTurn);
        }
    }
}