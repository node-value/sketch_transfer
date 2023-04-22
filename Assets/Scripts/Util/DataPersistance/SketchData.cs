using System;
using UnityEngine;

[Serializable]
public class SketchData {
    public Vector3    Position;
    public Vector3    Scale;
    public Quaternion Rotation;
    public byte[]     Texture;

    public SketchData(Vector3 position, Quaternion rotation, Vector3 scale, Texture texture) {
        this.Position = position; 
        this.Rotation = rotation; 
        this.Scale    = scale; 
        this.Texture  = texture == null ? null : ((Texture2D) texture).EncodeToPNG();
    }

}

