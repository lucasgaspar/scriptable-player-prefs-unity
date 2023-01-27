using Newtonsoft.Json;
using UnityEngine;

namespace ScriptablePlayerPrefs
{
    [CreateAssetMenu(menuName = MenuName)]
    public class ScriptablePlayerPref : ScriptableObject
    {
        #region FIELDS

        protected const string MenuName = "Scriptable Player Pref";

        #endregion

        #region PROPERTIES

        private string HashCode { get => GetHashCode().ToString(); }

        #endregion

        #region BEHAVIORS

        public bool HasData()
        {
            return PlayerPrefs.HasKey(HashCode);
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(HashCode);
        }

        public T Get<T>(T defaultValue = default)
        {
            if (string.IsNullOrEmpty(HashCode) || !PlayerPrefs.HasKey(HashCode))
                return defaultValue;

            try
            {
                string savedValue = PlayerPrefs.GetString(HashCode);
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
                    return Deserialize<T>(PlayerPrefs.GetString(HashCode));
            }
            catch
            {
                return defaultValue;
            }
        }

        public void Set<T>(T value)
        {
            if (string.IsNullOrEmpty(HashCode))
                return;

            if (typeof(T) == typeof(string))
                PlayerPrefs.SetString(HashCode, (string)(object)value);
            else
                PlayerPrefs.SetString(HashCode, Serialize(value));
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

        #endregion
    }
}
