using UnityEngine;
using UnityEditor;

namespace RPGToolkit
{
    public class RPGToolkitManagerWindow : EditorWindow
    {
        GUISkin customSkin;
        
        private bool showPlayerModule = false;
        protected bool showLevelSettings = false;
        protected bool showHealthSettings = false;
        protected bool showManaSettings = false;
        protected bool showStaminaSettings = false;

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
            var hasLevel = serializedObject.FindProperty("hasLevel");
            var hasHealth = serializedObject.FindProperty("hasHealth");
            var hasMana = serializedObject.FindProperty("hasMana");
            var hasStamina = serializedObject.FindProperty("hasStamina");

            GUILayout.Space(RPGToolkitManagerEditor.foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showPlayerModule = EditorGUILayout.Foldout(showPlayerModule, "Player Module", true, foldoutStyle);
            showPlayerModule = GUILayout.Toggle(showPlayerModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutBottomSpace);

            if (showPlayerModule)
            {
                RPGToolkitEditorHandler.DrawProperty(hasInventory, customSkin, "Has Inventory", "Enable to allow Player to have Inventory.");

                RPGToolkitEditorHandler.DrawProperty(hasLevel, customSkin, "Has Level", "Enable to allow Player to have Level.");
                showLevelSettings = hasLevel.boolValue;
                if (showLevelSettings)
                {
                    EditorGUI.indentLevel = 1;
                    var startingLevel = serializedObject.FindProperty("startingLevel");
                    var startingExperience = serializedObject.FindProperty("startingExperience");
                    var maxExperience = serializedObject.FindProperty("maxExperience");
                    RPGToolkitEditorHandler.DrawPropertyCW(startingLevel, customSkin, "Starting Level", "The Level the Player starts at.", 145);
                    RPGToolkitEditorHandler.DrawPropertyCW(startingExperience, customSkin, "Starting Experience", "The starting Experience int for the Player.", 145);
                    RPGToolkitEditorHandler.DrawPropertyCW(maxExperience, customSkin, "Max Experience", "The threshold of Experience to hit for level up.", 145);
                    EditorGUI.indentLevel = 0;
                }

                RPGToolkitEditorHandler.DrawProperty(hasHealth, customSkin, "Has Health", "Enable to allow Player to have Health.");
                showHealthSettings = hasHealth.boolValue;
                if (showHealthSettings)
                {
                    EditorGUI.indentLevel = 1;
                    var maxHealth = serializedObject.FindProperty("maxHealth");
                    RPGToolkitEditorHandler.DrawPropertyCW(maxHealth, customSkin, "Max Health", "Maximum Health point of the Player.", 145);
                    EditorGUI.indentLevel = 0;
                }

                RPGToolkitEditorHandler.DrawProperty(hasMana, customSkin, "Has Mana", "Enable to allow Player to have Mana.");
                showManaSettings = hasMana.boolValue;
                if (showManaSettings)
                {
                    EditorGUI.indentLevel = 1;
                    var maxMana = serializedObject.FindProperty("maxMana");
                    RPGToolkitEditorHandler.DrawPropertyCW(maxMana, customSkin, "Max Mana", "Maximum Mana point of the Player.", 145);
                    EditorGUI.indentLevel = 0;
                }

                RPGToolkitEditorHandler.DrawProperty(hasStamina, customSkin, "Has Stamina", "Enable to allow Player to have Stamina.");
                showStaminaSettings = hasStamina.boolValue;
                if (showStaminaSettings)
                {
                    EditorGUI.indentLevel = 1;
                    var maxStamina = serializedObject.FindProperty("maxStamina");
                    RPGToolkitEditorHandler.DrawPropertyCW(maxStamina, customSkin, "Max Stamina", "Maximum Stamina point of the Player.", 145);
                    EditorGUI.indentLevel = 0;
                }
            }

            GUILayout.EndVertical();
            GUILayout.Space(RPGToolkitManagerEditor.foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawQuestModule()
        {
            // Quest Module
            var hasQuestTrackUI = serializedObject.FindProperty("hasQuestTrackUI");
            var hasQuestBookUI = serializedObject.FindProperty("hasQuestBookUI");
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
                RPGToolkitEditorHandler.DrawProperty(hasQuestTrackUI, customSkin, "Have Quest Track UI", "Enable to have a Quest panel to track active quests.");
                RPGToolkitEditorHandler.DrawProperty(hasQuestBookUI, customSkin, "Have Quest Book UI", "Enable to have a Quest book to see details of all accepted quests.");
                RPGToolkitEditorHandler.DrawProperty(saveQuest, customSkin, "Save Quests", "Enable to allow quest states to be saved (Persistent Data).");
                RPGToolkitEditorHandler.DrawProperty(loadQuest, customSkin, "Load Quests", "Enable to allow quest states to be loaded when running the game.");
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
                // RPGToolkitEditorHandler.DrawProperty(primaryColor, customSkin, "Primary");
                // RPGToolkitEditorHandler.DrawProperty(secondaryColor, customSkin, "Secondary");
                // RPGToolkitEditorHandler.DrawProperty(primaryReversed, customSkin, "Primary Reversed");
                // RPGToolkitEditorHandler.DrawProperty(negativeColor, customSkin, "Negative");
                // RPGToolkitEditorHandler.DrawProperty(backgroundColor, customSkin, "Background");
                
                // Apply changes to SerializedObject
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(rpgToolkitManager);
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
                // RPGToolkitEditorHandler.DrawProperty(lightFont, customSkin, "Light");
                // RPGToolkitEditorHandler.DrawProperty(regularFont, customSkin, "Regular");
                // RPGToolkitEditorHandler.DrawProperty(mediumFont, customSkin, "Medium");
                // RPGToolkitEditorHandler.DrawProperty(semiBoldFont, customSkin, "Semibold");
                // RPGToolkitEditorHandler.DrawProperty(boldFont, customSkin, "Bold");

                // Apply changes to SerializedObject
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(rpgToolkitManager);
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
                // RPGToolkitEditorHandler.DrawProperty(backgroundMusic, customSkin, "Background Music");
                // RPGToolkitEditorHandler.DrawProperty(hoverSound, customSkin, "Hover SFX");
                // RPGToolkitEditorHandler.DrawProperty(clickSound, customSkin, "Click SFX");

                // Apply changes to SerializedObject
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(rpgToolkitManager);
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
