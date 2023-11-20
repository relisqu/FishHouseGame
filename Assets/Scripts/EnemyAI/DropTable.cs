using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.EnemyAI
{
    [CreateAssetMenu(fileName = "New Drop Table", menuName = "Configs/DropTable", order = 0)]
    public class DropTable : SerializedScriptableObject
    {
        public Dictionary<Health.DamageType, Drop> Drops;
    }

    public class Drop
    {
        public int MinDropAmount;
        public int MaxDropAmount;
        public Item DropItem;
    }
}