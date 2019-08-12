using Assets.Scripts.Core;
using Assets.Scripts.DevMode;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevStats : MonoBehaviour
{

    public Dropdown playersDD;
    public Dropdown newsDD;
    public GameObject contentParent;

    private Dictionary<int, uint> players = new Dictionary<int, uint>(); // Index in playersDD, player ID
    private Dictionary<int, uint> news = new Dictionary<int, uint>(); // Index in newsDD, news ID

    private GameObject rowPrefab;
    private List<DevStatsData> dataToDisplay;

    private void Awake()
    {
        rowPrefab = (GameObject)Resources.Load("Prefabs/DevMode/Row", typeof(GameObject));
    }

    // Start is called before the first frame update
    void Start()
    {
        // ********** DATA GENERATION *********** //
        foreach (KeyValuePair<uint, string> player in Database.GetPlayers())
        {
            playersDD.options.Add(new Dropdown.OptionData(player.Value));
            players.Add(playersDD.options.Count - 1, player.Key);
        }

        List<uint> idsNewsOrderByTitle = StaticClass.newsList.OrderBy(x => x.GetTitle()).Select(n => n.GetId()).ToList();
        List<string> titlesNewsOrderByTitle = StaticClass.newsList.Select(n => n.GetTitle()).OrderBy(x => x).ToList();

        for (int i = 0; i < idsNewsOrderByTitle.Count; i++)
        {
            newsDD.options.Add(new Dropdown.OptionData(titlesNewsOrderByTitle[i]));
            news.Add(newsDD.options.Count - 1, idsNewsOrderByTitle[i]);
        }
        // ************************************** //
        DisplayStats();
    }

    public void DisplayStats()
    {
        foreach (Transform child in contentParent.transform)
        {
            if (child.GetSiblingIndex() != 0)
                Destroy(child.gameObject);
        }

        if (playersDD.value == 0)
        {
            if (newsDD.value == 0)
                dataToDisplay = Database.GetDevStatsData(0, 0, false, false); // Select all players and all news in SQL cmd
            else
                dataToDisplay = Database.GetDevStatsData(news[newsDD.value], 0, true, false); // Select all players in SQL cmd
        }
        else
        {
            if (newsDD.value == 0)
                dataToDisplay = Database.GetDevStatsData(0, players[playersDD.value], false, true); // Select all news in SQL cmd
            else
                dataToDisplay = Database.GetDevStatsData(news[newsDD.value], players[playersDD.value], true, true); // Specific player and news
        }

        foreach (DevStatsData data in dataToDisplay)
        {
            GameObject row = Instantiate(rowPrefab, contentParent.transform);
            row.GetComponent<DevStatsRow>().Fill(data.playerName, data.newsTitle, data.reaction, data.nbCmt, data.date);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(6); // Go back to Admin Scene
    }
}
