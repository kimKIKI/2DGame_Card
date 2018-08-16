using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace BurgZergArcade.ItemSystem.Editor
{
    public class ISQualityDatabaseEditor : EditorWindow
    {
        ISQualityDatabase qualityDatabase;
        ISQuality selectedItem;
        Texture2D selectedtexture;

        const int SPRITE_BUTTON_SIZE = 46;
        const string DATABASE_FILE_NAME = @"bzaQualityDatabase.asset";
        const string DATABASE_FOLDER_NAME = @"Database";
        const string DATABASE_Full_PATH = @"Assets/" + DATABASE_FOLDER_NAME + "/" + DATABASE_FILE_NAME;

        [MenuItem("BZA/Database/Quality Editor %#i")]
        static public void Init()
        {
            ISQualityDatabaseEditor window = EditorWindow.GetWindow<ISQualityDatabaseEditor>();
            window.minSize = new Vector2(400,300);
            window.title = "Quality Database";
            window.Show();
        }

         void OnEnable()
        {
            qualityDatabase = AssetDatabase.LoadAssetAtPath(DATABASE_Full_PATH,typeof(ISQualityDatabase)) as ISQualityDatabase;

            if (qualityDatabase == null)
            {
                if (!AssetDatabase.IsValidFolder("Assets/" + DATABASE_FOLDER_NAME))
                {
                    AssetDatabase.CreateFolder("Assets",DATABASE_FOLDER_NAME);
                }

                qualityDatabase = ScriptableObject.CreateInstance<ISQualityDatabase>();
                    AssetDatabase.CreateAsset(qualityDatabase, DATABASE_Full_PATH);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                
             
            }
            selectedItem = new ISQuality();
        }

        private void OnGUI()
        {
            GUILayout.Label("This  is  Label");
            AddQualityToDatabase();
        }

        void AddQualityToDatabase()
        {
            //name
            //Sprite
            selectedItem.Name = EditorGUILayout.TextField("Name:",selectedItem.Name);
            if (selectedItem.ICon)
                selectedtexture = selectedItem.ICon.texture;
            else
                selectedtexture = null;

             //에디터의 버튼 사이즈 조절 ,버튼이 있을때
            if (GUILayout.Button(selectedtexture, GUILayout.Width(SPRITE_BUTTON_SIZE), GUILayout.Height(SPRITE_BUTTON_SIZE)))
            {
                //Sprite 이미지 컨트롤 박스가 전부 뜬다.
                int controlerID = EditorGUIUtility.GetControlID(FocusType.Passive);
                EditorGUIUtility.ShowObjectPicker<Sprite>(null, true, null, controlerID);
            }

            string commandName = Event.current.commandName;
            if (commandName == "ObjectSelectorUpdated")
            {
                //버튼을 클릭시 픽커의 이미지가 버튼에 그려지게 한다.
                //image타입이 Texture이어야함 Sprite Img타입은 적용안됨
                selectedItem.ICon = (Sprite)EditorGUIUtility.GetObjectPickerObject();
                Repaint();
            }

            if (GUILayout.Button("Save"))
            {
                if (selectedItem == null)
                    return;

                qualityDatabase.Add(selectedItem);
                //qualityDatabase.database.Add(selectedItem);
                selectedItem = new ISQuality();
            }
            
        } 
    }
}
