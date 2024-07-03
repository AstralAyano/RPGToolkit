using UnityEngine;
using UnityEditor;

namespace RPGToolkit
{
    public class RPGToolkitManagerWindow : EditorWindow
    {
        [MenuItem("RPG Toolkit/Manager Window", false, 0)]
        public static void ShowMyEditor()
        {
            EditorWindow window = GetWindow<RPGToolkitManagerWindow>();
            window.titleContent = new GUIContent("RPG Toolkit Manager");

            window.minSize = new Vector2(450, 200);
            window.maxSize = new Vector2(1920, 720);
        }

        GUISkin customSkin;
        protected static string buildID = "RPGTK-20240703";
        protected static float foldoutItemSpace = 2;
        protected static float foldoutTopSpace = 5;
        protected static float foldoutBottomSpace = 2;

        protected static bool showPlayerModule = false;
        protected static bool showQuestModule = false;
        protected static bool showColors = false;
        protected static bool showFonts = false;
        protected static bool showLogo = false;
        protected static bool showParticle = false;
        protected static bool showSounds = false;

        private RPGToolkitManager manager;
        private GUIStyle foldoutStyle;

        private void OnEnable()
        {
            manager = FindObjectOfType<RPGToolkitManager>();

            if (EditorGUIUtility.isProSkin == true)
            {
                customSkin = (GUISkin)Resources.Load("Editor\\Manager Skin Dark");
            }
            else
            {
                customSkin = (GUISkin)Resources.Load("Editor\\Manager Skin Light");
            }
        }

        private void OnGUI()
        {
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
        }

        private void DrawPlayerModule()
        {
            // Player Module
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showPlayerModule = EditorGUILayout.Foldout(showPlayerModule, "Player Module", true, foldoutStyle);
            showPlayerModule = GUILayout.Toggle(showPlayerModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showPlayerModule && manager != null)
            {
                manager.hasInventory = EditorGUILayout.Toggle("Has Inventory", manager.hasInventory);
                manager.hasQuestBook = EditorGUILayout.Toggle("Has Quest Book", manager.hasQuestBook);
                manager.hasLevel = EditorGUILayout.Toggle("Has Level", manager.hasLevel);
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawQuestModule()
        {
            // Quest Module
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showQuestModule = EditorGUILayout.Foldout(showQuestModule, "Quest Module", true, customSkin.FindStyle("UIM Foldout"));
            showQuestModule = GUILayout.Toggle(showQuestModule, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showQuestModule && manager != null)
            {
                manager.saveQuest = EditorGUILayout.Toggle("Save Quests", manager.saveQuest);
                manager.loadQuest = EditorGUILayout.Toggle("Load Quests", manager.loadQuest);
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);
        }

        private void DrawUIColors()
        {
            // UI Colors       
            
        }

        private void DrawUIFonts()
        {
            // UI Fonts
            
        }

        private void DrawUISounds()
        {
            // UI Sounds
            
        }

        private void DrawUISettings()
        {
            //UI Settings
            GUILayout.EndVertical();
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
            GUILayout.Label("ID : " + buildID);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
        }

        void Email() { Application.OpenURL("https://astralayano.wixsite.com/portfolio/contact"); }
    }
}
