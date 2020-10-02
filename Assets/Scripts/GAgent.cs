using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> sgoals;

    // flag to indicate if this goal has to be removed after been satisfied
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{

    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    private GPlanner planner;
    private Queue<GAction> actionQueue;

    public GAction currentAction;

    private SubGoal currentGoal;

    private bool invoked = false;

    // Start is called before the first frame update
    protected void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a);
        }
    }

    void LateUpdate()
    {
        // there is an action running, check if it has finished
        if (currentAction != null && !currentAction.running)
        {
            // check if the agent has reached its destination
            if (currentAction.agent.hasPath && currentAction.agent.remainingDistance < 1f)
            {
                // run the complition of the action
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if (planner == null || actionQueue == null)
        {
            // the agent has no plan create a new one
            planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.Plan(actions, sg.Key.sgoals, null);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (actionQueue != null)
        {
            if (actionQueue.Count == 0)
            {
                // action queue is empty
                if (currentGoal.remove)
                {
                    goals.Remove(currentGoal);
                }
                planner = null;
            }
            else if (actionQueue.Count > 0)
            {
                // prepare the next action of the plan
                currentAction = actionQueue.Dequeue();
                if (currentAction.PrePerform())
                {
                    if (currentAction.target == null && currentAction.targetTag != "")
                        currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                    if (currentAction.target != null)
                    {
                        currentAction.running = true;
                        currentAction.agent.SetDestination(currentAction.target.transform.position);
                    }
                }
                else
                {
                    actionQueue = null;
                }
            }
        }
    }

    private void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

}
