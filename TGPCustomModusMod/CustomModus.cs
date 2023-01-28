using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TGPCustomModusMod
{
    public class CustomModus : MelonMod
    {
        public static AssetBundle modiBundle;
        public static readonly string[] customDescriptions = new string[] {
            "Queuestack:Access only the items in the front and back of the queue. First in, first/last out."
        };

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Changing the list of active modi");

            Type sylladexType = typeof(Sylladex);
            FieldInfo modiField = sylladexType.GetField("modi", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            modiField.SetValue(null, new List<string> { "Queue", "Stack", "Array", "Hashmap", "Tree", "Queuestack" });

            List<string> values = (List<string>)modiField.GetValue(null);

            foreach (string value in values)
            {
                MelonLogger.Msg(value);
            }

            MelonLogger.Msg("Loading AssetBundles");
            Stream modiBundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TGPCustomModusMod.Bundles.modi");
            modiBundle = AssetBundle.LoadFromStream(modiBundleStream);
            
            foreach (string assetName in modiBundle.GetAllAssetNames())
            {
                MelonLogger.Msg(assetName);
            }
        }
    }
}
