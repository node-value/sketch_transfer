using System;
using UnityEngine;


[Serializable]
public class SketchData {
    public Vector3    Position;
    public Quaternion Rotation;
    public Vector3    Scale;
    public byte[]     Texture;

    public SketchData(Vector3 position, Quaternion rotation, Vector3 scale, Texture texture) {
        this.Position = position; 
        this.Rotation = rotation; 
        this.Scale    = scale; 
        this.Texture  = ((Texture2D) texture).EncodeToPNG();
    }

}

