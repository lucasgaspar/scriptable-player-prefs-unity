using Newtonsoft.Json;
using UnityEngine;

namespace ScriptablePlayerPrefs
{
    [CreateAssetMenu(menuName = MenuName)]
    public class ScriptablePlayerPref : ScriptableObject
    {
        #region FIELDS

        protected const string MenuName = "Scriptable Player Pref";
        protected const string PlayerPrefFormat = "PlayerPref/{0}";

        [SerializeField, HideInInspector] private string _guid = null;

        #endregion

        #region PROPERTIES

        public string GUID
        {
            get
            {
#if UNITY_EDITOR
                UpdateGUID();
#endif
                return _guid;
            }
        }

        public string PlayerPrefKey
        {
            get
            {
                if (string.IsNullOrEmpty(GUID))
                    return string.Empty;

                return string.Format(PlayerPrefFormat, GUID);
            }
        }

        #endregion

        #region BEHAVIORS

        public bool HasData()
        {
            return PlayerPrefs.HasKey(PlayerPrefKey);
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(PlayerPrefKey);
        }

        public T Get<T>(T defaultValue = default)
        {
            if (string.IsNullOrEmpty(PlayerPrefKey) || !PlayerPrefs.HasKey(PlayerPrefKey))
                return defaultValue;

            try
            {
                string savedValue = PlayerPrefs.GetString(PlayerPrefKey);
                if (typeof(T) == typeof(string))
                    return (T)(object)savedValue;
                else if (typeof(T) == typeof(int))
                    return (T)(object)int.Parse(savedValue);
                else if (typeof(T) == typeof(float))
                    return (T)(object)float.Parse(savedValue);
                else if (typeof(T) == typeof(long))
                    return (T)(object)long.Parse(savedValue);
                else if (typeof(T) == typeof(bool))
                    return (T)(object)bool.Parse(savedValue);
                else
                    return Deserialize<T>(PlayerPrefs.GetString(PlayerPrefKey));
            }
            catch
            {
                return defaultValue;
            }
        }

        public void Set<T>(T value)
        {
            if (typeof(T) == typeof(string))
                PlayerPrefs.SetString(PlayerPrefKey, (string)(object)value);
            else
                PlayerPrefs.SetString(PlayerPrefKey, Serialize(value));
        }

        private string Serialize<T>(T value)
        {
            JsonSerializerSettings settings = new()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, settings);
        }

        private T Deserialize<T>(string json)
        {
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }


#if UNITY_EDITOR
        private void OnEnable()
        {
            UpdateGUID();
        }

        private void UpdateGUID()
        {
            if (UnityEditor.EditorApplication.isUpdating)
            {
                return;
            }

            string path = UnityEditor.AssetDatabase.GetAssetPath(this);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string currentGuid = UnityEditor.AssetDatabase.AssetPathToGUID(path);

            if (string.IsNullOrEmpty(currentGuid))
            {
                return;
            }

            if (_guid == currentGuid)
            {
                return;
            }

            _guid = currentGuid;
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif

        #endregion
    }
}
