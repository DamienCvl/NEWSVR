using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DevStats : MonoBehaviour
{

    public Dropdown playersDD;
    public Dropdown newsDD;

    private Dictionary<int, string> players;

    // Start is called before the first frame update
    void Start()
    {
        Database.GenerateNewsList();
        players = Database.GetPlayers();
        playersDD.AddOptions(players.Values.ToList());
        newsDD.AddOptions(StaticClass.newsList.Select(n => n.GetTitle()).OrderBy(x => x).ToList());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayStats()
    {
        if (playersDD.value == 0)
        {
            if (newsDD.value == 0)
            {
                // Afficher la derniere vue sur toutes les news de tout les joueurs
            }
            else
            {
                // Afficher la derniere vue sur la new X de tout les joueurs
            }
        }
        else
        {
            if (newsDD.value == 0)
            {
                // Afficher la derniere vue sur toutes les news du joueurs X
            }
            else
            {
                // Afficher la derniere vue sur la new X du joueurs X
            }
        }
    }
}
