using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController
{

    private GameObject ghostControllerObj_;
    private Ghost_Stats ghost_Stats_;

    public GhostController(GameObject controller)
    {
        ghostControllerObj_ = controller;
    }

    public void Awake()
    {

    }
    public void Start(Ghost_Stats ghost_Stats)
    {
        ghost_Stats_ = ghost_Stats;
    }
    public void Update() 
    {
        switch (ghost_Stats_.ghostCurrentState)
        {
            case GhostState.GHOST_IDLE:

                break;
            case GhostState.GHOST_MOVEMENT:

                break;
            case GhostState.GHOST_POSSESSED:

                break;

            default:
                break;
        }
    }
}
