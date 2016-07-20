using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ZenFulcrum.EmbeddedBrowser;

public class MapLoader : MonoBehaviour {

    public Dropdown dropdown;

    private string MapType 
    {
    get
        {
            if (dropdown == null)
            {
                return "roadmap";
            }
            else
            {
                return dropdown.options[dropdown.value].text.ToLower();
            }
        }
    }

    public void LoadDefaultMap()
    {
        Browser browser = GetComponent<Browser>();

        string lat = "53+20 44.3";
        string lon = "-6+17 47.0";
        
        BNOController bno = FindObjectOfType<BNOController>();
        string url = "https://maps.google.com/maps?q=" + lat + "," + lon
            + "(DN_13206)&iwloc=A&hl=en&zoom=1&output=embed&maptype=" + MapType;
        /*
        https://www.google.com/maps/embed/v1/view
  ? key = YOUR_API_KEY
  & center = -33.8569,151.2152
     & zoom = 18
     & maptype = satellite


        AIzaSyCnLmacv0vthhAh4vLsUkQs - 7CC_7i8mGI
        */
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
            + "(DN_13206)&iwloc=A&hl=en&output=embed&maptype=" + MapType;

        string html = "<blah><head><style type=\"text/css\">body {scrolling:no;}iframe {position:absolute;z-index:1;   top:0px;    left:0px;}</style></head><body><iframe src=\"" + url +  "\" height=\"100%\" width=\"100%\" frameborder=\"0\"></iframe></body>";

        browser.LoadHTML(html);
        Debug.Log(url);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
