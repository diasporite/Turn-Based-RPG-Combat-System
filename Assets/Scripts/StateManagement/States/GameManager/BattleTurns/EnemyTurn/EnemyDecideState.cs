using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG_Project
{
    public class EnemyDecideState : IState
    {
        BattleManager battle;
        EnemyTurn turn;
        StateMachine sm;

        BattleChar combatant;

        BattleChar[] activeParty;
        BattleChar[] activeEnemies;

        // Base weightings for different actions
        int attackWeight = 6;
        int skillWeight = 2;

        AIAction[] skillActions = new AIAction[] { AIAction.Skill1, AIAction.Skill2,
            AIAction.Skill3, AIAction.Skill4, AIAction.Skill5, AIAction.Skill6, AIAction.Skill7 };

        List<AIAction> possibleActions = new List<AIAction>();

        public EnemyDecideState(EnemyTurn turn)
        {
            this.turn = turn;
            sm = turn._sm;

            battle = GameManager.instance._battle;

            combatant = turn._combatant;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            activeParty = battle._activeParty;
            activeEnemies = battle._activeEnemies;

            possibleActions.Clear();

            Decide();
        }

        public void ExecutePerFrame()
        {
            battle.ToggleAutoMode();
        }

        public void Exit()
        {

        }
        #endregion

        void Decide()
        {
            PopulateActions();

            AIAction action = ChooseAction();

            if (action == AIAction.Attack)
            {
                Attack();
            }
            else if (skillActions.Contains(action))
            {
                int i = System.Array.IndexOf(skillActions, action);
                UseSkill(i);
            }

            sm.ChangeState(StateID.EnemyTurnAction);
        }

        AIAction ChooseAction()
        {
            return possibleActions[Random.Range(0, possibleActions.Count)];
        }

        void PopulateActions()
        {
            for(int i = 0; i < attackWeight; i++)
            {
                possibleActions.Add(AIAction.Attack);
            }

            for(int i = 0; i < combatant._currentSkillset.Length; i++)
            {
                if (combatant.CanUseSkill(combatant._currentSkillset[i]))
                {
                    for (int j = 0; j < skillWeight; j++)
                    {
                        possibleActions.Add(skillActions[i]);
                    }
                }
            }
        }

        void Attack()
        {
            turn._command = new AttackCommand(combatant, battle._regularAttack);

            BattleChar[] target = new BattleChar[] { activeParty[Random.Range(0, activeParty.Length)] };

            turn._command._targets = target;
        }

        void UseSkill(int index)
        {
            Skill skill = combatant._currentSkillset[index];

            turn._command = skill.GetCommand(combatant);

            if(skill is AttackSkill)
            {
                UseAttackSkill(skill);
            }
            else if(skill is BuffSkill)
            {
                UseBuffSkill(skill);
            }
            else if(skill is HealSkill)
            {
                UseHealSkill(skill);
            }
        }

        void UseAttackSkill(Skill skill)
        {
            AttackSkill aSkill = (AttackSkill)skill;

            if (aSkill._attackAll)
            {
                turn._command._targets = battle._activeParty;
            }
            else
            {
                turn._command._targets = new BattleChar[] { activeParty[Random.Range(0, activeParty.Length)] };
            }
        }

        void UseBuffSkill(Skill skill)
        {
            BuffSkill bSkill = (BuffSkill)skill;

            if (bSkill._affectsWholeSide)
            {
                if (bSkill._affectsAllies)
                {
                    turn._command._targets = battle._activeEnemies;
                }
                else if (bSkill._affectsOpponents)
                {
                    turn._command._targets = battle._activeParty;
                }
            }
            else
            {
                if (bSkill._affectsAllies)
                {
                    turn._command._targets = new BattleChar[] { combatant };
                }
                else if (bSkill._affectsOpponents)
                {
                    turn._command._targets = new BattleChar[] { activeParty[Random.Range(0, activeParty.Length)] };
                }
            }
        }

        void UseHealSkill(Skill skill)
        {
            HealSkill hSkill = (HealSkill)skill;

            if (hSkill._healAll)
            {
                turn._command._targets = battle._activeEnemies;
            }
            else
            {
                turn._command._targets = new BattleChar[] { combatant };
            }
        }
    }
}