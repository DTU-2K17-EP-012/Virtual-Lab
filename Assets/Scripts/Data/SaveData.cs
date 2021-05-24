using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Data
{
    public static class SaveData
    {
        private const string NICKNAME_KEY = "NickName";
        private const string LASTROOMCODE_KEY = "LastRoomCode";
        private const string GAMEACTIVE_KEY = "GameActive";
        private const string PUNUSERID_KEY = "PUNUserID";

        public static string NickName
        {
            get
            {
                if (!PlayerPrefs.HasKey(NICKNAME_KEY))
                {
                    PlayerPrefs.SetString(NICKNAME_KEY, "");
                }
                string name = PlayerPrefs.GetString(NICKNAME_KEY);
#if UNITY_EDITOR
                if (!ParrelSync.ClonesManager.IsClone())
                {
                    name += "0";
                }
                else
                {
                    string cloneArgs = ParrelSync.ClonesManager.GetArgument();
                    string[] cloneArgsArr = cloneArgs.Split(' ');
                    name += cloneArgsArr[1];
                }
#endif
                return name;
            }

            set
            {
                PlayerPrefs.SetString(NICKNAME_KEY, value);
            }

        }
        public static string LastRoomCode
        {
            get
            {
                if (!PlayerPrefs.HasKey(LASTROOMCODE_KEY))
                {
                    PlayerPrefs.SetString(LASTROOMCODE_KEY, "");
                }
                return PlayerPrefs.GetString(LASTROOMCODE_KEY);
            }

            set
            {
                PlayerPrefs.SetString(LASTROOMCODE_KEY, value);
            }
        }

        public static bool PlayerInRoom
        {
            get
            {
                if (!PlayerPrefs.HasKey(GAMEACTIVE_KEY))
                {
                    PlayerPrefs.SetInt(GAMEACTIVE_KEY, 0);
                }
                return PlayerPrefs.GetInt(GAMEACTIVE_KEY) == 1;
            }

            set
            {
                PlayerPrefs.SetInt(GAMEACTIVE_KEY, value ? 1 : 0);
            }
        }

        public static string PUNUserID
        {
            get
            {
                if (!PlayerPrefs.HasKey(PUNUSERID_KEY))
                {
                    PlayerPrefs.SetString(PUNUSERID_KEY, "");
                }
                string name = PlayerPrefs.GetString(PUNUSERID_KEY);
#if UNITY_EDITOR
                string projectName = GetProjectName();
                return projectName;
#else
                return name;
#endif
            }

            set
            {
                PlayerPrefs.SetString(PUNUSERID_KEY, value);
            }
        }
        public static bool IsUserIDSet => PlayerPrefs.HasKey(PUNUSERID_KEY) && !string.IsNullOrEmpty(PlayerPrefs.GetString(PUNUSERID_KEY));
        private static string GetProjectName()
        {
            string dataPath = Application.dataPath;
            string[] array = dataPath.Split("/"[0]);
            return array[array.Length - 2];
        }
    }
}

