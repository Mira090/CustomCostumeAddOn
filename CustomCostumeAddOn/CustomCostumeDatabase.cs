using FMODUnity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AnimationSet.StateInfo;
using Event = AnimationSet.StateInfo.Event;

namespace CustomCostumeAddOn
{
    public static class CustomCostumeDatabase
    {
        public static readonly bool Log = true;
        public static readonly bool LogMetadataToEntity = false;
        public static readonly string Metadata = "Metadata.json";
        private static Dictionary<string, CustomCostumeEntity> costumeDictionary = null;
        private static Dictionary<string, CustomCostumeMetadata> metadataDictionary = null;
        public static void Initialize()
        {
            costumeDictionary = new Dictionary<string, CustomCostumeEntity>();
            metadataDictionary = new Dictionary<string, CustomCostumeMetadata>();

            CustomCostumeEntity[] array = LoadAll(Path.Combine(Application.streamingAssetsPath, "Costume"));
            foreach (CustomCostumeEntity costumeEntity in array)
            {
                costumeDictionary[costumeEntity.id] = costumeEntity;
                Core.Log("New Costume: " + costumeEntity.id);
            }
        }
        [Obsolete]
        public static void Reload()
        {
            Core.Log("Reload Costume");
            Destroy();
            Initialize();
            CostumeDatabase.Destroy();
            CostumeDatabase.Initialize();
            LoadAllStartingItems(CostumeDatabase.GetAll());
        }
        public static void InitializeGameObjects(PlayerAvatarCostume example)
        {
            foreach (var entity in costumeDictionary.Values)
            {
                entity.CreateGameObject(example);
            }
        }
        public static void RegisterCostumes(CostumeEntity example)
        {
            if (example.costumePrefab.TryGetComponent<PlayerAvatarCostume>(out var costume))
            {
                CustomCostumeDatabase.InitializeGameObjects(costume);
                foreach(var mod in CustomCostumeDatabase.CreateAll())
                {
                    if(CostumeDatabase.FindCostumeByID(mod.id) != null)
                    {
                        Core.Log("Costume ID already exists! " + mod.id);
                        UnityEngine.Object.Destroy(mod);
                        continue;
                    }
                    CostumeDatabase.Register(mod);
                    SaveManager.Current.SetString("PlayerCostume_CurrentSkin_" + mod.id, string.Empty);
                }
            }
        }
        public static void RegisterCostumeSkins()
        {
            foreach(var skin in CustomCostumeDatabase.CreateAllSkin())
            {
                ReflectionExtensions.RegisterCostumeSkin(skin);
            }
        }

        public static void Destroy()
        {
            foreach (var entity in costumeDictionary.Values)
            {
                GameObject.Destroy(entity.costumePrefab);
                GameObject.Destroy(entity);
            }
            costumeDictionary = null;
            metadataDictionary = null;
        }

        public static CustomCostumeEntity FindCostumeByID(string id)
        {
            if (costumeDictionary.TryGetValue(id, out CustomCostumeEntity result))
            {
                return result;
            }
            return null;
        }

        public static IEnumerable<CustomCostumeEntity> GetAll()
        {
            return costumeDictionary.Values;
        }
        public static IEnumerable<CostumeEntity> CreateAll()
        {
            return GetAll().Select(x => x.ToNormal());
        }
        public static IEnumerable<CostumeSkinEntity> CreateAllSkin()
        {
            return GetAll().Select(x => x.ToSkin());
        }
        public static void LoadAllStartingItems(IEnumerable<CostumeEntity> entities)
        {
            if (Log)
                Core.Log("Loading... CustomeCostume StartingItems");
            foreach (var entity in entities)
            {
                if (metadataDictionary.ContainsKey(entity.id) && metadataDictionary[entity.id].startingItems != null)
                {
                    entity.startingItems = metadataDictionary[entity.id].startingItems.Select(ItemDatabase.FindItemById).Where(x => x != null).ToArray();
                }
            }
        }
        public static CostumeEntity ToNormal(this CustomCostumeEntity custom)
        {
            var entity = ScriptableObject.CreateInstance<CostumeEntity>();
            entity.name = custom.name;
            entity.id = custom.id;
            entity.icon = custom.animationData[0].timeline[0].sprite;
            entity.stats = custom.stats;
            entity.costumePrefab = custom.costumePrefab;
            entity.aName = new LocalizedString(custom.costumeName);
            entity.aFlavorText = new LocalizedString(custom.costumeFlavorText);
            entity.order = 500;
            entity.costumeType = ECostumeUnlockType.Default;
            return entity;
        }
        public static CostumeSkinEntity ToSkin(this CustomCostumeEntity custom)
        {
            var entity = ScriptableObject.CreateInstance<CostumeSkinEntity>();
            entity.name = custom.name;
            entity.relatedCostumeID = custom.id;
            entity.skinID = custom.id + "_Skin";
            entity.icon = custom.animationData[0].timeline[0].sprite;
            entity.purchasePrice = 30;
            entity.skinPrefab = custom.costumePrefab;
            entity.aName = new LocalizedString(custom.costumeName);
            entity.unlockType = CostumeSkinEntity.ECostumeUnlockType.Default;
            return entity;
        }

        public static CustomCostumeEntity[] LoadAll(string path)
        {
            if (Log)
                Core.Log("LoadAll: " + path);
            var paths = Directory.GetDirectories(path);
            var list = new List<CustomCostumeEntity>();
            foreach (var p in paths)
            {
                var entity = Load(p);
                if (entity != null)
                    list.Add(entity);
            }
            return list.ToArray();
        }
        public static CustomCostumeEntity Load(string path)
        {
            if (Log)
                Core.Log("Load: " + path);
            var metadataPath = Path.Combine(path, Metadata.ToLowerInvariant());
            if (!File.Exists(metadataPath))
            {
                metadataPath = Path.Combine(path, Metadata);
                if (!File.Exists(metadataPath))
                {
                    Core.Log("metadata.json does not exist! " + metadataPath);
                    return null;
                }
            }
            var metadata = LoadMetadata(metadataPath);
            var entity = MetadataToEntity(metadata, path);
            if (entity != null)
                metadataDictionary[entity.id] = metadata;
            return entity;
        }
        public static CustomCostumeMetadata LoadMetadata(string path)
        {
            if (Log)
                Core.Log("LoadMetadata: " + path);
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var streamReader = new StreamReader(fileStream);
            var metadata = JsonConvert.DeserializeObject<CustomCostumeMetadata>(streamReader.ReadToEnd());
            streamReader.Close();
            fileStream.Close();
            return metadata;
        }
        public static CustomCostumeEntity MetadataToEntity(CustomCostumeMetadata metadata, string dir)
        {
            if (Log)
                Core.Log("MetadataToEntity: " + dir);
            var entity = ScriptableObject.CreateInstance<CustomCostumeEntity>();
            entity.name = metadata.id;
            entity.comment = metadata.comment;
            if (LogMetadataToEntity)
                Core.Log("MetadataToEntity 0");
            entity.id = metadata.id;
            entity.costumeName = metadata.costumeName;
            entity.costumeFlavorText = metadata.costumeFlavorText;
            entity.hitboxSize = metadata.hitboxSize;
            entity.movementCollisionRadius = (float)metadata.movementCollisionRadius;
            entity.hasBackImage = metadata.hasBackImage;
            if (LogMetadataToEntity)
                Core.Log("MetadataToEntity 1");
            if (metadata.stats != null)
                entity.stats = metadata.stats.ToArray();
            else
                entity.stats = new string[0];
            if (LogMetadataToEntity)
                Core.Log("MetadataToEntity 2");

            var state = new List<AnimationSet.StateInfo>();
            foreach (var animation in metadata.animationData)
            {
                if (LogMetadataToEntity)
                    Core.Log("MetadataToEntity 3");
                var info = new AnimationSet.StateInfo();
                info.state = animation.state;
                info.fps = animation.fps;
                info.repeat = animation.repeat;

                info.timeline = new List<SpriteKeyFrame>();
                if (LogMetadataToEntity)
                    Core.Log("MetadataToEntity 4");
                foreach (var time in animation.timeline)
                {
                    var timeline = new SpriteKeyFrame();
                    timeline.frameIdx = time.frameIdx;
                    timeline.sprite = Core.LoadSpritePath(Path.Combine(dir, time.sprite));
                    info.timeline.Add(timeline);
                }

                if (LogMetadataToEntity)
                    Core.Log("MetadataToEntity 5");
                if (animation.frameEvents != null)
                {
                    info.frameEvents = new List<FrameEvent>();
                    foreach (var frame in animation.frameEvents)
                    {
                        var frameEvent = new FrameEvent();
                        frameEvent.frame = frame.frame;

                        frameEvent.events = new List<Event>();
                        foreach (var ev in frame.events)
                        {
                            var even = new Event();
                            even.methodName = ev.methodName;
                            even.componentName = ev.componentName;
                            even.priority = ev.priority == "Essential" ? Event.EPriority.Essential : Event.EPriority.Ignorable;
                            frameEvent.events.Add(even);
                        }
                        info.frameEvents.Add(frameEvent);
                    }
                }

                if (LogMetadataToEntity)
                    Core.Log("MetadataToEntity 6");
                if (animation.soundEvents != null)
                {
                    info.soundEvents = new List<FrameSoundEvent>();
                    foreach (var sound in animation.soundEvents)
                    {
                        var soundEvent = new FrameSoundEvent();
                        soundEvent.frame = sound.frame;
                        soundEvent.events = new List<SoundEvent>();
                        foreach (var ev in sound.events)
                        {
                            var even = new SoundEvent();
                            even.attachToPerformer = ev.attachToPerformer;
                            even.path = RuntimeManager.PathToEventReference(ev.path);
                            soundEvent.events.Add(even);
                        }
                        info.soundEvents.Add(soundEvent);
                    }
                }
                state.Add(info);
            }
            if (LogMetadataToEntity)
                Core.Log("MetadataToEntity 7");
            entity.animationData = state.ToArray();

            if (LogMetadataToEntity)
                Core.Log("MetadataToEntity 8");

            return entity;
        }
        public static GameObject CreateGameObject(this CustomCostumeEntity entity, PlayerAvatarCostume example)
        {
            var clone = UnityEngine.Object.Instantiate(example);
            clone.transform.position = new Vector3(-100000, -100000, 0);
            clone.gameObject.name = entity.id;
            var animator = clone.gameObject.GetComponent<Animator2D_MultipleSpriteRenderer>();

            var set = ScriptableObject.CreateInstance<AnimationSet>();
            set.name = entity.id;
            set.sprites = entity.animationData.ToList();
            animator.ChangeSet(set);
            clone.movementColliderRadius = entity.movementCollisionRadius;
            var collider = clone.hitbox.gameObject.GetComponent<CircleCollider2D>();

            collider.radius = 0.25f * entity.hitboxSize;
            collider.offset = new Vector2(0, 0.25f * entity.hitboxSize);

            entity.costumePrefab = clone.gameObject;
            return clone.gameObject;
        }
    }
}
