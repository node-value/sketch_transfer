using System;
using UnityEngine;


[Serializable]
public class SketchData {
    private Vector3    Position { get; }
    private Quaternion Rotation { get; }
    private Vector3    Scale    { get; }
    private Texture2D  Texture  { get; }

    public SketchData(Vector3 position, Quaternion rotation, Vector3 scale, Texture2D texture) {
        this.Position = position; this.Rotation = rotation; this.Scale = scale; this.Texture = texture;
    }
}

