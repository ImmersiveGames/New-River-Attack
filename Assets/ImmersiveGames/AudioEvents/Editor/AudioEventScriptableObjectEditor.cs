using UnityEditor;
using UnityEngine;

namespace ImmersiveGames.AudioEvents.Editor
{
    [CustomEditor(typeof(AudioEventSo), true)]
    public class AudioEventScriptableObjectEditor : UnityEditor.Editor
    {
        [SerializeField] private AudioSource previewer;
        public void OnEnable()
        {
            previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        }

        public void OnDisable()
        {
            DestroyImmediate(previewer.gameObject);
        }
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            if (GUILayout.Button("Preview"))
            {
                ((AudioEvent)target).PreviewPlay(previewer);
            }
            if (GUILayout.Button("Stop"))
            {
                ((AudioEvent)target).PreviewStop(previewer);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
