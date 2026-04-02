using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCostumeAddOn
{
    public class Core : HorayModBase
    {
        protected override void OnModLoaded()
        {
            base.OnModLoaded();
            HorayModAPI.OnLoadCostumeDatabase += OnLoadCostumeDatabase;
            HorayModAPI.OnLoadItemDatabase += OnLoadItemDatabase;
            CustomCostumeDatabase.Initialize();
        }

        private void OnLoadItemDatabase()
        {
            CustomCostumeDatabase.LoadAllStartingItems();
        }

        private void OnLoadCostumeDatabase()
        {
            var example = CostumeDatabase.GetAll().First();
            Core.Log("Registering...");
            CustomCostumeDatabase.RegisterCostumes(example);
            CustomCostumeDatabase.RegisterCostumeSkins();
        }

        protected override void OnModUnloaded()
        {
            base.OnModUnloaded();
            HorayModAPI.OnLoadCostumeDatabase -= OnLoadCostumeDatabase;
            HorayModAPI.OnLoadItemDatabase -= OnLoadItemDatabase;
            CustomCostumeDatabase.Destroy();
        }
        public static Sprite LoadSpritePath(string path)
        {
            if (!File.Exists(path))
            {
                Core.Log(path + " dose not exist!");
                return null;
            }

            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(fileData);

            var sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.2f), 16
            );
            sprite.name = Path.GetFileNameWithoutExtension(path);
            sprite.texture.filterMode = FilterMode.Point;
            return sprite;
        }
        public static void Log(string message)
        {
            Debug.Log("[CustomCostumeAddOn] " + message);
        }
    }
}
