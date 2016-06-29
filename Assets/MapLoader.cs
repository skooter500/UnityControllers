using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ZenFulcrum.EmbeddedBrowser;

public class MapLoader : MonoBehaviour {

    public void LoadMapAsync(string param = "")
    {
        //StartCoroutine("LoadMap");
        Browser browser = GetComponent<Browser>();

        BNOController bno = FindObjectOfType<BNOController>();
                string url = "https://maps.google.com/maps?q=" + bno.latitude + "," + bno.longitude
            + "(DN_13206)&iwloc=A&hl=en&output=embed&" + param;

        //browser.Url = url;
        //string html = "<html><body>Hello</body></html>";

        string html = "<blah><head><style type=\"text/css\">body {scrolling:no;}iframe {position:absolute;z-index:1;   top:0px;    left:0px;}</style></head><body><iframe src=\"" + url +  "\" height=\"100%\" width=\"100%\" frameborder=\"0\"></iframe></body>";

        browser.LoadHTML(html);
        Debug.Log("Setting browser to: " + url);
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
