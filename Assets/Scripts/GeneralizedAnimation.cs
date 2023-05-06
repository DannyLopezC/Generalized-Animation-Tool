using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;
using UnityEditor;
using UnityEngine;

public class GeneralizedAnimation : EditorWindow
{
    public float animationTime;

    public List<UIAnimator> animations;
    public List<Rect> rects;
    private Vector2 scrollPosition = Vector2.zero;

    // variables para el foldout
    private bool isListFoldoutOpen;
    private bool isRectsFoldoutOpen;

    private void OnGUI()
    {
        animationTime = EditorGUILayout.FloatField("Animation Time", animationTime);

        Vector2 lastItemPosition;
        CreateAnimationsList(out lastItemPosition);

        DragAction();

        DrawRects(lastItemPosition);
    }

    private void DrawRects(Vector2 lastItemPosition)
    {
        rects.Clear();
        const float height = 30;
        float width = 100;
        const float xPos = 10;

        const float offSetBetween = (height / 2) + 10;

        isRectsFoldoutOpen = EditorGUILayout.Foldout(isRectsFoldoutOpen, "Timeline");

        if (isRectsFoldoutOpen)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                Rect rect = new Rect(xPos, lastItemPosition.y + offSetBetween * i + (height / 2) * i, width, height);
                GUI.Box(rect, "hello");

                rects.Add(rect); // Agrega el rect generado a la lista
            }
        }
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
                if (draggedObject is not GameObject) continue;
                GameObject go = draggedObject as GameObject;
                if (go.TryGetComponent(out UIAnimator animator))
                {
                    animations.Add(animator);
                }
            }

            evt.Use();
        }
    }

    private float CreateAnimationsList(out Vector2 lastItemPosition)
    {
        // crear el foldout
        isListFoldoutOpen = EditorGUILayout.Foldout(isListFoldoutOpen, "Animations List");
        float maxScrollHeight = 100f; // valor máximo de altura del scroll
        float totalHeight = 0f; // altura total de los objetos
        float scrollHeight = 0f;


        if (isListFoldoutOpen)
        {
            foreach (UIAnimator obj in animations)
            {
                totalHeight += EditorGUIUtility.singleLineHeight + 7f;
            }

            if (animations.Count < 1)
            {
                totalHeight = EditorGUIUtility.singleLineHeight;
            }

            scrollHeight = Mathf.Min(totalHeight, maxScrollHeight);
            scrollPosition =
                EditorGUILayout.BeginScrollView(scrollPosition, GUI.skin.box, GUILayout.Height(scrollHeight));

            // Add space to the left
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();

            // Draw list items
            for (int i = 0; i < animations.Count; i++)
            {
                animations[i] = (UIAnimator)EditorGUILayout.ObjectField("Animation " + (i + 1).ToString(),
                    animations[i],
                    typeof(UIAnimator), true);
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

        lastItemPosition = new Vector2(20, 90 + scrollHeight); // posición del último elemento
        return scrollHeight;
    }

    [MenuItem("Window/Generalized Animation")]
    public static void ShowWindow()
    {
        GetWindow<GeneralizedAnimation>("Generalized Animation");
    }
}