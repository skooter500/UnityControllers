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

        string[] parts = lat.Split('+');
        string dec = ("" + (float.Parse(parts[1]) / 60.0f)).Substring(2);
        lat = parts[0] + "." + dec;

        parts = lon.Split('+');
        dec = ("" + (float.Parse(parts[1]) / 60.0f)).Substring(2);
        lon = parts[0] + "." + dec;

        string url = "http://maps.google.com/maps/api/staticmap?center=" + lat + "," + lon + "&zoom=20&size=300x300&type=hybrid&sensor=true";

        //string url = "http://maps.google.com/maps?q=" + lat + "," + lon + "(Your+I2C+GPS+Stick+is+located+here)&iwloc=A&hl=en";
        
        Debug.Log(url);

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
