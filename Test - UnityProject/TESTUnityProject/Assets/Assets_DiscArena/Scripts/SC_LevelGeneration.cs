using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SC_LevelGeneration : MonoBehaviour
{
#if UNITY_EDITOR
    public class LevelGenerationWindow : EditorWindow
    {
        string fileName = "FileName";
        string creatorName = "Creator";

        [MenuItem("Window/LevelGeneration")]
        public static void ShowWindow()
        {
            GetWindow<LevelGenerationWindow>("LevelGeneration");
        }

        private void OnGUI()
        {
            GUILayout.Space(20);
            fileName = EditorGUILayout.TextField("File name : ", fileName);

            creatorName = EditorGUILayout.TextField("Creator name : ", creatorName);

            if (GUILayout.Button("Export Level"))
            {
                ExportLevel(fileName, creatorName);
            }

            if (GUILayout.Button("Load Level"))
            {
                LoadLevel(fileName);
            }
        }
    }
#endif

    [System.Serializable]
    public class LevelData
    {
        public List<SC_BuildingMaster.BuildingSave> buildingList = new List<SC_BuildingMaster.BuildingSave>();
        public int levelDiscCount = 0;
        public string creatorName = "Creator";
    }

    static public void ExportLevel(string pFileName,string pCreatorName)
    {
        LevelData data = new LevelData();

        // Save all building
        var BuildingList = FindObjectsOfType<SC_BuildingMaster>();

        for (int i = 0; i < BuildingList.Length; i++)
        {
            SC_BuildingMaster.BuildingSave newBuilding = new SC_BuildingMaster.BuildingSave();
            newBuilding.key = BuildingList[i].Key;
            newBuilding.position = BuildingList[i].transform.parent.transform.position;
            newBuilding.rotation = BuildingList[i].transform.parent.transform.rotation;
            newBuilding.scale = BuildingList[i].transform.parent.transform.localScale;
            newBuilding.health = BuildingList[i].MaxHealth;
            data.buildingList.Add(newBuilding);
        }

        // Save disc count
        data.levelDiscCount = FindObjectOfType<SC_GameManager>().DiscCount;
        data.creatorName = pCreatorName;

        // Generate Json
        string json = JsonUtility.ToJson(data);
        string destination = "Assets/StreamingAssets/" + pFileName + ".json";

        if (File.Exists(destination) == false)
        {
            FileStream file = File.Create(destination);
            file.Close();
        }

        File.WriteAllText(destination, json);

        Debug.Log("Export Succeed");
    }

    static public void LoadLevel(string fileName)
    {
        SC_BuildingList buildingList = FindObjectOfType<SC_BuildingList>();

#if UNITY_EDITOR
        string destination = "Assets/Resources/" + fileName + ".json";
        string json = File.ReadAllText(destination);
#elif UNITY_IOS
         string destination = Application.streamingAssetsPath + "/" + fileName+ ".json";
        string json = File.ReadAllText(destination);
#elif UNITY_ANDROID
        string destination = "jar:file://" + Application.dataPath + "!/assets/" + fileName + ".json";
        WWW wwwfile = new WWW(destination);
        while (!wwwfile.isDone) { }
        string filePath = Application.persistentDataPath + "/" + fileName + ".json";
        File.WriteAllBytes(filePath, wwwfile.bytes);

        StreamReader wr = new StreamReader(filePath);        
        string json = wr.ReadToEnd();
#endif

        LevelData data = JsonUtility.FromJson<LevelData>(json);

        // Generate building
        for (int i = 0; i < data.buildingList.Count; i++)
        {
            GameObject go = Instantiate(buildingList.GetPrefabs(data.buildingList[i].key));
            go.transform.position = data.buildingList[i].position;
            go.transform.rotation = data.buildingList[i].rotation;
            go.transform.localScale = data.buildingList[i].scale;
            go.GetComponentInChildren<SC_BuildingMaster>().LoadSave(data.buildingList[i]);
        }

        // Load Game Manager
        FindObjectOfType<SC_GameManager>().LoadLevel(data);
    }
}
