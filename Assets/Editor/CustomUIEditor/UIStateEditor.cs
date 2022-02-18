using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(UIState))]
public class UIStateEditor : Editor
{
    bool fold;
    bool fold2;
    string[] options = new string[] { "RectTransform", "Image", "RawImage", "Text", "OutLine", "Gradient", "Shadow" };
    int selectidx = 0;
    
    public override void OnInspectorGUI()
    {
 
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.DropdownButton(new GUIContent("state"), FocusType.Keyboard))
        {

            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < options.Length; i++)
            {
                menu.AddItem(new GUIContent(options[i]), false, OnButtonAddComponent, options[i]);
            }
            menu.ShowAsContext();
        }

        SerializedProperty allStates = serializedObject.FindProperty("allStates");
        //selectidx = EditorGUILayout.Popup(selectidx, allStates);
        GUILayout.Button("����");
        GUILayout.Button("ɾ��");
        if (GUILayout.Button(new GUIContent("���ӿ������")))
        {
            
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < options.Length; i++)
            {
                menu.AddItem(new GUIContent(options[i]), false, OnButtonAddComponent, options[i]);
            }
            menu.ShowAsContext();
        }
        GUILayout.EndHorizontal();

        //EditorGUI.

        fold = EditorGUILayout.Foldout(fold, new GUIContent("RectTransform"));
        if(fold)
        {
            EditorGUILayout.Vector3Field("position", Vector3.zero);
        }

        base.OnInspectorGUI();
    }

    //����UI״̬
    private void OnButtonAddState()
    {

    }

    //ɾ��UI״̬
    private void OnButtonDeleteState()
    {

    }

    //���ӿ������
    private void OnButtonAddComponent(object val)
    {
        Debug.Log("Select DropDownBtn val = " + val.ToString());
    }



}
