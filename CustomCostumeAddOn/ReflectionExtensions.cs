using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomCostumeAddOn
{
    public static class ReflectionExtensions
    {
        public static void RegisterCostumeSkin(CostumeSkinEntity costumeSkinEntity)
        {
            var costumeSkinDictionary = GetCostumeSkinDictionary();
            var costumeRelatedTable = GetCostumeRelatedTable();


            var costume = CostumeDatabase.FindCostumeByID(costumeSkinEntity.relatedCostumeID);
            if (costume == null)
            {
                UnityEngine.Object.Destroy(costumeSkinEntity);
                Core.Log("Costume with ID " + costumeSkinEntity.relatedCostumeID + " not found. Costume skin " + costumeSkinEntity.skinID + " registration failed.");
                return;
            }
            if (!costumeRelatedTable.ContainsKey(costume.id))
            {
                costumeRelatedTable[costume.id] = new List<CostumeSkinEntity>();
            }

            if(costumeRelatedTable[costume.id].Count > 0)
                costumeSkinEntity.skinID += "_" + costumeRelatedTable[costume.id].Count;
            costumeRelatedTable[costume.id].Add(costumeSkinEntity);
            costumeSkinDictionary[costumeSkinEntity.skinID] = costumeSkinEntity;
        }
        public static Dictionary<string, CostumeSkinEntity> GetCostumeSkinDictionary()
        {
            return typeof(CostumeDatabase).GetField("costumeSkinDictionary", BindingFlags.Static | BindingFlags.NonPublic).GetValue(typeof(CostumeDatabase)) as Dictionary<string, CostumeSkinEntity>;
        }
        public static Dictionary<string, List<CostumeSkinEntity>> GetCostumeRelatedTable()
        {
            return typeof(CostumeDatabase).GetField("costumeRelatedTable", BindingFlags.Static | BindingFlags.NonPublic).GetValue(typeof(CostumeDatabase)) as Dictionary<string, List<CostumeSkinEntity>>;
        }
    }
}
