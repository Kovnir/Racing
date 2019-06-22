using System;
using System.Collections.Generic;
using Kovnir.FastTweener;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using Zenject;
using Random = System.Random;

public class SettingsEditorWindow : EditorWindow
{
    private static Header header = new Header();
    private static GameSettings gameSettings;
    private const int PADDING = 10;
    const int MENU_HEIGHT = 30;
    private GUIStyle titleStyle;
    private Vector2 scrollPosition;
    private LevelSettings toRemove;
    private LevelSettings toUp;
    private static readonly string[] menuItems =
    {
        "General", "Levels",
    };
    private enum MenuItems
    {
        General = 0,
        Levels = 1,
    }

    private MenuItems currentMenuItem;
    
    [MenuItem("Racing/Level Settings")]
    static void Init()
    {
        SettingsEditorWindow window = (SettingsEditorWindow) GetWindow(typeof(SettingsEditorWindow));
        gameSettings = Resources.Load<GameSettings>("GameSettings");
        window.Show();
    }

    void OnGUI()
    {
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 15
            };
        }

        
        int headerHeight = header.Draw(position);
        GUILayout.Space(headerHeight + PADDING);
        
        DrawMenu();
        GUILayout.Space(PADDING);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUIStyle.none,GUI.skin.verticalScrollbar);
        switch (currentMenuItem)
        {
            case MenuItems.General:
                DrawGeneralMenu();
                break;
            case MenuItems.Levels:
                DrawLevelsMenu();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        EditorGUILayout.EndScrollView();

        //do it without check here because have no enough time
        //todo fix it
        EditorUtility.SetDirty(gameSettings);
    }

    private void DrawGeneralMenu()
    {
        if (BigButton("Add Level GameObjects to Current Scene"))
        {
            throw new NotImplementedException();
        }
        if (BigButton("Clear Saves"))
        {
            throw new NotImplementedException();
        }
    }
    
    private void DrawLevelsMenu()
    {
        toRemove = null;
        toUp = null;
        for (var index = 0; index < gameSettings.Levels.Count; index++)
        {
            var level = gameSettings.Levels[index];
            DrawOneLevel(level, index);
        }

        if (toRemove != null)
        {
            gameSettings.Levels.Remove(toRemove);
        }
        if (toUp != null)
        {
            int index = gameSettings.Levels.IndexOf(toUp);
            gameSettings.Levels.Remove(toUp);
            gameSettings.Levels.Insert(index-1, toUp);
        }
        
        GUILayout.Space(PADDING);
        if (BigButton("Add New Level"))
        {
            gameSettings.Levels.Add(new LevelSettings());
        }
    }

    private void DrawOneLevel(LevelSettings level, int id)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(false));
        GUILayout.Space(PADDING);
        GUILayout.Label("Level " + id, titleStyle);
        GUILayout.Space(PADDING);
        level.ThreeStarsTime = EditorGUILayout.FloatField("3 Start Time", level.ThreeStarsTime);
        level.TwoStarsTime = EditorGUILayout.FloatField("2 Start Time", level.TwoStarsTime);
        level.OneStarTime = EditorGUILayout.FloatField("1 Start Time", level.OneStarTime);
        level.SceneName = EditorGUILayout.TextField("Level Name", level.SceneName);
        if (BigButton("Remove", 10))
        {
            toRemove = level;
        }

        GUI.enabled = id != 0;
        if (BigButton("Up", 10))
        {
            toUp = level;
        }

        GUI.enabled = true;

        GUILayout.Space(PADDING);
        EditorGUILayout.EndVertical();
    }


    private bool BigButton(string text, int offset = 0)
    {
        var style = new GUIStyle(GUI.skin.button);
        style.fixedWidth = position.width - 8 - offset;
        style.fixedHeight = 30;
        return GUILayout.Button(text, style);
    }

    private void DrawMenu()
    {
        var style = new GUIStyle(EditorStyles.toolbarButton)
        {
            alignment = TextAnchor.MiddleCenter,
            fixedHeight = MENU_HEIGHT,
            fontSize = 10,
            fontStyle = FontStyle.Bold
        };
        currentMenuItem = (MenuItems) GUILayout.Toolbar((int) currentMenuItem, menuItems, style);
    }

    
}