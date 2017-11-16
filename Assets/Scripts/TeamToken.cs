using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamToken : NetworkBehaviour {

    /// <summary>
    /// The GameObject that controls the team (the player that made/owns this object)
    /// </summary>
    [SyncVar]
    public GameObject teamCaptain;
    public Color teamColor = Color.red;//by default, all team colors will be red, except for your own color
    
    /// <summary>
    /// Returns whether or not the given GameObjects are on the same team
    /// </summary>
    /// <param name="obja"></param>
    /// <param name="objb"></param>
    /// <returns></returns>
    public static bool isFriendly(GameObject obja, GameObject objb)
    {
        TeamToken tta = obja.GetComponent<TeamToken>();
        TeamToken ttb = objb.GetComponent<TeamToken>();
        if (tta == null || ttb == null)
        {
            //If there's no explicit team defined,
            //assume it's not friendly
            return false;
        }
        return tta.teamCaptain == ttb.teamCaptain;
    }

    /// <summary>
    /// Assigns the given recruit to the team of the given member
    /// </summary>
    /// <param name="recruit"></param>
    /// <param name="member"></param>
    public static void assignTeam(GameObject recruit, GameObject member)
    {
        member.GetComponent<TeamToken>().CmdAssignTeams(recruit);
        //TeamToken ttr = recruit.GetComponent<TeamToken>();
        //TeamToken ttm = member.GetComponent<TeamToken>();
        //ttr.teamColor = ttm.teamColor;
    }

    [Command]
    void CmdAssignTeams(GameObject recruit)
    {
        TeamToken ttr = recruit.GetComponent<TeamToken>();
        if (ttr == null)
        {
            throw new UnityException("TeamToken.CmdAssignTeams(): Recruit doesn't have a TeamToken! recruit: " + recruit.name);
        }
        ttr.teamCaptain = teamCaptain;
    }

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (teamCaptain.GetComponent<PlayerController>().isLocalPlayer)
        {
            teamColor = Color.white;
        }
        sr.color = teamColor;
    }
}
