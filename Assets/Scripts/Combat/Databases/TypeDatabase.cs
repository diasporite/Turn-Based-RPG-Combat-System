using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "TypeDatabase", menuName = "Combat/Database/Types")]
    public class TypeDatabase : ScriptableObject
    {
        [SerializeField] TypeData[] types;

        Dictionary<TypeID, TypeData> database = new Dictionary<TypeID, TypeData>();

        public void BuildDatabase()
        {
            foreach(var type in types)
            {
                database.Add(type._id, type);
            }
        }

        public float TypeModifier(TypeID attacking, TypeID defending, float weakMult, float resMult)
        {
            float modifier = 1;

            if (attacking == TypeID.Typeless || defending == TypeID.Typeless) return modifier;

            var defendingType = database[defending];

            if (defendingType._immunities.Contains(attacking)) return 0;
            if (defendingType._weaknesses.Contains(attacking)) modifier *= weakMult;
            if (defendingType._resistances.Contains(attacking)) modifier *= resMult;

            return modifier;
        }
    }
}