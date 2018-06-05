using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActionEditorWindow : EditorWindow {

    public static int propertyPanelWidth = 400;

    [MenuItem("Window/ScenarioEditor/ActionWindow")]
    public static void Show_ActionEditor()
    {
        GetWindow<ActionEditorWindow>("Action");
    }

    int leftSize = 10;
    Vector2 scrollPos = Vector2.zero;
    int activePanel = 0;
    bool[] actionGroupFolds;

    protected void OnEnable()
    {
        //VinomaEditorWindow.Reflect();
        //UpdateWindowIcon();
        actionGroupFolds = new bool[(int)ActionGroup.MAX];
        for (int i = 0; i < (int)ActionGroup.MAX; i++) actionGroupFolds[i] = 1 == EditorPrefs.GetInt("ScenarioEd.ActionGroupFold." + i, 1);
    }

    protected void OnGUI()
    {
        ScenarioEditorGUI.UseSkin();

        GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;

        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);//, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.scrollView);
            {
                if (activePanel == 0) DrawActionsPanel();
                else DrawResourcesPanel();
                GUILayout.Space(100);
            }
            EditorGUILayout.EndScrollView();
        }

        GUI.enabled = true;
    }

    private void DrawActionsPanel()
    {
        for (int i = 0; i < (int)ActionGroup.MAX; i++)
        {
            // render the group label
            EditorGUILayout.Space();
            GUI.backgroundColor = ScenarioEditorGUI.ActionGroup_Colors[i];
            if (GUILayout.Button(((ActionGroup)i).ToString(), ScenarioEditorGUI.ActionGroup_Style, GUILayout.ExpandWidth(true)))
            {
                actionGroupFolds[i] = !actionGroupFolds[i];
                EditorPrefs.SetInt("VinomaEd.ActionGroupFold." + i, actionGroupFolds[i] ? 1 : 0);
            }
            GUI.backgroundColor = Color.white;

            Rect r = GUILayoutUtility.GetLastRect();
            r.x += 2; r.y += 2; r.width = 25;
            GUI.Label(r, actionGroupFolds[i] ? "∨" : "∧", ScenarioEditorGUI.ActionGroupCollapseIcon_Style);
            /*
            Rect r2 = GUILayoutUtility.GetLastRect();
            Rect rr = r2;
            rr.width = 20; rr.height = 20; rr.x += 7; rr.y += 7;
            GUI.DrawTexture(rr, ScenarioEditorGUI.Action_Icons["Dialogue"]);
            */
            /*
            if (actionGroupFolds[i])
            {
                // render the group's actions
                EditorGUILayout.BeginVertical(ScenarioEditorGUI.ActionListContainer_Style);
                for (int j = 0; j < VinomaEditorWindow.actions.Count; j++)
                {
                    if ((int)VinomaEditorWindow.actions[j].att.Group == i)
                    {
                        r = VinomaEditorWindow.DrawActionLabel(VinomaEditorWindow.actions[j], true);
                        if (Event.current.button == 0 && Event.current.type == EventType.MouseDrag)
                        {
                            if (r.Contains(Event.current.mousePosition))
                            {
                                //plyEdGUI.ClearFocus();
                                DragAndDrop.PrepareStartDrag();
                                DragAndDrop.objectReferences = new UnityEngine.Object[0];
                                DragAndDrop.paths = null;
                                DragAndDrop.SetGenericData("VinomaActionNfo", VinomaEditorWindow.actions[j]);
                                DragAndDrop.StartDrag(VinomaEditorWindow.actions[j].att.Name);
                                Event.current.Use();
                            }
                        }

                    }
                }
                EditorGUILayout.EndVertical();
            }
            */
        }
        GUILayout.FlexibleSpace();
    }

    private void DrawResourcesPanel()
    {
        GUILayout.FlexibleSpace();
    }

}
