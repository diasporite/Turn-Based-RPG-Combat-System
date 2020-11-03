using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        bool gamePaused = false;

        [Header("Initial Battle Settings")]
        [Range(1, levelCap)]
        [SerializeField] int partyLevel = 1;
        [Range(1, levelCap)]
        [SerializeField] int encounterLevel = 1;

        [Header("Party Caps")]
        const int partyCap = 8;
        const int inventoryCap = 10;

        [Header("Combat Caps")]
        const int levelCap = 99;
        const int skillCap = 8;
        const int pointsCap = 9999;
        const int statCap = 999;
        const float buffCap = 2f;
        const float debuffCap = 0.5f;

        [Header("Databases")]
        [SerializeField] CharacterDatabase charDatabase;
        [SerializeField] SkillDatabase skillDatabase;
        [SerializeField] TypeDatabase typeDatabase;
        [SerializeField] ItemDatabase itemDatabase;

        [Header("Player Managers")]
        [SerializeField] Party party;
        [SerializeField] Inventory inventory;

        BattleManager battle;
        CombatManager combat;
        UIManager ui;

        StateMachine sm = new StateMachine();

        public bool _gamePaused { get { return gamePaused; }
            set { gamePaused = value; } }

        public int _partyLevel => partyLevel;
        public int _encounterLevel => encounterLevel;

        public int _partyCap => partyCap;
        public int _inventoryCap => inventoryCap;

        public int _levelCap => levelCap;
        public int _skillCap => skillCap;

        public int _pointsCap => pointsCap;
        public int _statCap => statCap;

        public float _buffCap => buffCap;
        public float _debuffCap => debuffCap;

        public CharacterDatabase _charDatabase => charDatabase;
        public SkillDatabase _skillDatabase => skillDatabase;
        public TypeDatabase _typeDatabase => typeDatabase;
        public ItemDatabase _itemDatabase => itemDatabase;

        public Party _party => party;
        public Inventory _inventory => inventory;

        public BattleManager _battle => battle;
        public CombatManager _combat => combat;
        public UIManager _ui => ui;

        public StateMachine _sm => sm;

        private void Awake()
        {
            //Screen.SetResolution(960, 540, false);

            if (instance == null) instance = this;
            else Destroy(gameObject);

            charDatabase.BuildDatabase();
            skillDatabase.BuildDatabase();
            typeDatabase.BuildDatabase();
            itemDatabase.BuildDatabase();

            party.BuildParty(partyLevel);
            inventory.BuildInventory();

            battle = GetComponent<BattleManager>();
            combat = GetComponent<CombatManager>();
            ui = GetComponent<UIManager>();

            InitSM();
        }

        private void Start()
        {
            gamePaused = false;

            ui.InitUI();

            sm.ChangeState(StateID.GameBattle);
        }

        private void Update()
        {
            sm.Update();
        }

        void InitSM()
        {
            sm.AddState(StateID.GameBattle, new BattleState(this));
            // Other game states added here
        }
    }
}