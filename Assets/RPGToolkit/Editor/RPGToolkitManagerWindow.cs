using UnityEngine;
using UnityEditor;

namespace RPGToolkit
{
    public class RPGToolkitManagerWindow : EditorWindow
    {
        GUISkin customSkin;
        
        private bool showPlayerModule = false;
        private bool showQuestModule = false;
        private bool showColors = false;
        private bool showFonts = false;
        private bool showSounds = false;

        private GUIStyle foldoutStyle;
        private SerializedObject serializedObject;
        private RPGToolkitManager rpgToolkitManager;
        
        [MenuItem("Window/RPG Toolkit Manager")]
        public static void ShowWindow()
        {
            GetWindow<RPGToolkitManagerWindow>("RPG Toolkit Manager");
        }

        private void OnEnable()
        {
            // Load the existing ScriptableObject
            string path = "Assets/RPGToolkit/RPGToolkitManager.asset";
            rpgToolkitManager = AssetDatabase.LoadAssetAtPath<RPGToolkitManager>(path);

            if (rpgToolkitManager == null)
            {
                EditorGUILayout.HelpBox("RPGToolkitManager asset not found at " + path + ". Please ensure it exists.", MessageType.Error);
                return;
            }

            serializedObject = new SerializedObject(rpgToolkitManager);

            if (EditorGUIUtility.isProSkin)
            {
                customSkin = (GUISkin)Resources.Load("Editor/Manager Skin Dark");
            }
            else
            {
                customSkin = (GUISkin)Resources.Load("Editor/Manager Skin Light");
            }
        }

        private void OnGUI()
        {
            serializedObject.Update();

            if (customSkin == null)
            {
                EditorGUILayout.HelpBox("Editor variables are missing. You can manually fix this by deleting " +
                    "RPGToolkit > Resources folder and then re-import the package. \n\nIf you're still seeing this " +
                    "dialog even after the re-import, contact me with this ID: " + RPGToolkitManagerEditor.buildID, MessageType.Error);

                if (GUILayout.Button("Contact"))
                {
                    Email();
                }

                return;
            }

            // Foldout style
            foldoutStyle = customSkin.FindStyle("UIM Foldout");

            // UIM Header
            RPGToolkitEditorHandler.DrawHeader(customSkin, "UIM Header", 8);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Draw GUIs
            DrawPlayerModule();
            DrawQuestModule();

            DrawUIColors();
            DrawUIFonts();
            DrawUISounds();
            DrawUISettings();

            DrawSupport();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPlayerModule()
        {
            // Player Module
            var hasInventory = serializedObject.FindProperty("hasInventory");
            var hasQuestBook = serializedObject.FindProperty("hasQuestBook");
            var hasLevel = serializedObject.FindProperty("hasLevel");

            GUILayout.Space(RPGToolkitManagerEditor.foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showPlayerModule = EditorGUILayout.Foldout(showPlayerModule, "Player Module", true, foldoutStyle);
            showPlayerModule = GUILayout.Toggle(showPlayerModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutBottomSpace);

            if (showPlayerModule)
            {
                RPGToolkitEditorHandler.DrawProperty(hasInventory, customSkin, "Has Inventory");
                RPGToolkitEditorHandler.DrawProperty(hasQuestBook, customSkin, "Has Quest Book");
                RPGToolkitEditorHandler.DrawProperty(hasLevel, customSkin, "Has Level");

                // Apply changes to SerializedObject
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(rpgToolkitManager);
            }

            GUILayout.EndVertical();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawQuestModule()
        {
            // Quest Module
            var saveQuest = serializedObject.FindProperty("saveQuest");
            var loadQuest = serializedObject.FindProperty("loadQuest");

            GUILayout.Space(RPGToolkitManagerEditor.foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showQuestModule = EditorGUILayout.Foldout(showQuestModule, "Quest Module", true, foldoutStyle);
            showQuestModule = GUILayout.Toggle(showQuestModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutBottomSpace);

            if (showQuestModule)
            {
                RPGToolkitEditorHandler.DrawProperty(saveQuest, customSkin, "Save Quests");
                RPGToolkitEditorHandler.DrawProperty(loadQuest, customSkin, "Load Quests");
            }

            GUILayout.EndVertical();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawUIColors()
        {
            // UI Colors       
            var primaryColor = serializedObject.FindProperty("primaryColor");
            var secondaryColor = serializedObject.FindProperty("secondaryColor");
            var primaryReversed = serializedObject.FindProperty("primaryReversed");
            var negativeColor = serializedObject.FindProperty("negativeColor");
            var backgroundColor = serializedObject.FindProperty("backgroundColor");

            GUILayout.Space(RPGToolkitManagerEditor.foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showColors = EditorGUILayout.Foldout(showColors, "UI Colors", true, foldoutStyle);
            showColors = GUILayout.Toggle(showColors, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutBottomSpace);

            if (showColors)
            {
                RPGToolkitEditorHandler.DrawProperty(primaryColor, customSkin, "Primary");
                RPGToolkitEditorHandler.DrawProperty(secondaryColor, customSkin, "Secondary");
                RPGToolkitEditorHandler.DrawProperty(primaryReversed, customSkin, "Primary Reversed");
                RPGToolkitEditorHandler.DrawProperty(negativeColor, customSkin, "Negative");
                RPGToolkitEditorHandler.DrawProperty(backgroundColor, customSkin, "Background");
            }

            GUILayout.EndVertical();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawUIFonts()
        {
            // UI Fonts
            var lightFont = serializedObject.FindProperty("lightFont");
            var regularFont = serializedObject.FindProperty("regularFont");
            var mediumFont = serializedObject.FindProperty("mediumFont");
            var semiBoldFont = serializedObject.FindProperty("semiBoldFont");
            var boldFont = serializedObject.FindProperty("boldFont");

            GUILayout.Space(RPGToolkitManagerEditor.foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showFonts = EditorGUILayout.Foldout(showFonts, "UI Fonts", true, foldoutStyle);
            showFonts = GUILayout.Toggle(showFonts, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutBottomSpace);

            if (showFonts)
            {
                RPGToolkitEditorHandler.DrawProperty(lightFont, customSkin, "Light");
                RPGToolkitEditorHandler.DrawProperty(regularFont, customSkin, "Regular");
                RPGToolkitEditorHandler.DrawProperty(mediumFont, customSkin, "Medium");
                RPGToolkitEditorHandler.DrawProperty(semiBoldFont, customSkin, "Semibold");
                RPGToolkitEditorHandler.DrawProperty(boldFont, customSkin, "Bold");
            }

            GUILayout.EndVertical();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawUISounds()
        {
            // UI Sounds
            var backgroundMusic = serializedObject.FindProperty("backgroundMusic");
            var hoverSound = serializedObject.FindProperty("hoverSound");
            var clickSound = serializedObject.FindProperty("clickSound");

            GUILayout.Space(RPGToolkitManagerEditor.foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showSounds = EditorGUILayout.Foldout(showSounds, "UI Sounds", true, foldoutStyle);
            showSounds = GUILayout.Toggle(showSounds, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutBottomSpace);

            if (showSounds)
            {
                RPGToolkitEditorHandler.DrawProperty(backgroundMusic, customSkin, "Background Music");
                RPGToolkitEditorHandler.DrawProperty(hoverSound, customSkin, "Hover SFX");
                RPGToolkitEditorHandler.DrawProperty(clickSound, customSkin, "Click SFX");
            }
        }

        private void DrawUISettings()
        {
            //UI Settings
            GUILayout.EndVertical();
            RPGToolkitEditorHandler.DrawHeader(customSkin, "Options Header", 14);

            var enableDynamicUpdate = serializedObject.FindProperty("enableDynamicUpdate");
            enableDynamicUpdate.boolValue = RPGToolkitEditorHandler.DrawToggle(enableDynamicUpdate.boolValue, customSkin, "Update Values");

            var enableExtendedColorPicker = serializedObject.FindProperty("enableExtendedColorPicker");
            enableExtendedColorPicker.boolValue = RPGToolkitEditorHandler.DrawToggle(enableExtendedColorPicker.boolValue, customSkin, "Extended Color Picker");

            if (enableExtendedColorPicker.boolValue == true) { EditorPrefs.SetInt("RPGToolkitManager.EnableExtendedColorPicker", 1); }
            else { EditorPrefs.SetInt("RPGToolkitManager.EnableExtendedColorPicker", 0); }

            var editorHints = serializedObject.FindProperty("editorHints");

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(-3);
            editorHints.boolValue = RPGToolkitEditorHandler.DrawTogglePlain(editorHints.boolValue, customSkin, "UI Manager Hints");
            GUILayout.Space(3);

            if (editorHints.boolValue == true)
            {
                EditorGUILayout.HelpBox("These values are universal and affect all objects containing 'UI Manager' component.", MessageType.Info);
                EditorGUILayout.HelpBox("If want to assign unique values, remove 'UI Manager' component from the object ", MessageType.Info);
            }

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        private void DrawSupport()
        {
            // Support
            RPGToolkitEditorHandler.DrawHeader(customSkin, "Support Header", 14);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Need help? Contact me via :", customSkin.FindStyle("Text"));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("E-mail", customSkin.button)) { Email(); }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("ID : " + RPGToolkitManagerEditor.buildID);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
        }

        void Email() { Application.OpenURL("https://astralayano.wixsite.com/portfolio/contact"); }
    }
}
