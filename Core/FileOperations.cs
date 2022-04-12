using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace WorkMate.Core
{
    internal class FileOperations
    {
        private static bool _encrypted = false;
        public static bool Encrypted
        {
            get { return _encrypted; }
            set { _encrypted = value; }
        }

        private static string _psw = "";
        public static string Psw
        {
            set { _psw = value; }
        }
        static public void CreateFile(Dictionary<string,object> data, string path, string file = "default.json")
        {
            string json = System.Text.Json.JsonSerializer.Serialize(data);
            if (_encrypted)
            {
                json = StringCypher.Encrypt(json, _psw);
            }

            File.WriteAllText(path + file, json);
        }
        static public void CreateDir(string path)
        {
            Directory.CreateDirectory(path);
        }
        static public void RemoveDir(string path, bool subs)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, subs);
            }
        }
        static public List<string> GetDirs(string path, bool just_name = true)
        {
            List<string> dirs = new List<string>();
            if (just_name)
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    dirs.Add(dir.Split('/')[dir.Split('/').Length - 1]);
                }
                
            }
            else
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    dirs.Add(dir);
                }
            }
            return dirs;
        }

        static public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        static public Dictionary<string, object> GetFile(string file = "default.json")
        {
            string dataread = File.ReadAllText(file);
            string datadict;
            if (_encrypted)
            {
                datadict = StringCypher.Decrypt(dataread, _psw);
            }
            else
            {
                datadict = dataread;
            }
            Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(datadict);
            return data;
        }
    }
}
