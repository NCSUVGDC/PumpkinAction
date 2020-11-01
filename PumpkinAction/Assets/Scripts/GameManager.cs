using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject[] teamCandles;
    public List<Team> remainingTeams = new List<Team>() { Team.Team1, Team.Team2, Team.Team3, Team.Team4 } ;
    [Tooltip("Assign the player bases in order of their team number")]
    public List<PlayerSpawner> spawners;


    public List<int> deathsPerTeam = new List<int>() { 0, 0, 0, 0 };

    public void CandleDestroyed(Team team)
    {
        remainingTeams.Remove(team);

        if(remainingTeams.Count == 1)
        {
            Debug.Log(remainingTeams[0].ToString() + " is the spookiest!");
        }
        
    }

    public Transform requestSpawn(Team team)
    {
        if (remainingTeams.Contains(team))
        {
            deathsPerTeam[(int) team]++;
            return spawners[(int)team].RequestSpawnpoint();
        }
        return null;
    }
}
