using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//2017-11-15: FUTURE CODE: This class will manage teams and which player is on which team
public class TeamManager : NetworkBehaviour
{
    [SyncVar]
    public int team1Count = 0;
    [SyncVar]
    public int team2Count = 0;

    [SyncVar]
    public int playerCount = 0;
    
    private TeamToken[] teamCaptains = new TeamToken[3];

    private static TeamManager instance;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
    public static void assignTeam(PlayerController pc)
    {
        if (instance.team1Count == 0 || instance.team1Count < instance.team2Count)
        {
            instance.assignToTeam(pc, 1);
        }
        else if (instance.team2Count < instance.team1Count)
        {
            instance.assignToTeam(pc, 2);
        }
        else
        {
            int randomTeam = Random.Range(1,3);
            instance.assignToTeam(pc, randomTeam);
        }
        instance.playerCount++;
        pc.name += " Player " + instance.playerCount;
    }

    private void assignToTeam(PlayerController pc, int teamNumber)
    {
        if (teamCaptains[teamNumber] == null)
        {
            TeamToken tt = pc.GetComponent<TeamToken>();
            tt.teamCaptain = pc.gameObject;
            teamCaptains[teamNumber] = tt;
        }
        TeamToken.assignTeam(pc.gameObject, teamCaptains[teamNumber].gameObject);
    }
}
