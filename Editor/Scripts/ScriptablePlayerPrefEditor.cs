using System;
using UnityEditor;
using UnityEngine;

namespace LG.ScriptablePlayerPrefs
{
    [CustomEditor(typeof(ScriptablePlayerPref))]
    public class ScriptablePlayerPrefEditor : Editor
    {
        #region FIELDS

        private const string FieldSavedData = "Saved data";
        private const string ButtonClearData = "Clear data";

        private ScriptablePlayerPref _saveData = null;
        private string _message = String.Empty;

        #endregion

        #region BEHAVIORS

        public void OnEnable()
        {
            _saveData = (ScriptablePlayerPref)target;
            _message = _saveData.Load<string>(String.Empty);
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
            if (String.IsNullOrEmpty(_message))
                _saveData.Clear();
            else
                _saveData.Save<string>(_message);
        }

        private void RenderClearButton()
        {
            if (!GUILayout.Button(ButtonClearData))
                return;

            _saveData.Clear();
            _message = String.Empty;
        }

        #endregion
    }
}
