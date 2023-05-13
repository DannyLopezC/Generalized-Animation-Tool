using System;
using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class GeneralizedAnimation : EditorWindow
{
    public float animationTime;

    public List<UIAnimator> animations;
    public List<ResizableRects> rects;
    private Vector2 animationListScrollPosition = Vector2.zero;
    private Vector2 rectsScrollPosition = Vector2.zero;

    // variables para el foldout
    private bool isListFoldoutOpen;
    private bool isRectsFoldoutOpen;

    private void OnEnable()
    {
        animations = new List<UIAnimator>();
        rects = new List<ResizableRects>();
    }

    private void OnGUI()
    {
        animationTime = EditorGUILayout.FloatField("Animation Time", animationTime);

        CreateAnimationsList();

        DragAction();

        DrawRects();

        HandleResizeEvents();
    }

    private void DrawRects()
    {
        const float heightRect = 30;
        float widthRect = 100;
        const float xRectPos = 10;

        const float offSetBetweenRects = (heightRect / 2) + 10;

        isRectsFoldoutOpen = EditorGUILayout.Foldout(isRectsFoldoutOpen, "Timeline");
        float maxScrollHeight = 300f; // valor máximo de altura del scroll
        float totalHeight = 0f; // altura total de los objetos
        float scrollHeight = 0f;

        if (isRectsFoldoutOpen)
        {
            foreach (ResizableRects obj in rects)
            {
                totalHeight += heightRect + (heightRect / 2);
            }

            if (animations.Count < 1)
            {
                totalHeight = EditorGUIUtility.singleLineHeight;
            }

            scrollHeight = Mathf.Min(totalHeight, maxScrollHeight);

            rectsScrollPosition = EditorGUILayout.BeginScrollView(animationListScrollPosition, GUI.skin.box,
                GUILayout.Height(scrollHeight));


            for (int i = 0; i < animations.Count; i++)
            {
                rects[i] = new ResizableRects(xRectPos, 7 + offSetBetweenRects * i + (heightRect / 2) * i,
                    widthRect, heightRect);
                Handles.DrawSolidRectangleWithOutline(rects[i].basicRect, Color.blue, Color.white);
                Handles.DrawSolidRectangleWithOutline(rects[i].resizeRect, Color.red, Color.red);
            }

            EditorGUILayout.EndScrollView();
        }
    }

    private void HandleResizeEvents()
    {
        Event currentEvent = Event.current;
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                break;
            case EventType.MouseUp:
                break;
            case EventType.MouseDrag:
                break;
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
                    rects.Add(null);
                }
            }

            evt.Use();
        }
    }

    private void CreateAnimationsList()
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
            animationListScrollPosition =
                EditorGUILayout.BeginScrollView(animationListScrollPosition, GUI.skin.box, GUILayout.Height(scrollHeight));

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
                rects.Add(null);
            }

            if (GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(20)))
            {
                if (animations.Count > 0)
                {
                    animations.RemoveAt(animations.Count - 1);
                    rects.RemoveAt(rects.Count - 1);
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