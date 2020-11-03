using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "CharDatabase", menuName = "Combat/Database/Characters")]
    public class CharacterDatabase : ScriptableObject
    {
        [SerializeField] CharData[] charData;

        Dictionary<CharID, CharData> database = new Dictionary<CharID, CharData>();

        public CharData GetCharData(CharID id)
        {
            if (database.ContainsKey(id))
                return database[id];
            else Debug.LogError("Database does not contain a character with the ID: " + id.ToString());

            return null;
        }

        public BattleChar GetChar(CharID id, int lv)
        {
            if (database.ContainsKey(id))
                return new BattleChar(database[id], lv);
            else Debug.LogError("Database does not contain a character with the ID: " + id.ToString());

            return null;
        }

        public Sprite GetCharSprite(CharID id)
        {
            if (database.ContainsKey(id))
                return database[id]._sprite;
            else Debug.LogError("Database does not contain a character with the ID: " + id.ToString());

            return null;
        }

        public void BuildDatabase()
        {
            foreach(var data in charData)
            {
                database.Add(data._id, data);
            }
        }
    }
}