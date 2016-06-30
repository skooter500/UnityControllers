using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ZenFulcrum.EmbeddedBrowser;

public class MapLoader : MonoBehaviour {

    public void LoadDefaultMap()
    {
        //StartCoroutine("LoadMap");
        Browser browser = GetComponent<Browser>();

        string lat = "53+20 40.55";
        string lon = "-6+16 2.05";

        BNOController bno = FindObjectOfType<BNOController>();
        string url = "https://maps.google.com/maps?q=" + lat + "," + lon
            + "(DN_13206)&iwloc=A&hl=en&output=embed";

        //browser.Url = url;
        //string html = "<html><body>Hello</body></html>";

        string html = "<blah><head><style type=\"text/css\">body {scrolling:no;}iframe {position:absolute;z-index:1;   top:0px;    left:0px;}</style></head><body><iframe src=\"" + url + "\" height=\"100%\" width=\"100%\" frameborder=\"0\"></iframe></body>";

        browser.LoadHTML(html);
        Debug.Log(url);
    }



    public void LoadMap(string param = "")
    {
        Browser browser = GetComponent<Browser>();

        BNOController bno = FindObjectOfType<BNOController>();
                string url = "https://maps.google.com/maps?q=" + bno.latitude + "," + bno.longitude
            + "(DN_13206)&iwloc=A&hl=en&output=embed";

        string html = "<blah><head><style type=\"text/css\">body {scrolling:no;}iframe {position:absolute;z-index:1;   top:0px;    left:0px;}</style></head><body><iframe src=\"" + url +  "\" height=\"100%\" width=\"100%\" frameborder=\"0\"></iframe></body>";

        browser.LoadHTML(html);
        Debug.Log(url);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
