using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapLoader : MonoBehaviour {

    public void LoadMapAsync()
    {
        StartCoroutine("LoadMap");
    }

	// Use this for initialization
	IEnumerator LoadMap () {

        BNOController bno = FindObjectOfType<BNOController>();

        string lat = bno.latitude;
        string lon = bno.longitude;
        string url = "http://maps.google.com/maps/api/staticmap?center=" + lat + "," + lon + "&zoom=14&size=300x300&type=hybrid&sensor=true";
        WWW www;
        www = new WWW(url);
        yield return www;
        RawImage ri = GetComponent<RawImage>();
        ri.texture = www.texture;        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
