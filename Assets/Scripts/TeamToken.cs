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

    private static TeamToken instanceLocalPlayer;

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
        if (tta == null || ttb == null
            || tta.teamCaptain == null
            || ttb.teamCaptain == null)
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
        TeamToken ttm = member.GetComponent<TeamToken>();
        TeamToken ttr = recruit.GetComponent<TeamToken>();
        if (ttr == null)
        {
            throw new UnityException("TeamToken.CmdAssignTeams(): Recruit doesn't have a TeamToken! recruit: " + recruit.name);
        }
        ttr.teamCaptain = ttm.teamCaptain;
        ttr.RpcAssignTeams();
    }

    [ClientRpc]
    void RpcAssignTeams()
    {
        assignTeamColor();
    }

    private SpriteRenderer sr;

    private void Start()
    {
        assignTeamColor();
    }

    public void assignTeamColor()
    {
        if (instanceLocalPlayer == null)
        {
            if (isLocalPlayer)
            {
                instanceLocalPlayer = this;
            }
            else
            {
                return;
            }
        }
        if (isLocalPlayer || TeamToken.isFriendly(instanceLocalPlayer.gameObject, gameObject))
        {
            teamColor = Color.white;
        }
        else
        {
            teamColor = Color.red;
        }
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        sr.color = teamColor;
        if (isLocalPlayer)
        {
            foreach (TeamToken tt in FindObjectsOfType<TeamToken>())
            {
                if (!tt.isLocalPlayer)
                {
                    tt.assignTeamColor();
                }
            }
        }
    }

    private static void findLocalPlayer()
    {
        foreach (TeamToken tt in FindObjectsOfType<TeamToken>())
        {
            if (tt.isLocalPlayer)
            {
                instanceLocalPlayer = tt;
                return;
            }
        }
    }
}
