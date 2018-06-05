using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[Serializable]
public class ScenarioEditorGUI : MonoBehaviour
{
    // ------------------------------------------------------------------------------------------------------------
    #region defs

    public static int PropertyPanelWidth = 400;

    public static readonly Color[] ActionGroup_Colors = // this must be exactly same amount of entries as ActionGroup enum
    {
        new Color(71 / 255.0f, 165 / 255.0f, 227 / 255.0f),	// story
		new Color(33 / 255.0f, 189 / 255.0f, 29 / 255.0f),		// characters
		new Color(228 / 255.0f, 129 / 255.0f, 70 / 255.0f),	// images
		new Color(231 / 255.0f, 68 / 255.0f, 69 / 255.0f),		// media
		new Color(195 / 255.0f, 68 / 255.0f, 231 / 255.0f),	// effects
		new Color(170 / 255.0f, 170 / 255.0f, 170 / 255.0f),	// other
	};

    public static readonly Color ActionPropLabelColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    #endregion
    // ------------------------------------------------------------------------------------------------------------
    #region styles

    public static GUIStyle LeftPanelBack_Style;
    public static GUIStyle ActionGroup_Style;
    public static GUIStyle ActionGroupCollapseIcon_Style;
    public static GUIStyle ActionListContainer_Style;

    public static GUIStyle MainPanelBack_Style;
    public static GUIStyle MainPanelContent_Style;
    public static GUIStyle Notification_Style;

    public static GUIStyle[] Action_Style = new GUIStyle[(int)ActionGroup.MAX];
    public static GUIStyle ActionPropertiesContainer_Style;
    public static GUIStyle ActionNote_Style;
    public static GUIStyle ActionCollapseButton_Style;
    public static GUIStyle ActionRemoveButton_Style;
    public static GUIStyle ActionActiveButton_Style;

    public static GUIStyle Frame_Style;
    public static GUIStyle FrameFill_Style;
    public static GUIStyle Label_Style;
    public static GUIStyle ObjectField_Style;

    public static GUIStyle LogoContainer_Style;

    #endregion
    // ------------------------------------------------------------------------------------------------------------
    #region textures

    public static Texture2D Texture_WinIcon;
    public static Texture2D Texture_Logo;

    public static Dictionary<string, Texture2D> Action_Icons = new Dictionary<string, Texture2D>();
    public static Texture2D Texture_VinomaIcon;
    public static Texture2D Texture_Timeline;
    public static Texture2D Texture_TimelineDot;
    public static Texture2D Texture_ActionActive;

    private static Texture2D[] Texture_Action = new Texture2D[(int)ActionGroup.MAX];
    private static Texture2D Texture_LeftPanelBack;
    private static Texture2D Texture_MainPanelBack;
    private static Texture2D Texture_ActionPropsContainer;
    private static Texture2D Texture_ActionNode;

    private static Texture2D Texture_Frame;
    private static Texture2D Texture_FrameFill;

    private static Texture2D Texture_LogoConainerBack;

    #endregion
    // ------------------------------------------------------------------------------------------------------------
    #region init

    public static void UseSkin()
    {
        LoadResources();
        DefineStyles();
    }

    private static void DefineStyles()
    {
        if (ActionGroup_Style != null) return;
        GUIStyle[] customStyles = GUI.skin.customStyles;

        #region Left Panel Styles

        LeftPanelBack_Style = new GUIStyle()
        {
            normal = { background = Texture_LeftPanelBack }
        };

        ActionGroup_Style = new GUIStyle(GUI.skin.label)
        {
            // the background texture will be tinted
            normal = { background = EditorGUIUtility.whiteTexture, textColor = Color.white },
            padding = new RectOffset(35, 5, 5, 5),
            margin = new RectOffset(2, 2, 0, 5),
            stretchWidth = true,
            //font = sansRegularFont,
            fontSize = 15,
            fontStyle = FontStyle.Bold,
        };

        ActionGroupCollapseIcon_Style = new GUIStyle()
        {
            normal = { textColor = Color.white },
            fontSize = 20,
            fontStyle = FontStyle.Bold,
        };

        ActionListContainer_Style = new GUIStyle()
        {
            padding = new RectOffset(7, 0, 0, 0),
        };

        MainPanelBack_Style = new GUIStyle()
        {
            normal = { background = Texture_MainPanelBack }
        };

        MainPanelContent_Style = new GUIStyle()
        {
            padding = new RectOffset(20, 0, 0, 0)
        };

        Notification_Style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 25,
            fontStyle = FontStyle.Bold,
            normal = { textColor = new Color(0f, 0f, 0f, 0.2f) },
            margin = new RectOffset(30, 20, 20, 20)
        };

        for (int i = 0; i < Action_Style.Length; i++)
        {
            Action_Style[i] = new GUIStyle()
            {
                normal = { background = Texture_Action[i], textColor = new Color(1f, 1f, 1f, 0.85f) },
                padding = new RectOffset(35, 0, 3, 0),
                margin = new RectOffset(2, 0, 0, -22),
                //font = sansRegularFont,
                fontSize = 14,
                fontStyle = FontStyle.Normal,
            };
        }

        ActionPropertiesContainer_Style = new GUIStyle()
        {
            normal = { background = Texture_ActionPropsContainer },
            margin = new RectOffset(40, 0, -22, 5),
            padding = new RectOffset(10, 10, 15, 5),
            border = new RectOffset(164, 3, 13, 6),
            stretchWidth = false,
        };

        ActionNote_Style = new GUIStyle()
        {
            richText = false,
            //font = sansRegularFont,
            fontSize = 11,
            fontStyle = FontStyle.Normal,
            clipping = TextClipping.Clip,
            wordWrap = false,
            padding = new RectOffset(5, 0, 5, 0),
            border = new RectOffset(0, 0, 1, 1),
            normal = { textColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.4f) : new Color(0f, 0f, 0f, 0.6f), background = Texture_ActionNode },
        };

        ActionCollapseButton_Style = new GUIStyle()
        {
            normal = { textColor = Color.white },
            fontSize = 16,
            fontStyle = FontStyle.Bold,
        };

        ActionRemoveButton_Style = new GUIStyle()
        {
            normal = { textColor = EditorGUIUtility.isProSkin ? new Color(0.9f, 0f, 0f, 0.5f) : new Color(0.6f, 0f, 0f, 0.35f) },
            fontSize = 14,
            fontStyle = FontStyle.Normal,
        };

        ActionActiveButton_Style = new GUIStyle()
        {
            normal = { textColor = EditorGUIUtility.isProSkin ? new Color(0.3f, 0.7f, 1.0f, 0.5f) : new Color(0f, 0.4f, 0.6f, 0.4f) },
            fontSize = 14,
            fontStyle = FontStyle.Normal,
        };

        #endregion
        #region GUI Styles

        Frame_Style = new GUIStyle(GUI.skin.box)
        {
            normal = { background = Texture_Frame },
            border = new RectOffset(2, 2, 2, 2),
        };

        FrameFill_Style = new GUIStyle(GUI.skin.box);
        if (!EditorGUIUtility.isProSkin)
        {
            FrameFill_Style = new GUIStyle(Frame_Style)
            {
                normal = { background = Texture_FrameFill },
            };
        }

        Label_Style = new GUIStyle(GUI.skin.label)
        {
            normal = { textColor = ActionPropLabelColor }
        };

        ObjectField_Style = new GUIStyle(EditorStyles.objectField)
        {
            normal = { textColor = ActionPropLabelColor }
        };

        LogoContainer_Style = new GUIStyle(GUI.skin.box)
        {
            stretchWidth = true,
            margin = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(5, 5, 5, 5),
            border = new RectOffset(0, 0, 0, 2),
            normal = { background = Texture_LogoConainerBack },
        };

        #endregion

        GUI.skin.customStyles = customStyles;
    }

    private static void LoadResources()
    {
        if (Texture_VinomaIcon != null) return;

        AddActionIcon("empty");

        // story
        AddActionIcon("Dialogue");
        AddActionIcon("Label");
        AddActionIcon("Goto");
        AddActionIcon("Branch");
        AddActionIcon("Buttons");
        AddActionIcon("Pages");
        AddActionIcon("Hotspots");

        // character
        AddActionIcon("Enter Scene");
        AddActionIcon("Exit Scene");
        AddActionIcon("Change Pose");

        // visual
        AddActionIcon("Change Background");
        AddActionIcon("Panel");
        AddActionIcon("Text");
        AddActionIcon("Image");

        // media
        AddActionIcon("Start Music");
        AddActionIcon("Stop Music");
        AddActionIcon("Play Sound");
        AddActionIcon("Stop Sound");

        // effects
        AddActionIcon("Wait");
        AddActionIcon("Animation");
        AddActionIcon("Move Object");

        // other
        AddActionIcon("Script");
        AddActionIcon("GameObject");
        AddActionIcon("Console");
        AddActionIcon("Variable");
        AddActionIcon("Switch");
        AddActionIcon("Blox Event");

        // ...
        for (int i = 0; i < Texture_Action.Length; i++)
        {
            Texture_Action[i] = LoadTextureResource("action_" + i);
        }
        
        Texture_ActionActive = LoadTextureResource("action_a");
        Texture_VinomaIcon = LoadTextureResource("icon");
        Texture_LeftPanelBack = LoadTextureResource("left_panel");
        Texture_MainPanelBack = LoadTextureResource("main_panel");
        Texture_ActionPropsContainer = LoadTextureResource("action_props_container");
        Texture_ActionNode = LoadTextureResource("action_note.png");
        Texture_Timeline = LoadTextureResource("timeline");
        Texture_TimelineDot = LoadTextureResource("timeline_dot");

        Texture_Frame = LoadTextureResource("frame");
        Texture_FrameFill = LoadTextureResource("framefill");

        Texture_Logo = LoadTextureResource("logo");
        Texture_LogoConainerBack = LoadTextureResource("logo_back");

    }

    private static void AddActionIcon(string key)
    {
        if (!Action_Icons.ContainsKey(key))
        {
            Action_Icons.Add(key, LoadIcon("icons/" + key));
        }
        Debug.Log(Action_Icons.Count);
    }

    private static Texture2D LoadTextureResource(string path)
    {
        Texture2D t = Resources.Load<Texture2D>(path);
        return t;
    }

    private static Texture2D LoadIcon(string name)
    {
        Texture2D t = Resources.Load<Texture2D>(name);
        return t;
    }

    public static Texture2D ActionIcon(string name)
    {
        if (Action_Icons.ContainsKey(name)) return Action_Icons[name];
        return Action_Icons["empty"];
    }

    #endregion
}