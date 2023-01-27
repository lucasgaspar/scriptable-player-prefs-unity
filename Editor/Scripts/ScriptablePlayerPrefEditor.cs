using UnityEditor;
using UnityEngine;

namespace ScriptablePlayerPrefs
{
    [CustomEditor(typeof(ScriptablePlayerPref))]
    public class ScriptablePlayerPrefEditor : Editor
    {
        #region FIELDS

        private const string FieldSavedData = "Saved data";
        private const string ButtonClearData = "Clear data";

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
            RenderSavedDataField();
            RenderClearButton();
        }

        private void RenderSavedDataField()
        {
            string newMessage = EditorGUILayout.TextField(FieldSavedData, _message);
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
            if (!GUILayout.Button(ButtonClearData))
                return;

            _saveData.Clear();
            _message = string.Empty;
        }

        #endregion
    }
}
