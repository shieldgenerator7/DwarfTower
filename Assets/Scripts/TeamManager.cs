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

    private GameObject[] teamCaptains = new GameObject[3];
    
    // Use this for initialization
    void Start()
    {
    }
    
    public void assignTeam(GameObject go)
    {
        PlayerController pc = go.GetComponent<PlayerController>();
        playerCount++;
        string name = pc.name + " Player " + playerCount;
        pc.name = name;
        pc.RpcRenamePlayer(name);
        if (team1Count == 0 || team1Count < team2Count)
        {
            assignToTeam(pc, 1);
        }
        else if (team2Count < team1Count)
        {
            assignToTeam(pc, 2);
        }
        else
        {
            int randomTeam = Random.Range(1, 3);
            assignToTeam(pc, randomTeam);
        }
    }

    private void assignToTeam(PlayerController pc, int teamNumber)
    {
        switch (teamNumber)
        {
            case 1: team1Count++; break;
            case 2: team2Count++; break;
            default: throw new UnityException("Invalid team number: " + teamNumber);
        }
        if (teamCaptains[teamNumber] == null)
        {
            TeamToken tt = pc.GetComponent<TeamToken>();
            tt.teamCaptain = pc.gameObject;
            teamCaptains[teamNumber] = pc.gameObject;
        }
        TeamToken.assignTeam(pc.gameObject, teamCaptains[teamNumber]);
    }
}
