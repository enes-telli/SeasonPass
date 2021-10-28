using UnityEngine;
using TMPro;

public class Clothes : MonoBehaviour
{
    public CharacterCustomization.ClothesPartType clothesPartType;
    public int index;
    public SeasonTheme relatedSeason;

    private GameObject clothesGO;
    
    private float rotateSpeed = 120f;
    private Vector3 startingScale;

    private void Start()
    {
        clothesGO = GameObject.Find("Clothes");
        startingScale = transform.localScale;

        string zonePath = "Zones/" + relatedSeason;
        GameObject zone = Instantiate(Resources.Load(zonePath)) as GameObject;
        zone.GetComponent<Zone>().linkedClothes = this;
        zone.transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);
        zone.transform.SetParent(transform.parent);

        GameObject seasonText = Instantiate(Resources.Load("SeasonText")) as GameObject;
        seasonText.GetComponent<TextMeshPro>().text = relatedSeason.ToString();
        seasonText.transform.position = new Vector3(transform.position.x, 3.2f, transform.position.z);
        seasonText.transform.SetParent(transform);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0), Space.World);
        transform.localScale = Vector3.Lerp(startingScale, startingScale * 1.2f, Mathf.PingPong(Time.time, 1));
    }

    public void SetClothes(Collider other)
    {
        other.GetComponent<CharacterCustomization>().SetElementByIndex(clothesPartType, index);
        
        if (SeasonManager.currentSeason != relatedSeason)
        {
            SeasonManager.LoseLives();
        }
        string path = DetectEmojiPath();
        GameObject emoji = Instantiate(Resources.Load(path) as GameObject, new Vector3(transform.position.x, 3f, transform.position.z), Quaternion.identity);

        Destroy(clothesGO.transform.GetChild(0).gameObject);
    }

    private string DetectEmojiPath()
    {
        string path = "Emojis/";
        
        if (SeasonManager.currentSeason.Equals(SeasonTheme.AUTUMN))
        {
            if (relatedSeason.Equals(SeasonTheme.AUTUMN)) path += "EmojiCool";
            else if (relatedSeason.Equals(SeasonTheme.SUMMER)) path += "EmojiAngry";
            else if (relatedSeason.Equals(SeasonTheme.WINTER)) path += "EmojiSad";
        }
        else if (SeasonManager.currentSeason.Equals(SeasonTheme.SUMMER))
        {
            if (relatedSeason.Equals(SeasonTheme.AUTUMN)) path += "EmojiSad";
            else if (relatedSeason.Equals(SeasonTheme.SUMMER)) path += "EmojiCool";
            else if (relatedSeason.Equals(SeasonTheme.WINTER)) path += "EmojiYawn";
        }
        else if (SeasonManager.currentSeason.Equals(SeasonTheme.WINTER))
        {
            if (relatedSeason.Equals(SeasonTheme.AUTUMN)) path += "EmojiYawn";
            else if (relatedSeason.Equals(SeasonTheme.SUMMER)) path += "EmojiScared";
            else if (relatedSeason.Equals(SeasonTheme.WINTER)) path += "EmojiCool";
        }

        return path;
    }
}
