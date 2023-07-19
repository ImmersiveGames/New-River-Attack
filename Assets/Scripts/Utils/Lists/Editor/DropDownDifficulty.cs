using System.Collections.Generic;
using RiverAttack;
using UnityEditor;
namespace Utils.Lists.Editor
{
    [CustomEditor(typeof(ObstacleMoveByApproach), true)]
    public class DropDownDifficulty : UnityEditor.Editor
    {

        DifficultyList m_EnemyDifficultyList;
        SerializedProperty m_DifficultType;
        List<string> m_DifficultList;
        int m_IndexDifficult;

        public void OnEnable()
        {
            SetInitialReferences();
        }
        void SetInitialReferences()
        {
            var script = (ObstacleMoveByApproach)target;
            m_EnemyDifficultyList = script.enemyDifficultyList;
            //enemyDifficultyList = Selection.activeGameObject.GetComponent<EnemyDifficulty>().GetDifficultList();
            SetDropDownDifficult();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            GetDropDownDifficult();
            serializedObject.ApplyModifiedProperties();
        }

        void SetDropDownDifficult()
        {
            if (!m_EnemyDifficultyList || m_EnemyDifficultyList.difficultiesList.Count <= 0)
                return;
            m_DifficultList = m_EnemyDifficultyList.ListDifficultyByName();
            m_IndexDifficult = 0;
            m_DifficultType = serializedObject.FindProperty("difficultType");
            m_IndexDifficult = m_DifficultList.IndexOf(m_DifficultType.stringValue);
        }

        void GetDropDownDifficult()
        {
            if (!m_EnemyDifficultyList || m_DifficultList.Count <= 0)
                return;
            m_IndexDifficult = EditorGUILayout.Popup("Enemy Difficult", m_IndexDifficult, m_DifficultList.ToArray(), EditorStyles.popup);
            if (m_IndexDifficult < 0)
                m_IndexDifficult = 0;
            m_DifficultType.stringValue = m_DifficultList[m_IndexDifficult];
        }
    }
}
