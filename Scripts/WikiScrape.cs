using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class WikiScrape : MonoBehaviour
{
    private static WikiScrape _instance;
    public static WikiScrape instance
    {
        get { return _instance; }
    }

    public string url;
    public List<string> hyperLinks = new List<string>();
    
    //[SerializeField] Image image;
    //public List<Image> thumbnails = new List<Image>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        LinkScrape(url);
        //ImageScrape(url);
    }

    public void LinkScrape(string HTML)
    {
        string linkToFind;
        int cycleProtection = 0;

        clearList();

        WebRequest.GetData(HTML, (string error) =>
        {
            //Error
            Debug.LogError(error);
        },
       (string htmlCode) =>
       {
            //Successfully contacted URL
            Debug.Log("Received: " + htmlCode);

           while (htmlCode.IndexOf("<a href=\"/wiki/") != -1 && cycleProtection <100)
           {
               cycleProtection++;
               linkToFind = "<a href=\"/wiki/";
               htmlCode = htmlCode.Substring(htmlCode.IndexOf(linkToFind) + linkToFind.Length);
               string link = "https://en.wikipedia.org/wiki/" + htmlCode.Substring(0, htmlCode.IndexOf("\""));

               hyperLinks.Add(link);

           }
       });
    }

    public void addLinkToCitizen(Enemy citizen)
    {  
        for (int listNum = 0 ; string.IsNullOrEmpty(citizen.link); listNum++)
        {
            if (hyperLinks[listNum] != null)
            {
                citizen.link = hyperLinks[listNum];
                hyperLinks.RemoveAt(listNum);
                return;
            }
        }
    }

    private void clearList()
    {
        GameObject[] citizens = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject citizen in citizens)
        {
            citizen.GetComponent<Enemy>().link = null;
        }
        hyperLinks.Clear();
    }
    /*
    private void ImageScrape(string HTML)
    {
        string imageToFind;
        int cycleProtection = 0;
        //Gets Image URL
        WebRequest.GetData(HTML, (string error) =>
        {
            //Error
            Debug.LogError(error);
        },
       (string htmlCode) =>
       {
           //Successfully contacted URL
           Debug.Log("Received: " + htmlCode);
           while (htmlCode.IndexOf("jpg\" src=\"") != -1 && cycleProtection < 100)
           {
               cycleProtection++;
               imageToFind = "jpg\" src=\"";
               htmlCode = htmlCode.Substring(htmlCode.IndexOf(imageToFind) + imageToFind.Length);
               string imageURL = htmlCode.Substring(0, htmlCode.IndexOf(" "));
               hyperLinks.Add(imageURL);
           }
           
       });
        //Gets Actual Image
        /*
        WebRequest.GetTexture(HTML, (string error) =>
        {
            //Error
            Debug.LogError("error: " + error);
        },
        (Texture2D texture2D) =>
        {
            //Successfully contacted URL
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f), 10);
            image.sprite = sprite;
        });
        
    }
    */
}

