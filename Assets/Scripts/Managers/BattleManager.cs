using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG_Project
{
    public class BattleManager : MonoBehaviour
    {
        [Header("General Battle Stats")]
        [SerializeField] bool autoMode = false;
        [SerializeField] int actions = 0;

        [SerializeField] AttackSkill regularAttack;

        [SerializeField] Encounter encounter;

        GameObject reticleObj;

        [SerializeField] int currentBattlerIndex = 0;
        [SerializeField] List<BattleChar> activeBattlers = new List<BattleChar>();

        [SerializeField] int currentExpEarned = 0;
        [SerializeField] int currentMoneyEarned = 0;

        [Header("General Battle Settings")]
        [SerializeField] BattlePortrait portraitPrefab;

        const int combatantSideCap = 3;

        [SerializeField] float delay = 0.75f;

        [Header("UI Settings")]
        [SerializeField] float fadeTime = 0.25f;

        [Header("Position Settings")]
        [SerializeField] Vector3[] playerPositions = new Vector3[combatantSideCap];
        [SerializeField] Vector3[] enemyPositions = new Vector3[combatantSideCap];

        [Header("Party Settings")]
        [SerializeField] BattleChar currentPlayer;
        [SerializeField] BattleChar[] partyMembers;
        [SerializeField] List<BattleChar> activeParty = new List<BattleChar>();
        List<BattlePortrait> activePartyPortraits = new List<BattlePortrait>();

        [Header("Enemy Settings")]
        [SerializeField] BattleChar[] enemies;
        [SerializeField] BattleChar currentEnemy;
        [SerializeField] List<BattleChar> activeEnemies = new List<BattleChar>();
        List<BattlePortrait> activeEnemyPortraits = new List<BattlePortrait>();

        GameManager game;
        UIManager ui;

        StateMachine sm = new StateMachine();

        #region Variables - Input
        [Header("Input - Commands")]
        [SerializeField] KeyCode attackKey = KeyCode.A;
        [SerializeField] KeyCode skillKey = KeyCode.D;
        [SerializeField] KeyCode switchKey = KeyCode.S;
        [SerializeField] KeyCode itemKey = KeyCode.W;
        [SerializeField] KeyCode analyseKey = KeyCode.J;
        [SerializeField] KeyCode runKey = KeyCode.L;

        [Header("Input - Skills")]
        [SerializeField] KeyCode[] skillSelectKeys;

        [Header("Input - Selection")]
        [SerializeField] KeyCode next = KeyCode.RightArrow;
        [SerializeField] KeyCode previous = KeyCode.LeftArrow;
        [SerializeField] KeyCode confirm = KeyCode.S;
        [SerializeField] KeyCode backOutKey = KeyCode.W;

        [Header("Input - Auto")]
        [SerializeField] KeyCode autoKey = KeyCode.Space;
        #endregion

        #region Getter/Setters
        public BattleChar _switchNewChar { get; set; }

        public bool _autoMode
        {
            get => autoMode;
            set => autoMode = value;
        }

        public AttackSkill _regularAttack => regularAttack;

        public GameObject _reticleObj => reticleObj;

        public int _currentExpEarned => currentExpEarned;
        public int _currentMoneyEarned => currentMoneyEarned;

        public BattlePortrait _portraitPrefab => portraitPrefab;

        public float _delay => delay;

        public float _fadeTime => fadeTime;

        public Vector3[] _playerPositions => playerPositions;
        public Vector3[] _enemyPositions => enemyPositions;

        public BattleChar _currentPlayer
        {
            get => currentPlayer;
            set => currentPlayer = value;
        }

        public Encounter _encounter
        {
            get => encounter;
            set => encounter = value;
        }

        public BattleChar[] _enemies
        {
            get => enemies;
            set => enemies = value;
        }

        public int _currentBattlerIndex
        {
            get => currentBattlerIndex;
            set => currentBattlerIndex = value;
        }

        public BattleChar _currentBattler => activeBattlers[currentBattlerIndex];

        public BattleChar[] _activeParty => activeParty.ToArray();
        public List<BattleChar> _activePartyList => activeParty;
        public BattlePortrait[] _activePartyPortraits => activePartyPortraits.ToArray();
        
        public BattleChar _currentEnemy => currentEnemy;
        public BattleChar[] _activeEnemies => activeEnemies.ToArray();
        public List<BattleChar> _activeEnemiesList => activeEnemies;
        public BattlePortrait[] _activeEnemyPortraits => activeEnemyPortraits.ToArray();

        public BattleChar[] _activeBattlers => activeBattlers.ToArray();
        public List<BattleChar> _activeBattlersList => activeBattlers;

        public StateMachine _sm => sm;

        public KeyCode _attackKey => attackKey;
        public KeyCode _skillKey => skillKey;
        public KeyCode _switchKey => switchKey;
        public KeyCode _itemKey => itemKey;
        public KeyCode _analyseKey => analyseKey;
        public KeyCode _runKey => runKey;

        public KeyCode[] _skillSelectKeys => skillSelectKeys;

        public KeyCode _nextKey => next;
        public KeyCode _previousKey => previous;
        public KeyCode _confirmKey => confirm;
        public KeyCode _backOutKey => backOutKey;

        public KeyCode _autoKey => autoKey;

        public bool _attack => Input.GetKeyDown(attackKey);
        public bool _skill => Input.GetKeyDown(skillKey);
        public bool _switch => Input.GetKeyDown(switchKey);
        public bool _item => Input.GetKeyDown(itemKey);
        public bool _analyse => Input.GetKeyDown(analyseKey);
        public bool _run => Input.GetKeyDown(runKey);

        public bool _next => Input.GetKeyDown(next);
        public bool _previous => Input.GetKeyDown(previous);
        public bool _confirm => Input.GetKeyDown(confirm);
        public bool _backOut => Input.GetKeyDown(backOutKey);

        public Vector3[] _partyPortraitPositions
        {
            get
            {
                Vector3[] pos = new Vector3[activePartyPortraits.Count];

                for (int i = 0; i < activePartyPortraits.Count; i++)
                {
                    pos[i] = activePartyPortraits[i].transform.position;
                }

                return pos;
            }
        }

        public Vector3[] _enemyPortraitPositions
        {
            get
            {
                Vector3[] pos = new Vector3[activeEnemyPortraits.Count];

                for (int i = 0; i < activeEnemyPortraits.Count; i++)
                {
                    pos[i] = activeEnemyPortraits[i].transform.position;
                }

                return pos;
            }
        }

        public bool InActiveParty(BattleChar battler)
        {
            return activeParty.Contains(battler);
        }

        public bool InActiveEnemies(BattleChar battler)
        {
            return activeEnemies.Contains(battler);
        }

        public int GetBattlerIndex(BattleChar battler)
        {
            return activeBattlers.IndexOf(battler);
        }

        public BattleChar GetActiveBattler(int index)
        {
            var i = Mathf.Abs(index) % activeBattlers.Count;
            return activeBattlers[i];
        }

        public BattlePortrait GetPortrait(BattleChar battler)
        {
            foreach(var partyPortrait in activePartyPortraits)
            {
                if (partyPortrait._character == battler) return partyPortrait;
            }

            foreach (var enemyPortrait in activeEnemyPortraits)
            {
                if (enemyPortrait._character == battler) return enemyPortrait;
            }

            return null;
        }

        public BattlePortrait[] GetPortraits(BattleChar[] battlers)
        {
            BattlePortrait[] portraits = new BattlePortrait[battlers.Length];

            for(int i = 0; i < battlers.Length; i++)
            {
                portraits[i] = GetPortrait(battlers[i]);
            }

            return portraits;
        }
        #endregion

        private void Awake()
        {
            game = GetComponent<GameManager>();
            ui = GetComponent<UIManager>();
        }

        #region  SM_Methods
        public void InitSM()
        {
            sm.AddState(StateID.BattleStart, new BattleStartState(this));
            sm.AddState(StateID.BattleSwitch, new BattleSwitchState(this));
            sm.AddState(StateID.BattleRun, new BattleRunState(this));

            sm.AddState(StateID.BattleWin, new BattleWinState(this));
            sm.AddState(StateID.BattleLose, new BattleLoseState(this));
        }

        public void UpdateSM()
        {
            sm.Update();
        }
        #endregion

        #region InitMethods
        public void InitBattle()
        {
            regularAttack = (AttackSkill)GameManager.instance._skillDatabase.GetSkill(SkillID.RegularAttack);

            GameObject partyHolder = new GameObject("Party Holder");
            GameObject enemyHolder = new GameObject("Enemy Holder");

            GameObject reticleObj = Instantiate(GameManager.instance._ui._reticle, 
                Vector3.zero, Quaternion.identity) as GameObject;
            this.reticleObj = reticleObj;
            this.reticleObj.gameObject.SetActive(false);

            Party party = game._party;

            party.PartyEnterBattle(combatantSideCap);
            partyMembers = party._party;

            encounter = FindObjectOfType<Encounter>();
            encounter.InitEncounter(game._encounterLevel);
            enemies = encounter._enemies;

            for (int i = 0; i < combatantSideCap; i++)
            {
                if (i < partyMembers.Length && partyMembers[i] != null)
                {
                    activeParty.Add(partyMembers[i]);
                    activeBattlers.Add(partyMembers[i]);

                    GameObject battlerObj = InstantiateBattler(partyMembers[i], playerPositions[i]);
                    battlerObj.transform.SetParent(partyHolder.transform);

                    BattlePortrait portrait = battlerObj.GetComponent<BattlePortrait>();
                    activePartyPortraits.Add(portrait);

                    sm.AddState(partyMembers[i], new PlayerTurn(this, partyMembers[i]));
                }

                if (i < enemies.Length && enemies[i] != null)
                {
                    activeEnemies.Add(enemies[i]);
                    activeBattlers.Add(enemies[i]);

                    GameObject battlerObj = InstantiateBattler(enemies[i], enemyPositions[i]);
                    battlerObj.transform.SetParent(enemyHolder.transform);

                    BattlePortrait portrait = battlerObj.GetComponent<BattlePortrait>();
                    activeEnemyPortraits.Add(portrait);

                    sm.AddState(enemies[i], new EnemyTurn(this, enemies[i]));
                }
            }

            // Sort by speed
            CalculateTurnOrder();
        }

        public GameObject InstantiateBattler(BattleChar bc, Vector3 pos)
        {
            GameObject charObj;

            charObj = Instantiate(portraitPrefab.gameObject, pos, Quaternion.identity) as GameObject;

            charObj.GetComponent<BattlePortrait>().InitPortrait(bc);

            return charObj;
        }
        #endregion

        #region TurnMethods
        public void AdvanceTurn()
        {
            actions++;

            if (activeParty.Count <= 0)
            {
                StartCoroutine(LoseBattleCo());
                return;
            }

            if (activeEnemies.Count <= 0)
            {
                StartCoroutine(WinBattleCo());
                return;
            }

            currentBattlerIndex++;
            currentBattlerIndex = currentBattlerIndex % activeBattlers.Count;

            sm.ChangeState(activeBattlers[currentBattlerIndex]);
        }

        public void CalculateTurnOrder()
        {
            activeBattlers = activeBattlers.OrderByDescending(bc => bc._speed._currentStatValue).ToList();
        }
        #endregion

        #region AutoModeMethods
        public void DisableAutoMode()
        {
            autoMode = false;

            GameManager.instance._ui.SetAutoText(false);

            Time.timeScale = 1;
        }

        public void EnableAutoMode()
        {
            autoMode = true;

            GameManager.instance._ui.SetAutoText(true);

            Time.timeScale = 2;
        }

        public void ToggleAutoMode()
        {
            if (Input.GetKeyDown(autoKey))
            {
                autoMode = !autoMode;

                GameManager.instance._ui.SetAutoText(autoMode);

                if (autoMode) Time.timeScale = 2;
                else Time.timeScale = 1;
            }
        }
        #endregion

        public void SwitchAddBattler(BattleChar newChar)
        {
            BattleChar currentChar = activeBattlers[currentBattlerIndex];
            int currentCharPartyIndex = activeParty.IndexOf(currentChar);

            activeBattlers.Add(newChar);

            activeParty.Add(newChar);

            activePartyPortraits[currentCharPartyIndex].InitPortrait(newChar);

            ui._partyStats.SwapPanelCharacter(currentCharPartyIndex, newChar);

            sm.AddState(newChar, new PlayerTurn(this, newChar));

            CalculateTurnOrder();

            currentBattlerIndex = activeBattlers.IndexOf(currentChar);
        }

        public void SwitchRemoveBattler(BattleChar oldChar)
        {
            activeBattlers.Remove(oldChar);

            activeParty.Remove(oldChar);

            sm.RemoveState(oldChar);
        }

        public void SwitchBattler(BattleChar newChar)
        {
            BattleChar currentChar = activeBattlers[currentBattlerIndex];
            int currentCharPartyIndex = activeParty.IndexOf(currentChar);

            activeBattlers.Add(newChar);

            activeParty[currentCharPartyIndex] = newChar;

            newChar._battleStatus = BattleStatus.InBattle;
            currentChar._battleStatus = BattleStatus.Standby;

            activePartyPortraits[currentCharPartyIndex].InitPortrait(newChar);

            ui._partyStats.SwapPanelCharacter(currentCharPartyIndex, newChar);

            sm.AddState(newChar, new PlayerTurn(this, newChar));
            sm.RemoveState(currentChar);

            CalculateTurnOrder();

            currentBattlerIndex = activeBattlers.IndexOf(currentChar);
            activeBattlers.Remove(currentChar);
        }

        public void LeaveBattle()
        {
            // Remove items from lists
            activeParty.Clear();
            activeEnemies.Clear();
            activeBattlers.Clear();

            foreach(var state in sm._states.ToArray())
            {
                if (state.Key is BattleTurn) sm.RemoveState(state.Key);
            }
        }

        public void AddExpEarned(int amount)
        {
            currentExpEarned += Mathf.Abs(amount);
        }

        public void AddMoneyEarned(int amount)
        {
            currentMoneyEarned += Mathf.Abs(amount);
        }

        #region Coroutines
        public IEnumerator RemoveBattlerCo(BattleChar battler, float dt)
        {
            var portrait = GetPortrait(battler);

            yield return StartCoroutine(GetPortrait(battler).DieCo(dt));

            activeBattlers.Remove(battler);

            if (activeParty.Contains(battler))
            {
                activeParty.Remove(battler);
                activePartyPortraits.Remove(portrait);
            }

            if (activeEnemies.Contains(battler))
            {
                activeEnemies.Remove(battler);
                activeEnemyPortraits.Remove(portrait);
            }

            if (ui._partyStats._statsDict.ContainsKey(battler))
                ui._partyStats._statsDict[battler].gameObject.SetActive(false);

            if (ui._enemyStats._statsDict.ContainsKey(battler))
                ui._enemyStats._statsDict[battler].gameObject.SetActive(false);

            sm.RemoveState(battler);

            portrait._character = null;
        }

        IEnumerator WinBattleCo()
        {
            float delay = 1f;

            ui.LogMessage("The enemies are defeated!");

            yield return new WaitForSeconds(delay);

            Time.timeScale = 1;

            yield return StartCoroutine(ui.FadeToBlackScreen(fadeTime));

            sm.ChangeState(StateID.BattleWin);
        }

        IEnumerator LoseBattleCo()
        {
            float delay = 1f;

            ui.LogMessage("The party has been defeated...");

            yield return new WaitForSeconds(delay);

            Time.timeScale = 1;

            yield return StartCoroutine(ui.FadeToBlackScreen(fadeTime));

            sm.ChangeState(StateID.BattleLose);
        }

        IEnumerator LeaveBattleCo()
        {
            foreach(var state in sm._states.ToArray())
            {
                if (state.Key is BattleChar) sm.RemoveState(state.Key);
            }

            yield return StartCoroutine(ui.FadeToBlackScreen(fadeTime));

            sm.ChangeState(StateID.GameOverworld);

            yield return null;
        }
        #endregion
    }
}