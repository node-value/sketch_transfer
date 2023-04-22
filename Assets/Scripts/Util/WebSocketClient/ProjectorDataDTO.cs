using System;
using UnityEngine;

[Serializable]
public class ProjectorDataDTO {
    public int Index;
    public Vector3 Position;
    public Vector3 Scale;
    public Quaternion Rotation;

    public ProjectorDataDTO(int index, Vector3 positon, Vector3 scale, Quaternion rotation) {
        this.Index = index; this.Rotation=rotation; this.Position = positon; this.Scale = scale;
    }
}

