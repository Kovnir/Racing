using UnityEditor;
using UnityEngine;

public class Header
{
    private Texture iconTexture;
    private GUIStyle labelStyle;
    private Color labelColor = new Color(0.1f, 0.1f, 0.1f);
    private const int PADDING = 5;
    private const int ICON_HEIGHT = 30;
    private const int ICON_WIDTH = 50;
    private const int HEIGHT = ICON_HEIGHT + PADDING * 2;
    private const string TITLE = "RACING SETTINGS";

    private void CreateButtonStyle()
    {
        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 20;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = labelColor;
    }

    public int Draw(Rect screen)
    {
        if (labelStyle == null)
        {
            CreateButtonStyle();
        }

        if (iconTexture == null)
        {
            iconTexture = Resources.Load("icon") as Texture;
        }

        Rect fullRect = new Rect(0, 0, screen.width, HEIGHT);
        //hole rect
        EditorGUI.DrawRect(fullRect,
            EditorGUIUtility.isProSkin ? new Color(.3f, .3f, .3f) : new Color(0.85f, 0.85f, 0.85f));
        //line under rect
        EditorGUI.DrawRect(new Rect(0, HEIGHT - 2, screen.width, 1), Color.black);

        //logo
        Rect logoRect = new Rect(PADDING, PADDING, ICON_WIDTH, ICON_HEIGHT);
        GUI.DrawTexture(logoRect, iconTexture, ScaleMode.ScaleToFit, true);
        //text
        Rect textRect = new Rect(PADDING + ICON_WIDTH + PADDING, PADDING, screen.width, ICON_HEIGHT);
        GUI.Label(textRect, TITLE, labelStyle);
        return HEIGHT;
    }

}