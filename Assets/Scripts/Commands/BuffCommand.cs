using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class BuffCommand : Command
    {
        BuffSkill skill;

        UIManager ui;

        float delay = 0.75f;

        public BuffCommand(BattleChar instigator, BuffSkill skill) : base(instigator)
        {
            this.skill = skill;

            ui = game._ui;

            delay = game._battle._delay;
        }

        public override IEnumerator ExecuteCo()
        {
            yield return BuffSkillCo();
        }

        IEnumerator BuffSkillCo()
        {
            ui.LogMessage(instigator._charName + " used " + skill._skillName + "!");

            instigator._magic.ChangeCurrentPoints(-skill._mpCost);

            yield return new WaitForSeconds(delay);

            foreach(var target in targets)
            {
                foreach(var effect in skill._effects)
                {
                    if(target.IsValueStat(effect._affectedStat))
                        target.GetStat(effect._affectedStat).AddModifier(effect._modifier);
                }
            }

            LogMessage();

            yield return new WaitForSeconds(delay);
        }

        void LogMessage()
        {
            string message = "Stats changed!";

            ui.LogMessage(message);
        }
    }
}