using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    public class PlayerItemSelectState : IState
    {
        PlayerTurn turn;
        StateMachine bsm;
        StateMachine sm;

        BattleManager battle;
        UIManager ui;

        Item[] items;
        int inventorySize;

        bool moving;
        float delay = 0.1f;

        public PlayerItemSelectState(PlayerTurn turn)
        {
            battle = turn._battle;
            ui = turn._ui;

            this.turn = turn;
            bsm = battle._sm;
            sm = turn._sm;

            delay = battle._delay;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            ui._itemPanel.gameObject.SetActive(true);
            ui._backPanel.SetActive(true);
            ui._scrollPanel.SetActive(true);
            ui._confirmPanel.SetActive(true);

            SetItems();

            moving = false;
        }

        public void ExecutePerFrame()
        {
            if (items.Length > 0)
            {
                if (!moving) ui._itemPanel.StartCoroutine(MoveSelectionCo());
                ConfirmItem();
            }

            Back();
        }

        public void Exit()
        {
            ui._backPanel.SetActive(false);
            ui._itemPanel.gameObject.SetActive(false);
            ui._scrollPanel.SetActive(false);
            ui._confirmPanel.SetActive(false);
        }
        #endregion

        void SetItems()
        {
            items = GameManager.instance._inventory._inventory;
            inventorySize = items.Length;

            ui._itemPanel.EnterPanel(items);
        }

        void NextItem()
        {
            if (battle._next)
            {
                ui._itemPanel.NextSelection(inventorySize);
            }
        }

        void PreviousItem()
        {
            if (battle._previous)
            {
                ui._itemPanel.PreviousSelection(inventorySize);
            }
        }

        void ConfirmItem()
        {
            if (battle._confirm)
            {
                Item selectedItem = ui._itemPanel._currentItem;
                turn._command = selectedItem.UseItem(turn._combatant);

                if (selectedItem is DamageItem)
                {
                    turn._targets = battle._activeEnemies;

                    sm.ChangeState(StateID.PlayerTurnTarget);
                }
                else if (selectedItem is RecoveryItem)
                {
                    turn._targets = battle._activeParty;

                    sm.ChangeState(StateID.PlayerTurnTarget);
                }
            }
        }

        void Back()
        {
            if (battle._backOut)
            {
                sm.ChangeState(StateID.PlayerTurnCommand);
            }
        }

        IEnumerator MoveSelectionCo()
        {
            if (battle._next)
            {
                moving = true;

                ui._itemPanel.NextSelection(inventorySize);

                yield return new WaitForSeconds(delay);

                moving = false;
            }
            else if (battle._previous)
            {
                moving = true;

                ui._itemPanel.PreviousSelection(inventorySize);

                yield return new WaitForSeconds(delay);

                moving = false;
            }
        }
    }
}