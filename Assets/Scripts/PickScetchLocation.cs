using UnityEngine;

public class PickScetchLocation : MonoBehaviour {

    public Sprite sketchSprite;
    public Transform referenceObject;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            int layerMask = 1 << LayerMask.NameToLayer("Model");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) {
                GameObject sketchObject = new("Sketch");
                SpriteRenderer renderer = sketchObject.AddComponent<SpriteRenderer>();
                renderer.sprite = sketchSprite;
                sketchObject.transform.SetPositionAndRotation(hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal, Vector3.up));
                sketchObject.transform.localScale = Vector3.one * 0.1f;
                sketchObject.transform.parent = referenceObject;
            }
        }
    }
}