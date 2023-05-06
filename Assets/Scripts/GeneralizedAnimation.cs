using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneralizedAnimation : EditorWindow
{
    public float animationTime;

    public List<GameObject> animations;
    private Vector2 scrollPosition = Vector2.zero;
    private GUIStyle scrollViewStyle;


    // variables para el foldout
    private bool isFoldoutOpen;

    private void OnGUI()
    {
        animationTime = EditorGUILayout.FloatField("Animation Time", animationTime);

        CreateAnimationsList();

        DragAction();
    }

    private void DragAction()
    {
        // Allow dragging animations into list
        Event evt = Event.current;
        if (evt.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            evt.Use();
        }
        else if (evt.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();
            foreach (Object draggedObject in DragAndDrop.objectReferences)
            {
                if (draggedObject is GameObject)
                {
                    animations.Add(draggedObject as GameObject);
                }
            }

            evt.Use();
        }
    }

    private void CreateAnimationsList()
    {
        // crear el foldout
        isFoldoutOpen = EditorGUILayout.Foldout(isFoldoutOpen, "Animations List");

        if (isFoldoutOpen)
        {
            float maxScrollHeight = 100f; // valor máximo de altura del scroll
            float totalHeight = 0f; // altura total de los objetos
            foreach (GameObject obj in animations)
            {
                totalHeight += EditorGUIUtility.singleLineHeight + 7f;
            }

            if (animations.Count < 1)
            {
                totalHeight = EditorGUIUtility.singleLineHeight;
            }

            float scrollHeight = Mathf.Min(totalHeight, maxScrollHeight);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUI.skin.box, GUILayout.Height(scrollHeight));

            // Add space to the left
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();

            // Draw list items
            for (int i = 0; i < animations.Count; i++)
            {
                animations[i] = (GameObject)EditorGUILayout.ObjectField("Animation " + (i + 1).ToString(), animations[i],
                    typeof(GameObject), true);
            }

            // End layout group
            GUILayout.EndVertical();

            // End horizontal group
            GUILayout.EndHorizontal();

            // End scroll view
            EditorGUILayout.EndScrollView();


            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.Height(20)))
            {
                animations.Add(null);
            }

            if (GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(20)))
            {
                if (animations.Count > 0)
                {
                    animations.RemoveAt(animations.Count - 1);
                }
            }

            GUILayout.EndHorizontal();
        }
    }

    [MenuItem("Window/Generalized Animation")]
    public static void ShowWindow()
    {
        GetWindow<GeneralizedAnimation>("Generalized Animation");
    }
}