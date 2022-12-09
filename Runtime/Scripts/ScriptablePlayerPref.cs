using Newtonsoft.Json;
using UnityEngine;

namespace LG.ScriptablePlayerPrefs
{
    [CreateAssetMenu(menuName = MenuName)]
    public class ScriptablePlayerPref : ScriptableObject
    {
        #region FIELDS

        protected const string MenuName = "Scriptable Player Prefs/Scriptable Player Pref";

        #endregion

        #region PROPERTIES

        private string HashCode { get => GetHashCode().ToString(); }

        #endregion

        #region BEHAVIORS

        public bool HasData()
        {
            return UnityEngine.PlayerPrefs.HasKey(HashCode);
        }

        public void Clear()
        {
            UnityEngine.PlayerPrefs.DeleteKey(HashCode);
        }

        public T Load<T>(T defaultValue)
        {
            if (string.IsNullOrEmpty(HashCode) || !UnityEngine.PlayerPrefs.HasKey(HashCode))
                return defaultValue;

            string savedValue = UnityEngine.PlayerPrefs.GetString(HashCode);
            try
            {
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
            }
            catch
            {
                return defaultValue;
            }

            return Deserialize<T>(savedValue);
        }

        public void Save<T>(T value)
        {
            if (string.IsNullOrEmpty(HashCode))
                return;
                
            if (typeof(T) == typeof(string))
                UnityEngine.PlayerPrefs.SetString(HashCode, (string)(object)value);
            else
                UnityEngine.PlayerPrefs.SetString(HashCode, Serialize(value));
        }

        private string Serialize<T>(T value)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            return JsonConvert.SerializeObject(value, settings);
        }

        private T Deserialize<T>(string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        #endregion
    }
}
