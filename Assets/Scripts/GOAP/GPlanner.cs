using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);
        this.action = action;
    }
}

public class GPlanner
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        // filter the usable actions
        List<GAction> usableActions = new List<GAction>();
        foreach (GAction a in actions)
        {
            if (a.IsAchievable())
                usableActions.Add(a);
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            /*if (cheapest == null || leaf.cost < cheapest.cost)
                cheapest = leaf;*/
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.cost < cheapest.cost)
                    cheapest = leaf;
            }
        }

        List<GAction> results = new List<GAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                results.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction a in results)
        {
            queue.Enqueue(a);
        }

        // return the plan
        Debug.Log("The Plan is: ");
        foreach (GAction a in queue)
            Debug.Log("Q: " + a.actionName);

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> nodes, List<GAction> actions, Dictionary<string, int> goal)
    {
        bool foundPath = false;

        foreach (GAction action in actions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> effect in action.effects)
                {
                    if (!currentState.ContainsKey(effect.Key))
                        currentState.Add(effect.Key, effect.Value);
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    nodes.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(actions, action);
                    foundPath = BuildGraph(node, nodes, subset, goal);
                }
            }
        }

        return foundPath;
    }

    // Check if the given state achieved every goal keys
    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }
        return true;
    }

    // Returns of a copy of the actions list removing the given action
    private List<GAction> ActionSubset(List<GAction> actions, GAction action)
    {
        List<GAction> subset = new List<GAction>();

        foreach (GAction a in actions)
        {
            if (!a.Equals(action))
                subset.Add(a);
        }

        return subset;
    }
}
