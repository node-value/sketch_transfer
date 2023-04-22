using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class PersistanceManager {

    public static readonly string ext = ".st", dataPath = Application.persistentDataPath;

    private static void WriteToFile(ProjectData data) {
        File.WriteAllText($"{dataPath}/{data.Name}{ext}", JsonUtility.ToJson(data));      
    }

    private static ProjectData GetProjectData(Transform refObj) {
        ProjectData data = new((string)GlobalParams.Map["projectName"], (BodyType)GlobalParams.Map["bodyType"]);
        for (int i = 0; i < refObj.childCount; i++) {
            Transform child = refObj.GetChild(i);
            data.SketchDataList.Add(
                new SketchData(child.position, child.rotation, child.localScale,
                    child.GetComponent<DecalProjector>().material.GetTexture("Base_Map")));
        } return data;
    }

    public static string GetProjectDataRaw(Transform refObj) {
        return JsonUtility.ToJson(GetProjectData(refObj));
    }

    public static void Save(Transform refObj) {
         WriteToFile(GetProjectData(refObj));
    }

    public static string[] GetFileNames() {
        return Directory.GetFiles(dataPath);
    }

    private static ProjectData ReadFromFile(string projectName) {
        string path = $"{dataPath}/{projectName}{ext}";
        return File.Exists(path) ? JsonUtility.FromJson<ProjectData>(File.ReadAllText(path)) : null; 
    }

    private static Texture2D CreateTexture(byte[] data) { 
        Texture2D texture = new(2,2); 
        texture.LoadImage(data);
        return texture;
    }
    private static Texture2D ResizeTexture(Texture2D texture) {
        Texture2D result = new(texture.width / 4, texture.height / 4);
        Graphics.ConvertTexture(texture, result);
        return result;
    }

    private static void CreateProjector(SketchData data, Transform refObj, GameObject prefab) {
        DecalProjector projector = Object.Instantiate(prefab, data.Position, data.Rotation, refObj).GetComponent<DecalProjector>();
        projector.material = Material.Instantiate(projector.material);
        Texture2D texture = CreateTexture(data.Texture);
        projector.material.SetTexture("Base_Map", texture);
        projector.gameObject.GetComponent<TextureHolder>().texture = ResizeTexture(texture); 
        projector.size = data.Scale;
    }

    public static void SetProjectData(string dataRaw, Transform refObj, GameObject prefab) {
        ProjectData data = JsonUtility.FromJson<ProjectData>(dataRaw);
        GlobalParams.Map.Add("bodyType", data.BodyType);
        data.SketchDataList.ForEach(sketchData => CreateProjector(sketchData, refObj, prefab));
    }

    public static void Load(Transform refObj, GameObject prefab, string projectName) {
        ProjectData data = ReadFromFile(projectName);
        if (data != null) {
            GlobalParams.Map.Add("bodyType", data.BodyType);
            data.SketchDataList.ForEach(sketchData => CreateProjector(sketchData, refObj, prefab) );
        } else Debug.Log("Loading failed");        
    }
}

