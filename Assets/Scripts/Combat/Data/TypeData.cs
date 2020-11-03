using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "New Type", menuName = "Combat/Type")]
    public class TypeData : ScriptableObject
    {
        [SerializeField] string typeName;
        [SerializeField] string typeNameShort;

        [SerializeField] TypeID id;

        [SerializeField] TypeID[] weaknesses;
        [SerializeField] TypeID[] resistances;
        [SerializeField] TypeID[] immunities;

        public string _typeName => typeName;
        public string _typeNameShort => typeNameShort;

        public TypeID _id => id;

        public TypeID[] _weaknesses => weaknesses;
        public TypeID[] _resistances => resistances;
        public TypeID[] _immunities => immunities;
    }
}