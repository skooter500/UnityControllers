using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        string lat = "56";
        string lon = "-6";
        //string url = "http://maps.google.com/maps/api/staticmap?center=" + lat + "," + lon + "&zoom=14&size=800x600&type=hybrid&sensor=true";
        string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
        WWW www;
        www = new WWW(url);
        yield return www;
        GetComponent<Renderer>().material.mainTexture = www.texture;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
