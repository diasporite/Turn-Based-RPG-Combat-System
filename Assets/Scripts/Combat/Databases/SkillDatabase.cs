using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "SkillDatabase", menuName = "Combat/Database/Skills")]
    public class SkillDatabase : ScriptableObject
    {
        [SerializeField] SkillData[] data;

        Dictionary<SkillID, SkillData> database = new Dictionary<SkillID, SkillData>();

        public Skill GetSkill(SkillID id)
        {
            if (database.ContainsKey(id))
                return database[id].GetSkill();
            else Debug.LogError("Database does not contain a skill with the ID: " + id.ToString());

            return null;
        }

        public void BuildDatabase()
        {
            foreach(var skill in data)
            {
                database.Add(skill._id, skill);
            }
        }
    }
}