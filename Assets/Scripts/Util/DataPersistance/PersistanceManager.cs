using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class PersistanceManager {

    public static readonly string ext = ".st", dataPath = Application.persistentDataPath;

    private static void WriteToFile(ProjectData data) {
        File.WriteAllText($"{dataPath}/{data.Name}{ext}", JsonUtility.ToJson(data));
    }

    public static void Save(Transform refObj) {
        ProjectData data = new((string) GlobalParams.Map["projectName"], (BodyType)GlobalParams.Map["bodyType"]);
        for (int i = 0; i < refObj.childCount; i++ ) {
            Transform child = refObj.GetChild(i);
            data.SketchDataList.Add(
                new SketchData(child.position, child.rotation, child.localScale, 
                    child.GetComponent<DecalProjector>().material.GetTexture("Base_Map")));
        } WriteToFile(data);
    }

    public static string[] GetFileNames() {
        return Directory.GetFiles(dataPath);
    }

    private static ProjectData ReadFromFile(string projectName) {
        string path = $"{dataPath}/{projectName}{ext}";
        return File.Exists(path) ? JsonUtility.FromJson<ProjectData>(File.ReadAllText(path)) : null; 
    }

    private static Texture2D CreateTexture(byte[] data) { 
        Texture2D texture = new(2,2); texture.LoadImage(data);
        return texture;
    }

    private static void CreateProjector(SketchData data, Transform refObj, GameObject prefab) {
        DecalProjector projector = Object.Instantiate(prefab, data.Position, data.Rotation, refObj).GetComponent<DecalProjector>();
        projector.material = Material.Instantiate(projector.material);
        projector.material.SetTexture("Base_Map", CreateTexture(data.Texture));
        projector.size = data.Scale;
    }
    public static void Load(Transform refObj, GameObject prefab, string projectName) {
        ProjectData data = ReadFromFile(projectName);
        if (data != null) {
            GlobalParams.Map.Add("bodyType", data.BodyType);
            data.SketchDataList.ForEach(sketchData => CreateProjector(sketchData, refObj, prefab) );
        } else Debug.Log("Loading failed");        
    }
}

