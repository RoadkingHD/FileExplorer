using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using FileExplorer.Models;

namespace FileExplorer.Services
{
    public static class CustomActionStore
    {
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

        private static string SettingsDirectory
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FileExplorer"); }
        }

        private static string StoreFilePath
        {
            get { return Path.Combine(SettingsDirectory, "custom-actions.json"); }
        }

        public static List<CustomAction> Load()
        {
            try
            {
                if (!File.Exists(StoreFilePath))
                {
                    return new List<CustomAction>();
                }

                return Serializer.Deserialize<List<CustomAction>>(File.ReadAllText(StoreFilePath)) ?? new List<CustomAction>();
            }
            catch (Exception)
            {
                return new List<CustomAction>();
            }
        }

        public static void Save(IEnumerable<CustomAction> actions)
        {
            Directory.CreateDirectory(SettingsDirectory);
            File.WriteAllText(StoreFilePath, Serializer.Serialize(actions));
        }
    }
}
