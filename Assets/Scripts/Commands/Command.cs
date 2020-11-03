using System.Collections;

namespace RPG_Project
{
    public class Command
    {
        protected BattleChar instigator;
        protected BattleChar[] targets;

        protected GameManager game;

        public BattleChar _instigator
        {
            get => instigator;
            set => instigator = value;
        }

        public BattleChar[] _targets
        {
            get => targets;
            set => targets = value;
        }

        public Command(BattleChar instigator)
        {
            this.instigator = instigator;

            game = GameManager.instance;
        }

        public virtual IEnumerator ExecuteCo()
        {
            yield return null;
        }
    }
}