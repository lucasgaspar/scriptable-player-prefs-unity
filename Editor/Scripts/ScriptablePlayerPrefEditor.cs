using UnityEditor;
using UnityEngine;

namespace ScriptablePlayerPrefs
{
    [CustomEditor(typeof(ScriptablePlayerPref))]
    public class ScriptablePlayerPrefEditor : Editor
    {
        #region FIELDS

        private const string CustomKeyLabel = "Custom Key";
        private const string FieldSavedDataLabel = "Saved data";
        private const string ButtonClearDataLabel = "Clear data";
        private const string GUIDLabelFormat = "GUID: {0}";

        private ScriptablePlayerPref _saveData = null;
        private string _message = string.Empty;

        #endregion

        #region BEHAVIORS

        public void OnEnable()
        {
            _saveData = (ScriptablePlayerPref)target;
            _message = _saveData.Get(string.Empty);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            RenderCustomKeyField();
            RenderSavedDataField();
            RenderClearButton();
            EditorGUILayout.Space();
            RenderGUIDLabel();
        }

        private void RenderCustomKeyField()
        {
            string newKey = EditorGUILayout.TextField(CustomKeyLabel, _saveData.CustomKey);
            if (newKey == _saveData.CustomKey)
                return;

            if (string.IsNullOrEmpty(newKey))
            {
                _saveData.CustomKey = null;
            }
            else
            {
                _saveData.CustomKey = newKey;
            }

            _message = _saveData.Get(string.Empty);
        }

        private void RenderSavedDataField()
        {
            string newMessage = EditorGUILayout.TextField(FieldSavedDataLabel, _message);
            if (newMessage == _message)
                return;

            _message = newMessage;
            if (string.IsNullOrEmpty(_message))
                _saveData.Clear();
            else
                _saveData.Set(_message);
        }

        private void RenderClearButton()
        {
            if (!GUILayout.Button(ButtonClearDataLabel))
                return;

            _saveData.Clear();
            _message = string.Empty;
        }

        private void RenderGUIDLabel()
        {
            string guid = _saveData.GUID;
            if (string.IsNullOrEmpty(guid))
                return;

            EditorGUILayout.LabelField(string.Format(GUIDLabelFormat, guid), EditorStyles.centeredGreyMiniLabel);
        }

        #endregion
    }
}
