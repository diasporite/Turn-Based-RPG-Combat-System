using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class PlayerSkillSelectState : IState
    {
        BattleManager battle;
        PlayerTurn turn;
        StateMachine sm;

        UIManager ui;

        BattleChar combatant;

        Text[] skillTexts;
        KeyCode[] skillSelectKeys;
        Skill[] skills;

        public PlayerSkillSelectState(PlayerTurn turn)
        {
            battle = turn._battle;
            ui = turn._ui;

            this.turn = turn;
            sm = turn._sm;

            combatant = turn._combatant;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            SetUI(true);

            skills = combatant._currentSkillset;

            SetSkills();
        }

        public void ExecutePerFrame()
        {
            NextSkill();
            PreviousSkill();
            ConfirmSkill();
            Back();
        }

        public void Exit()
        {
            SetUI(false);
        }
        #endregion

        void SetUI(bool value)
        {
            ui._skillPanel.gameObject.SetActive(value);
            ui._backPanel.SetActive(value);
            ui._confirmPanel.SetActive(value);
        }

        void SetSkills()
        {
            ui._skillPanel.EnterPanel(combatant._currentSkillset);
        }

        void NextSkill()
        {
            if (battle._next)
            {
                ui._skillPanel.NextSelection(skills.Length);
            }
        }

        void PreviousSkill()
        {
            if (battle._previous)
            {
                ui._skillPanel.PreviousSelection(skills.Length);
            }
        }

        void ConfirmSkill()
        {
            if (battle._confirm)
            {
                Skill skill = ui._skillPanel._currentSkill;

                if (combatant.CanUseSkill(skill))
                {
                    turn._command = skill.GetCommand(combatant);

                    if (skill is AttackSkill)
                    {
                        AttackSkill(skill);
                    }
                    else if (skill is BuffSkill)
                    {
                        BuffSkill(skill);
                    }
                    else if (skill is HealSkill)
                    {
                        HealSkill(skill);
                    }
                }
            }
        }

        void AttackSkill(Skill skill)
        {
            AttackSkill aSkill = (AttackSkill)skill;

            if (aSkill._attackAll)
            {
                turn._command._targets = battle._activeEnemies;
                sm.ChangeState(StateID.PlayerTurnAction);
            }
            else
            {
                turn._targets = battle._activeEnemies;
                sm.ChangeState(StateID.PlayerTurnTarget);
            }
        }

        void BuffSkill(Skill skill)
        {
            BuffSkill bSkill = (BuffSkill)skill;

            if (bSkill._affectsWholeSide)
            {
                if (bSkill._affectsAllies)
                {
                    turn._command._targets = battle._activeParty;
                }
                else if (bSkill._affectsOpponents)
                {
                    turn._command._targets = battle._activeEnemies;
                }

                sm.ChangeState(StateID.PlayerTurnAction);
            }
            else
            {
                if (bSkill._affectsAllies)
                {
                    turn._targets = battle._activeParty;
                }
                else if (bSkill._affectsOpponents)
                {
                    turn._targets = battle._activeEnemies;
                }

                sm.ChangeState(StateID.PlayerTurnTarget);
            }
        }

        void HealSkill(Skill skill)
        {
            HealSkill hSkill = (HealSkill)skill;

            if (hSkill._healAll)
            {
                turn._command._targets = battle._activeParty;

                sm.ChangeState(StateID.PlayerTurnAction);
            }
            else
            {
                turn._targets = battle._activeParty;

                sm.ChangeState(StateID.PlayerTurnTarget);
            }
        }

        void Back()
        {
            if (battle._backOut)
            {
                turn._sm.ChangeState(StateID.PlayerTurnCommand);
            }
        }
    }
}