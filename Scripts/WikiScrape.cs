using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class WikiScrape : MonoBehaviour
{
    private static WikiScrape _instance;
    public static WikiScrape instance
    {
        get { return _instance; }
    }

    public string url;
    public List<string> hyperLinks = new List<string>();
    // List of links to exclude from shootable targets
    public List<string> Links_to_Exclude = new List<string>();
    List<string> Good_Links = new List<string>();

    //[SerializeField] Image image;
    //public List<Image> thumbnails = new List<Image>();

    private void Awake()
    {
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Main_Page");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Wikipedia:Contents");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Portal:Current_events");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Wikipedia:About");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Wikipedia:Contact_us");
        Links_to_Exclude.Add("https://donate.wikimedia.org/w/index.php?title=Special%3ALandingPage&country=US&uselang=en");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Help:Link");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Help:Contents");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Help:Category");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Special:SpecialPages");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Help:Introduction");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Wikipedia:Community_portal");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Special:RecentChanges?hidebots=1&hidecategorization=1&hideWikibase=1&limit=50&days=7&urlversion=2");
        Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Wikipedia:File_Upload_Wizard");

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
               Links_to_Exclude.Add("https://en.wikipedia.org/wiki/Special:WhatLinksHere/" + htmlCode.Substring(0, htmlCode.IndexOf("\"")));

               hyperLinks.Add(link);
               // Good_Links now contains the page links excluding the ones from Links_to_Exclude
               Good_Links = hyperLinks.Except(Links_to_Exclude).ToList();

           }
       });
    }

    public void addLinkToCitizen(Enemy citizen)
    {
        for (int listNum = 0; string.IsNullOrEmpty(citizen.link); listNum++)
        {
            if (Good_Links[listNum] != null)
            {
                citizen.link = Good_Links[listNum];
                Good_Links.RemoveAt(listNum);
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
        Good_Links.Clear();
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


