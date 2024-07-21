using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace RPGToolkit
{
    [CustomEditor(typeof(RPGToolkitManager))]
    [System.Serializable]
    public class RPGToolkitManagerEditor : Editor
    {
        GUISkin customSkin;
        public static string buildID = "RPGTK-20240703";
        public static float foldoutItemSpace = 2;
        public static float foldoutTopSpace = 5;
        public static float foldoutBottomSpace = 2;

        public GameObject uiCanvas;
        public GameObject inventoryUI, questUI, healthUI, manaUI;
        public GameObject playerModule, inventoryModule, questModule;

        private bool allowUI;
        private bool allowPlayerModule, allowInventoryModule, allowQuestModule;

        protected static bool showKeybinds = false;
        protected static bool showPlayerModule = false;
        protected static bool showLevelSettings = false;
        protected static bool showHealthSettings = false;
        protected static bool showManaSettings = false;
        protected static bool showStaminaSettings = false;
        protected static bool showDashSettings = false;
        protected static bool showWallJumpSettings = false;
        protected static bool showInventoryModule = false;
        protected static bool showQuestModule = false;
        protected static bool showNPCModule = false;
        protected static bool showColors = false;
        protected static bool showFonts = false;
        protected static bool showLogo = false;
        protected static bool showParticle = false;
        protected static bool showSounds = false;

        private GUIStyle foldoutStyle;

        private void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true)
            {
                customSkin = (GUISkin)Resources.Load("Editor\\Manager Skin Dark");
            }
            else
            {
                customSkin = (GUISkin)Resources.Load("Editor\\Manager Skin Light");
            }

            Selection.activeObject = target;
        }

        public override void OnInspectorGUI()
        {
            if (serializedObject == null)
            {
                return;
            }
            
            serializedObject.Update();

            if (customSkin == null)
            {
                EditorGUILayout.HelpBox("Editor variables are missing. You can manually fix this by deleting " +
                    "RPGToolkit > Resources folder and then re-import the package. \n\nIf you're still seeing this " +
                    "dialog even after the re-import, contact me with this ID: " + buildID, MessageType.Error);

                if (GUILayout.Button("Contact"))
                {
                    Email();
                }

                return;
            }

            uiCanvas = GameObject.FindWithTag("RPGToolkitUI");
            playerModule = GameObject.FindWithTag("RPGToolkitPlayer");
            inventoryModule = GameObject.FindWithTag("RPGToolkitInventory");
            questModule = GameObject.FindWithTag("RPGToolkitQuest");

            allowUI = uiCanvas == null ? true : false;
            allowPlayerModule = playerModule == null ? true : false;
            allowInventoryModule = inventoryModule == null ? true : false;
            allowQuestModule = questModule == null ? true : false;

            // Foldout style
            foldoutStyle = customSkin.FindStyle("UIM Foldout");

            // UIM Header
            RPGToolkitEditorHandler.DrawHeader(customSkin, "UIM Header", 8);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Draw Modules and Creations
            DrawModules();

            // Draw GUIs
            DrawKeybinds();

            DrawPlayerModule();
            DrawInventoryModule();
            DrawQuestModule();
            DrawNPCModule();

            DrawUIColors();
            DrawUIFonts();
            DrawUISounds();
            DrawUISettings();

            DrawSupport();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawModules()
        {
            // All Modules and creations
            GUI.enabled = true;

            // RPG Toolkit UI Canvas
            if (allowUI)
            {
                if (GUILayout.Button("Create RPG Toolkit UI Canvas", customSkin.button))
                {
                    uiCanvas = RPGToolkitModules.CreateUI();
                }
            }
            else
            {
                if (GUILayout.Button("Find RPG Toolkit UI Canvas", customSkin.button))
                {
                    Selection.activeObject = uiCanvas;
                    EditorGUIUtility.PingObject(uiCanvas);
                }
            }

            // Player Module
            if (allowPlayerModule)
            {
                if (GUILayout.Button("Create Player Module", customSkin.button))
                {
                    playerModule = RPGToolkitModules.CreatePlayer();
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Find Player Module", customSkin.button))
                {
                    Selection.activeObject = playerModule;
                    EditorGUIUtility.PingObject(playerModule);
                }
                if (GUILayout.Button("Delete Player Module", customSkin.button))
                {
                    if (EditorUtility.DisplayDialog("Delete Player Module?", "This will delete the Player GameObject. Are you sure?", "Delete", "Cancel"))
                    {
                        DestroyImmediate(GameObject.FindWithTag("RPGToolkitPlayer"));
                    }
                }
                GUILayout.EndHorizontal();
            }

            // Inventory Module
            if (allowInventoryModule)
            {
                if (GUILayout.Button("Create Inventory Module", customSkin.button))
                {
                    inventoryModule = RPGToolkitModules.CreateInventory();
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Find Inventory Module", customSkin.button))
                {
                    Selection.activeObject = inventoryModule;
                    EditorGUIUtility.PingObject(inventoryModule);
                }
                if (GUILayout.Button("Delete Inventory Module", customSkin.button))
                {
                    if (EditorUtility.DisplayDialog("Delete Inventory Module?", "This will delete the Inventory GameObject as well as the UI for it. Are you sure?", "Delete", "Cancel"))
                    {
                        DestroyImmediate(GameObject.FindWithTag("RPGToolkitInventory"));
                        DestroyImmediate(GameObject.FindWithTag("RPGToolkitInventoryUI"));
                    }
                }
                GUILayout.EndHorizontal();
            }

            // Quest Module
            if (allowQuestModule)
            {
                if (GUILayout.Button("Create Quest Module", customSkin.button))
                {
                    questModule = RPGToolkitModules.CreateQuest();
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Find Quest Module", customSkin.button))
                {
                    Selection.activeObject = questModule;
                    EditorGUIUtility.PingObject(questModule);
                }
                if (GUILayout.Button("Delete Quest Module", customSkin.button))
                {
                    if (EditorUtility.DisplayDialog("Delete Quest Module?", "This will delete the Quest GameObject as well as the UI for it. Are you sure?", "Delete", "Cancel"))
                    {
                        DestroyImmediate(GameObject.FindWithTag("RPGToolkitQuest"));
                        DestroyImmediate(GameObject.FindWithTag("RPGToolkitQuestUI"));
                    }
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawKeybinds()
        {
            // Keybinds
            GUI.enabled = true;

            var interactKey = serializedObject.FindProperty("interactKey");
            var questKey = serializedObject.FindProperty("questBookKey");

            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showKeybinds = EditorGUILayout.Foldout(showKeybinds, "Keybinds", true, foldoutStyle);
            showKeybinds = GUILayout.Toggle(showKeybinds, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showKeybinds)
            {
                RPGToolkitEditorHandler.DrawProperty(interactKey, customSkin, "Interection Key", "Key to interact with NPCs or the world.");
                RPGToolkitEditorHandler.DrawProperty(questKey, customSkin, "Quest Book Key", "Key to open the Quest Book.");
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawPlayerModule()
        {
            // Player Module
            GUI.enabled = allowPlayerModule ? false : true;

            var hasInventory = serializedObject.FindProperty("hasInventory");
            var hasLevel = serializedObject.FindProperty("hasLevel");
            var hasHealth = serializedObject.FindProperty("hasHealth");
            var hasMana = serializedObject.FindProperty("hasMana");
            var hasStamina = serializedObject.FindProperty("hasStamina");
            var hasDash = serializedObject.FindProperty("hasDash");
            var hasWallJump = serializedObject.FindProperty("hasWallJump");

            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showPlayerModule = EditorGUILayout.Foldout(showPlayerModule, "Player Module", true, foldoutStyle);
            showPlayerModule = GUILayout.Toggle(showPlayerModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showPlayerModule)
            {
                RPGToolkitEditorHandler.DrawProperty(hasInventory, customSkin, "Has Inventory", "Enable to allow Player to have Inventory.");

                // Level
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

               // Health
                RPGToolkitEditorHandler.DrawProperty(hasHealth, customSkin, "Has Health", "Enable to allow Player to have Health.");
                showHealthSettings = hasHealth.boolValue;
                if (showHealthSettings)
                {
                    EditorGUI.indentLevel = 1;
                    var hasHealthUI = serializedObject.FindProperty("hasHealthUI");
                    var maxHealth = serializedObject.FindProperty("maxHealth");
                    RPGToolkitEditorHandler.DrawPropertyCW(hasHealthUI, customSkin, "Has Health UI", "Enable to allow Player to have Health Bar UI.", 145);
                    
                    EditorGUI.BeginChangeCheck();
                    RPGToolkitEditorHandler.DrawPropertyCW(maxHealth, customSkin, "Max Health", "Maximum Health point of the Player.", 145);
                    bool maxHealthChanged = EditorGUI.EndChangeCheck();

                    EditorGUI.indentLevel = 0;
                    
                    if (hasHealthUI.boolValue)
                    {
                        healthUI = RPGToolkitModules.CreateHealthUI();
                        
                        if (healthUI != null)
                        {
                            healthUI.GetComponentInChildren<Slider>().maxValue = maxHealth.floatValue;
                            healthUI.GetComponentInChildren<Slider>().value = healthUI.GetComponentInChildren<Slider>().maxValue;
                        }
                    }
                    else
                    {
                        healthUI = GameObject.FindWithTag("RPGToolkitHealthUI");
                        if (healthUI != null)
                        {
                            DestroyImmediate(healthUI);
                        }
                    }

                    if (maxHealthChanged)
                    {
                        if (hasHealthUI.boolValue)
                        {
                            healthUI = GameObject.FindWithTag("RPGToolkitHealthUI");
                            healthUI.GetComponentInChildren<Slider>().maxValue = maxHealth.floatValue;
                            healthUI.GetComponentInChildren<Slider>().value = healthUI.GetComponentInChildren<Slider>().maxValue;
                        }
                    }
                }

                // Mana
                RPGToolkitEditorHandler.DrawProperty(hasMana, customSkin, "Has Mana", "Enable to allow Player to have Mana.");
                showManaSettings = hasMana.boolValue;
                if (showManaSettings)
                {
                    EditorGUI.indentLevel = 1;
                    var hasManaUI = serializedObject.FindProperty("hasManaUI");
                    var maxMana = serializedObject.FindProperty("maxMana");
                    RPGToolkitEditorHandler.DrawPropertyCW(hasManaUI, customSkin, "Has Mana UI", "Enable to allow Player to have Mana Bar UI.", 145);
                    
                    EditorGUI.BeginChangeCheck();
                    RPGToolkitEditorHandler.DrawPropertyCW(maxMana, customSkin, "Max Mana", "Maximum Mana point of the Player.", 145);
                    bool maxManaChanged = EditorGUI.EndChangeCheck();
                    
                    EditorGUI.indentLevel = 0;

                    if (hasManaUI.boolValue)
                    {
                        manaUI = RPGToolkitModules.CreateManaUI();
                        
                        if (manaUI != null)
                        {
                            manaUI.GetComponentInChildren<Slider>().maxValue = maxMana.floatValue;
                            manaUI.GetComponentInChildren<Slider>().value = manaUI.GetComponentInChildren<Slider>().maxValue;
                        }
                    }
                    else
                    {
                        manaUI = GameObject.FindWithTag("RPGToolkitManaUI");
                        if (manaUI != null)
                        {
                            DestroyImmediate(manaUI);
                        }
                    }

                    if (maxManaChanged)
                    {
                        if (hasManaUI.boolValue)
                        {
                            manaUI = GameObject.FindWithTag("RPGToolkitManaUI");
                            manaUI.GetComponentInChildren<Slider>().maxValue = maxMana.floatValue;
                            manaUI.GetComponentInChildren<Slider>().value = manaUI.GetComponentInChildren<Slider>().maxValue;
                        }
                    }
                }

                // Stamina
                // RPGToolkitEditorHandler.DrawProperty(hasStamina, customSkin, "Has Stamina", "Enable to allow Player to have Stamina.");
                // showStaminaSettings = hasStamina.boolValue;
                // if (showStaminaSettings)
                // {
                //     EditorGUI.indentLevel = 1;
                //     var maxStamina = serializedObject.FindProperty("maxStamina");
                //     RPGToolkitEditorHandler.DrawPropertyCW(maxStamina, customSkin, "Max Stamina", "Maximum Stamina point of the Player.", 145);
                //     EditorGUI.indentLevel = 0;
                // }

                // Dash
                RPGToolkitEditorHandler.DrawProperty(hasDash, customSkin, "Has Dash", "Allows Player to have Dash ability to dodge and phase through.");
                showDashSettings = hasDash.boolValue;
                if (showDashSettings)
                {
                    EditorGUI.indentLevel = 1;
                    var phaseThroughLayers = serializedObject.FindProperty("phaseThroughLayers");
                    var dodgeableLayers = serializedObject.FindProperty("dodgeableLayers");
                    var dashPower = serializedObject.FindProperty("dashPower");
                    var dashDuration = serializedObject.FindProperty("dashDuration");
                    var dashCooldown = serializedObject.FindProperty("dashCooldown");
                    RPGToolkitEditorHandler.DrawPropertyCW(phaseThroughLayers, customSkin, "Phase Through Layers", "When using Dash ability, what layers can the Player phase.", 145);
                    RPGToolkitEditorHandler.DrawPropertyCW(dodgeableLayers, customSkin, "Dodgeable Layers", "When using Dash ability, what layers won't damage the Player.", 145);
                    RPGToolkitEditorHandler.DrawPropertyCW(dashPower, customSkin, "Dash Power", "How strong is the Dash ability.", 145);
                    RPGToolkitEditorHandler.DrawPropertyCW(dashDuration, customSkin, "Dash Duration", "How long does the Dash ability lasts.", 145);
                    RPGToolkitEditorHandler.DrawPropertyCW(dashCooldown, customSkin, "Dash Cooldown", "Intervals before the next allowed usage of the Dash ability.", 145);
                    EditorGUI.indentLevel = 0;
                }

                // Wall Jump
                RPGToolkitEditorHandler.DrawProperty(hasWallJump, customSkin, "Has Wall Jump", "Allows Player to have Wall Jump ability.");
                showWallJumpSettings = hasWallJump.boolValue;
                if (showWallJumpSettings)
                {
                    EditorGUI.indentLevel = 1;
                    var wallLayer = serializedObject.FindProperty("wallLayer");
                    var wallJumpForce = serializedObject.FindProperty("wallJumpForce");
                    var wallJumpDuration = serializedObject.FindProperty("wallJumpDuration");
                    RPGToolkitEditorHandler.DrawPropertyCW(wallLayer, customSkin, "Wall Layer", "When using Wall Jump ability, what layers can the Player jump from or to.", 145);
                    RPGToolkitEditorHandler.DrawPropertyCW(wallJumpForce, customSkin, "Wall Jump Force", "How strong is the Wall Jump ability.", 145);
                    RPGToolkitEditorHandler.DrawPropertyCW(wallJumpDuration, customSkin, "Wall Jump Duration", "How long does the Wall Jump ability lasts.", 145);
                    EditorGUI.indentLevel = 0;
                }
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawInventoryModule()
        {
            // Inventory Module
            GUI.enabled = true;
            
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showInventoryModule = EditorGUILayout.Foldout(showInventoryModule, "Inventory Module", true, foldoutStyle);
            showInventoryModule = GUILayout.Toggle(showInventoryModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showInventoryModule)
            {
                if (GUILayout.Button("Create new Item", customSkin.button))
                {
                    RPGToolkitModules.CreateNewItemSO();
                }
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawQuestModule()
        {
            // Quest Module
            GUI.enabled = allowQuestModule ? false : true;

            var hasQuestTrackUI = serializedObject.FindProperty("hasQuestTrackUI");
            var hasQuestBookUI = serializedObject.FindProperty("hasQuestBookUI");
            var saveQuest = serializedObject.FindProperty("saveQuest");
            var loadQuest = serializedObject.FindProperty("loadQuest");

            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showQuestModule = EditorGUILayout.Foldout(showQuestModule, "Quest Module", true, foldoutStyle);
            showQuestModule = GUILayout.Toggle(showQuestModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showQuestModule)
            {
                if (questModule != null)
                {
                    GUI.enabled = true;
                    if (GUILayout.Button("Create new Quest", customSkin.button))
                    {
                        RPGToolkitModules.CreateNewQuestSO();
                    }
                }
                else
                {
                    GUI.enabled = false;
                    GUILayout.Button("Create new Quest", customSkin.button);
                }

                RPGToolkitEditorHandler.DrawProperty(hasQuestTrackUI, customSkin, "Have Quest Track UI", "Enable to have a Quest panel to track active quests.");
                RPGToolkitEditorHandler.DrawProperty(hasQuestBookUI, customSkin, "Have Quest Book UI", "Enable to have a Quest book to see details of all accepted quests.");
                RPGToolkitEditorHandler.DrawProperty(saveQuest, customSkin, "Save Quests", "Enable to allow quest states to be saved (Persistent Data).");
                RPGToolkitEditorHandler.DrawProperty(loadQuest, customSkin, "Load Quests", "Enable to allow quest states to be loaded when running the game.");
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawNPCModule()
        {
            // NPC Module
            GUI.enabled = true;
            
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showNPCModule = EditorGUILayout.Foldout(showNPCModule, "NPC Module", true, foldoutStyle);
            showNPCModule = GUILayout.Toggle(showNPCModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showNPCModule)
            {
                if (GUILayout.Button("Create new NPC", customSkin.button))
                {
                    RPGToolkitModules.CreateNewNPC();
                }
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawUIColors()
        {
            // UI Colors       
            var primaryColor = serializedObject.FindProperty("uiPrimaryColor");
            var secondaryColor = serializedObject.FindProperty("uiSecondaryColor");
            var tertiaryColor = serializedObject.FindProperty("uiTertiaryColor");
            var accentColor = serializedObject.FindProperty("uiAccentColor");
            var backgroundColor = serializedObject.FindProperty("uiBackgroundColor");
            var altBackgroundColor = serializedObject.FindProperty("uiAltBackgroundColor");

            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showColors = EditorGUILayout.Foldout(showColors, "UI Colors", true, foldoutStyle);
            showColors = GUILayout.Toggle(showColors, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showColors)
            {
                RPGToolkitEditorHandler.DrawProperty(primaryColor, customSkin, "Primary", "This is the Primary color for all UI with RPGToolkitUISettings component.");
                RPGToolkitEditorHandler.DrawProperty(secondaryColor, customSkin, "Secondary", "This is the Secondary color for all UI with RPGToolkitUISettings component.");
                RPGToolkitEditorHandler.DrawProperty(tertiaryColor, customSkin, "Tertiary", "This is the Tertiary color for all UI with RPGToolkitUISettings component.");
                RPGToolkitEditorHandler.DrawProperty(accentColor, customSkin, "Accents", "This is the Accent color for all UI with RPGToolkitUISettings component.");
                RPGToolkitEditorHandler.DrawProperty(backgroundColor, customSkin, "Background", "This is the Background color for all UI with RPGToolkitUISettings component.");
                RPGToolkitEditorHandler.DrawProperty(altBackgroundColor, customSkin, "Alternate Background", "This is the alternate Background color for all UI with RPGToolkitUISettings component.");
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
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

            // UI Font Colors
            var primaryColor = serializedObject.FindProperty("fontPrimaryColor");
            var secondaryColor = serializedObject.FindProperty("fontSecondaryColor");
            var tertiaryColor = serializedObject.FindProperty("fontTertiaryColor");
            var accentColor = serializedObject.FindProperty("fontAccentColor");

            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showFonts = EditorGUILayout.Foldout(showFonts, "UI Fonts", true, foldoutStyle);
            showFonts = GUILayout.Toggle(showFonts, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showFonts)
            {
                RPGToolkitEditorHandler.DrawProperty(lightFont, customSkin, "Light", "The light weight (300) of the font family.");
                RPGToolkitEditorHandler.DrawProperty(regularFont, customSkin, "Regular", "The default weight or normal weight (400) of the font family.");
                RPGToolkitEditorHandler.DrawProperty(mediumFont, customSkin, "Medium", "The medium weight (500) of the font family.");
                RPGToolkitEditorHandler.DrawProperty(semiBoldFont, customSkin, "Semibold", "The semi bold weight (600) of the font family.");
                RPGToolkitEditorHandler.DrawProperty(boldFont, customSkin, "Bold", "The bold weight (700) of the font family.");

                GUILayout.Space(foldoutTopSpace);
                GUILayout.Space(foldoutBottomSpace);

                RPGToolkitEditorHandler.DrawProperty(primaryColor, customSkin, "Primary", "The primary color for all UI Font with RPGToolkitUISettings component.");
                RPGToolkitEditorHandler.DrawProperty(secondaryColor, customSkin, "Secondary", "The secondary color for all UI Font with RPGToolkitUISettings component.");
                RPGToolkitEditorHandler.DrawProperty(tertiaryColor, customSkin, "Tertiary", "The tertiary color for all UI Font with RPGToolkitUISettings component.");
                RPGToolkitEditorHandler.DrawProperty(accentColor, customSkin, "Accents", "The accent color for all UI Font with RPGToolkitUISettings component.");
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawUISounds()
        {
            // UI Sounds
            var backgroundMusic = serializedObject.FindProperty("backgroundMusic");
            var hoverSound = serializedObject.FindProperty("hoverSound");
            var clickSound = serializedObject.FindProperty("clickSound");

            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showSounds = EditorGUILayout.Foldout(showSounds, "UI Sounds", true, foldoutStyle);
            showSounds = GUILayout.Toggle(showSounds, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showSounds)
            {
                // RPGToolkitEditorHandler.DrawProperty(backgroundMusic, customSkin, "Background Music");
                // RPGToolkitEditorHandler.DrawProperty(hoverSound, customSkin, "Hover SFX");
                // RPGToolkitEditorHandler.DrawProperty(clickSound, customSkin, "Click SFX");
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
                EditorGUILayout.HelpBox("These values are universal and affect all objects containing 'RPG Toolkit' component.", MessageType.Info);
                EditorGUILayout.HelpBox("If want to assign unique values, remove 'RPG Toolkit' component from the object ", MessageType.Info);
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
            GUILayout.Label("Need help? Contact me via : ", customSkin.FindStyle("Text"));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("E-mail", customSkin.button))
            {
                Email();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("ID : " + buildID);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
        }

        void Email() { Application.OpenURL("https://astralayano.wixsite.com/portfolio/contact"); }
    }
}