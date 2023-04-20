using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FadeState {
    IN, OUT, NONE
}

public class FadeController : MonoBehaviour {

    public float fadeSpeed = 0.5f;

    private Material material;
    private FadeState state = FadeState.NONE;

    void Start() {
        material = GetComponent<Renderer>().material;
        Debug.Log(material == null);
    }
    void Update() {
        FadeOut();
        FadeIn();
    }

    public void SetState(FadeState state) { this.state = state; }

    void FadeOut() { 
        if (state == FadeState.OUT) {
            Color color = material.color;
            float alpha = color.a - (fadeSpeed * Time.deltaTime);
            Debug.Log(alpha);
            color = new Color(color.r, color.g, color.b, alpha);
            material.color = color;
            if (color.a <= 0) { 
                state= FadeState.NONE;
                //gameObject.SetActive(false); 
            }
            
        }
    }
    void FadeIn() { 
        if (state == FadeState.IN) {
            Color color = material.color;
            float alpha = color.a;
            alpha += (fadeSpeed * Time.deltaTime);
            alpha = Mathf.Clamp01(alpha);

            material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

            if (alpha >= 1) {
                state = FadeState.NONE;
            }
            
        }
    }
}
