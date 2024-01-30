using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//***************
//JSON UDT CLASS DEFINITION

[System.Serializable]
public class TagsAll 
{
    public TagInfo[] readResults; //Gets READRESULT, which is the main payload (list of Tags)
    //public int status;
    public TagsEnum GetEnumerator() //Allows for ForLooping through
    {
        return new TagsEnum(readResults);
    }
}

[System.Serializable]
public class TagInfo //each "tag" has the following JSON encoded properties
{
    public string id; //names must be verbatum within the JSON structure
    public bool s;
    public string r;
    public int v;
    public int t;
}

public class TagsEnum : IEnumerator //Necessary to forloop through the array of TagInfo[] from TagsAll class
{
    public TagInfo[] _tags;
    int position = -1;

    public TagsEnum(TagInfo[] list)
    {
        _tags = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _tags.Length);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public TagInfo Current
    {
        get
        {
            try
            {
                return _tags[position];
            }
            catch (System.IndexOutOfRangeException)
            {
                throw new System.InvalidOperationException();
            }
        }
    }
}


//***************
// STANDARD CLASS
public class HTTP_Request : MonoBehaviour
{

    public Text textTag1;
    public Text textTag2;

    private string url;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        yield return new WaitForSeconds(1f);
        url = "http://127.0.0.1:39320/iotgateway/read?ids=Simulation%20Examples.Functions.Ramp5&ids=Simulation%20Examples.Functions.Random1";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        //Debug.Log("GetText");
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonResult = www.downloadHandler.text;
            //Debug.Log("JSON: "+jsonResult);
            TagsAll _tags = JsonUtility.FromJson<TagsAll>(jsonResult);
            int index = 1;
            foreach (var t in _tags)
            {
                Debug.Log("Tag "+index.ToString() + " - " +t.v.ToString() );
                index++;
            }
            textTag1.text = _tags.readResults[0].v.ToString();
            textTag2.text = _tags.readResults[1].v.ToString();

            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
        }
    }

}
