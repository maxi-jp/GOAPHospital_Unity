using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.RemovePatientFromTheWaitingRoom();
        if (target == null)
            return false;

        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

}
