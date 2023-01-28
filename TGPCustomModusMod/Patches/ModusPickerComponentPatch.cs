using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace TGPCustomModusMod
{
    /* Disabled due to not being a good way to do this lol
    [HarmonyPatch(typeof(LobbyPlayerList), "OnEnable")]
    class LobbyListOnEnablePatch
    {
        private static void Postfix(LobbyPlayerList __instance)
        {
            Type lobbyPlayerListType = typeof(LobbyPlayerList);
            FieldInfo modusPickerField = lobbyPlayerListType.GetField("_modusPicker", BindingFlags.NonPublic | BindingFlags.Instance);
            ModusPicker modusPicker = (ModusPicker)modusPickerField.GetValue(__instance);

            MelonLogger.Msg("Got _modusPicker field!");

            modusPicker.AddModus("Queue");
            modusPickerField.SetValue(__instance, modusPicker);

            MelonLogger.Msg("Set _modusPicker field!");
        }
    }
    */

    [HarmonyPatch(typeof(ModusPickerComponent), "Awake")]
    class ModusPickerComponentAwakePatch
    {
        private static void Prefix(ModusPickerComponent __instance)
        {
            Type modusPickerComponentType = typeof(ModusPickerComponent);
            FieldInfo optionsField =  modusPickerComponentType.GetField("options", BindingFlags.NonPublic | BindingFlags.Instance);
            optionsField.SetValue(__instance, new string[] { "Queue", "Stack", "Array", "Hashmap", "Tree", "Queuestack" });
        }
    }

    [HarmonyPatch(typeof(ModusPickerComponent), "ModusChange")]
    class ModusPickerComponentModusChangePatch
    {
        private static void Postfix(Image ___image, string to)
        {
            string assetName = "assets/" + to.ToLower() + "modus.png";
            if (CustomModus.modiBundle.Contains(assetName))
            {
                ___image.sprite = CustomModus.modiBundle.LoadAsset<Sprite>(assetName);
            }
        }
    }
}
