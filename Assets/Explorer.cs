using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Explorer : MonoBehaviour
{
	public Material mat;
	public GameObject textGo;
	public Text text;
	public Vector2 pos;
	public float zoom, angle;

	private Vector2 smoothPos;
	private float smoothScale, smoothAngle;

    private void UpdateShader() {

		float scale = 1 / zoom;

		smoothPos = Vector2.Lerp(smoothPos, pos, 0.03f);
		smoothScale = Mathf.Lerp(smoothScale, scale, 0.03f);
		smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.03f);

		float aspect = (float)Screen.width / Screen.height;
		float scalex = smoothScale;
		float scaley = smoothScale;

		if(aspect>1f) {
			scaley /= aspect;
			text.text = string.Format("x {0}", scale);
		}
		else {
			scalex *= aspect;
			text.text = string.Format("x {0}", scale);
		}


		mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scalex, scaley));
		mat.SetFloat("_Angle", smoothAngle);
	}

	private void HandleInputs()
    {
        if(Input.GetKey(KeyCode.KeypadPlus)) {
			zoom *= 1.01f;
		}
		if(Input.GetKey(KeyCode.KeypadMinus)) {
			zoom *= 0.99f;
		}
		if(Input.GetKey(KeyCode.Q)) {
			angle -= 1f;
		}
		if(Input.GetKey(KeyCode.E)) {
			angle += 1f;
		}


		Vector2 dir = new Vector2(0.01f / zoom, 0);
		float s = Mathf.Sin(angle/180f*3.1415f);
		float c = Mathf.Cos(angle/180f*3.1415f);

		dir = new Vector2(dir.x*c-dir.y*s, dir.x*s + dir.y*c);
		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow )) {
			pos -= dir;
		}
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow )) {
			pos += dir;
		}


		dir = new Vector2(-dir.y, dir.x);
		if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow )) {
			pos -= dir;
		}
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow )) {
			pos += dir;
		}
		
    }

    private void Start()
    {
		textGo = GameObject.Find("ZoomText");
		text = textGo.GetComponent<Text>();

		Vector4 area = mat.GetVector("_Area");

	}

    // Update is called once per frame
    void FixedUpdate () {
		HandleInputs();

		UpdateShader();
	}

}
