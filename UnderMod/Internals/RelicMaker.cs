using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderModAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnderMod.Internals
{
    static class RelicMaker
    {
        public static void Test()
        {
            API.instance.GetLogger().Warn("RelicMaker: hooking events");

            //create relics
            API.instance.GetEvents().OnDataInitialized += RelicMaker_OnDataInitialized;
            API.instance.GetEvents().OnAvatarSpawned += RelicMaker_OnAvatarSpawned;
            API.instance.GetEvents().OnAvatarDestroyed += RelicMaker_OnAvatarDestroyed;
        }

        private static void RelicMaker_OnAvatarDestroyed(object sender, UnderModAPI.Events.Args.AvatarEventArgs e)
        {
            API.instance.GetLogger().Alert("By inheritance, this peasant shall be known as 'bwdy'.");
            Thor.GameData.Instance.SetPeonName("bwdy");
        }

        private static void RelicMaker_OnDataInitialized(object sender, UnderModAPI.Events.Args.DataInitializedEventArgs e)
        {
            API.instance.GetLogger().Alert("begin hackathon");
            List<Thor.ItemData> crList = new List<Thor.ItemData>();
            crList.Add(CreateRelic("fb73f3e50c7e4bc797f66f25b4a4dc03", "Warp Pipe", "Warps you to back to the beginning.", "World 1-1, as it were.", "assets/WarpPipe.png", Thor.EntityData.RarityType.Common, 1000));
            crList.Add(CreateRelic("fb645e7e74564d2aa0e57e7ec8205436", "Time Capsule", "Leave a random item behind for a future adventurer.", "Looks like it will hold 1 relic.", "assets/TimeCapsule.png", Thor.EntityData.RarityType.Common, 500));
            crList.Add(CreateRelic("b9cf8da2c0ba40ec81c8ed7693d39ca7", "Bag Lunch", "Refill your health on entering a new zone.", "There's a lot of food in there.", "assets/BagLunch.png", Thor.EntityData.RarityType.Common, 1000));
            crList.Add(CreateRelic("4b50fe29c8ed487e9bfb5097a85364e2", "Blank Map", "Get paid for each new room you discover.", "Hoodie's not used to competition.", "assets/BlankMap.png", Thor.EntityData.RarityType.Common, 1000));
            crList.Add(CreateRelic("cdceafb210174007b37e408165932804", "Cursed Wax", "Heals you significantly on gaining a curse.", "This can't be a good idea.", "assets/CursedWax.png", Thor.EntityData.RarityType.Common, 1000));
            crList.Add(CreateRelic("fa8e0d3c0e4940e2bf055f71c1eefbd9", "Cursifier", "Whenever you gain a curse, gain a second curse!", "Why would you touch this?", "assets/Cursifier.png", Thor.EntityData.RarityType.Common, 1000));
            crList.Add(CreateRelic("934de68da02c4810b97b6b005727340f", "Last Will", "Choose a relic to leave to your next of kin.", "No, you can't send them all.", "assets/LastWill.png", Thor.EntityData.RarityType.Common, 1000));
            crList.Add(CreateRelic("527315f36aeb4187a3490c487584ead4", "Pocket Furnace", "Chance of smelting gold drops into bars which are worth more.", "But which pocket does it go in?", "assets/PocketFurnace.png", Thor.EntityData.RarityType.Common, 1000));
            crList.Add(CreateRelic("77b17849cce447a89aed6800e2de217f", "Relic Catalog", "Choose from up to 10 relics instantly!", "Peasant tested, peasant approved.", "assets/RelicCatalog.png", Thor.EntityData.RarityType.Common, 1000));
            crList.Add(CreateRelic("4cb3572d3b3943e4a30c2721b45e7a5c", "Antique Coupon", "Shops will try to stock more relics.", "But only while supplies last!", "assets/AntiqueCoupon.png", Thor.EntityData.RarityType.Common, 1000));
            Thor.GameData.Instance.Items.AddRange(crList);
            API.instance.GetLogger().Alert("end hackathon");
        }

        private static void RelicMaker_OnAvatarSpawned(object sender, UnderModAPI.Events.Args.AvatarEventArgs e)
        {
            API.instance.GetLogger().Alert("By royal decree, this peasant shall be known as 'bwdy'.");
            Thor.SimulationPlayer av = e.AvatarInstance.GetSimulationPlayer() as Thor.SimulationPlayer;
            av.Avatar.name = "bwdy";
            Thor.GameData.Instance.SetPeonName("bwdy");
        }

        public static CustomRelic CreateRelic(string guid, string displayName, string description, string flavor, string iconFilePath, Thor.EntityData.RarityType rarity, int goldCost)
        {
            CustomRelic so = (CustomRelic)Thor.EntityData.CreateInstance(typeof(CustomRelic));
            so.Initialize(guid, displayName, description, flavor, iconFilePath, rarity, goldCost);
            Thor.GameData.Instance.RelicCollection.Add(so as Thor.DataObject);
            Thor.LootTableData LootTable = Thor.GameData.Instance.GetLootTable(Thor.ItemData.ItemHint.Relic);
            LootTable.Add(so as Thor.ItemData);
            return so;
        }

        public static bool HasCustomRelic(CustomRelic relic)
        {
            foreach (Thor.SimulationPlayer player in Thor.Game.Instance.Simulation.Players)
            {
                //if (player.HasItem(relic as Thor.ItemData))
                {
                    return true;
                }
            }
            return false;
        }

    }

    public class CustomRelic : Thor.ItemData
    {
        public CustomRelic()
        {
            hideFlags = UnityEngine.HideFlags.None;
            //Thor.ExtendedExternalBehaviorTree behaviour = new Thor.ExtendedExternalBehaviorTree().SetBehaviorSource(monobehaviourscriptreference);
            //todo and default
            Reflector.SetField(this, "m_hint", ItemHint.Relic, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_slot", "Relic", typeof(Thor.ItemData));
            Reflector.SetField(this, "m_audio", "event:/Items/Items_Land", typeof(Thor.ItemData));
            Reflector.SetField(this, "m_tags", new List<Thor.Tag> { new Thor.Tag() { Value = "relic" } }, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_maxDropCount", 1, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_isDefault", true, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_isDefaultDiscovered", true, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_isDeprecated", false, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_autoUnlock", true, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_rerollable", true, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_comboPiece", null, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_comboResult", null, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_userData", 0, typeof(Thor.ItemData));
            Reflector.SetField(this, "m_dropRequirements", new List<Thor.Requirement>(), typeof(Thor.ItemData));
            Reflector.SetField(this, "m_discoverRequirements", new List<Thor.Requirement>(), typeof(Thor.ItemData));
            //Reflector.SetField(this, "m_pickedUpBehavior", behaviour, typeof(Thor.ItemData)); //TODO
        }

        //required!
        public CustomRelic Initialize(string guid, string displayName, string description, string flavor, string iconFilePath, RarityType rarity, int goldCost)
        {
            name = "Custom Relic: " + displayName;
            Reflector.SetField(this, "m_guid", guid, typeof(Thor.DataObject));
            Reflector.SetField(this, "m_displayName", CreateLocId(displayName), typeof(Thor.EntityData));
            Reflector.SetField(this, "m_description", CreateLocId(description), typeof(Thor.EntityData));
            Reflector.SetField(this, "m_flavor", CreateLocId(flavor), typeof(Thor.EntityData));
            Reflector.SetField(this, "m_icon", CreateSprite(iconFilePath), typeof(Thor.EntityData));
            Reflector.SetField(this, "m_portrait", CreateSprite(iconFilePath), typeof(Thor.EntityData));
            Reflector.SetField(this, "m_rarity", rarity, typeof(Thor.EntityData));
            CostGroup gcost = new CostGroup()
            {
                type = Thor.CostExt.CostType.Purchase,
                costs = new List<Thor.ResourceManager.Resource>()
                {
                    new Thor.ResourceManager.Resource(Thor.GameData.Instance.GoldResource, goldCost)
                }
            };
            Reflector.SetField(this, "m_costGroups", new List<CostGroup> { gcost }, typeof(Thor.ItemData));
            return this;
        }

        public Thor.LocID CreateLocId(string text)
        {
            return new Thor.LocID() { Id = -1, Text = text };
        }

        private UnityEngine.Sprite CreateSprite(string filePath, int w = 112, int h = 112)
        {
            filePath = Path.Combine(UnderMod.GetGameDirectory(), "UnderMine_Data", "Managed", "VortexMods", "UnderMod", filePath);
            UnityEngine.Texture2D texture = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                texture = new UnityEngine.Texture2D(w, h);
                texture.LoadImage(fileData);
            } else
            {
                API.instance.GetLogger().Error("Could not load texture: " + filePath);
            }

            Sprite sprite = Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), new UnityEngine.Vector2(texture.width / 2, texture.height / 2));
            return sprite;
        }
    }
}