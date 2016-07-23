using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Plans what actions can be completed in order to fulfill a goal state.
 */
public class GOAPPlanner
{

	/**
	 * Plan what sequence of actions can fulfill the goal.
	 * Returns null if a plan could not be found, or a list of the actions
	 * that must be performed, in order, to fulfill the goal.
	 */
	public Queue<GOAPAction> plan(GameObject agent,
								  HashSet<GOAPAction> availableActions, 
	                              HashSet<KeyValuePair<string,object>> worldState, 
	                              HashSet<KeyValuePair<string,object>> goal) 
	{
		// reset the actions so we can start fresh with them
		foreach (GOAPAction a in availableActions) {
			a.doReset ();
		}

		// check what actions can run using their checkProceduralPrecondition
		HashSet<GOAPAction> usableActions = new HashSet<GOAPAction> ();
		foreach (GOAPAction a in availableActions) {
			if ( a.checkProceduralPrecondition(agent) )
				usableActions.Add(a);
		}
		
		// we now have all actions that can run, stored in usableActions

		// build up the tree and record the leaf nodes that provide a solution to the goal.
		List<Node> leaves = new List<Node>();

		// build graph
		Node start = new Node (null, 0, worldState, null);
		bool success = buildGraph(start, leaves, usableActions, goal);

		if (!success) {
			// oh no, we didn't get a plan
			//Debug.Log("NO PLAN");
			return null;
		}

		// get the cheapest leaf
		Node cheapest = null;
		foreach (Node leaf in leaves) {
			if (cheapest == null)
				cheapest = leaf;
			else {
				if (leaf.runningCost < cheapest.runningCost)
					cheapest = leaf;
			}
		}

		// get its node and work back through the parents
		List<GOAPAction> result = new List<GOAPAction> ();
		Node n = cheapest;
		while (n != null) {
			if (n.action != null) {
				result.Insert(0, n.action); // insert the action in the front
			}
			n = n.parent;
		}
		// we now have this action list in correct order

		Queue<GOAPAction> queue = new Queue<GOAPAction> ();
		foreach (GOAPAction a in result) {
			queue.Enqueue(a);
		}

		// hooray we have a plan!
		return queue;
	}

	/**
	 * Returns true if at least one solution was found.
	 * The possible paths are stored in the leaves list. Each leaf has a
	 * 'runningCost' value where the lowest cost will be the best action
	 * sequence.
	 */
	protected bool buildGraph (Node parent, List<Node> leaves, HashSet<GOAPAction> usableActions, HashSet<KeyValuePair<string, object>> goal)
	{
		bool foundOne = false;

		// go through each action available at this node and see if we can use it here
		foreach (GOAPAction action in usableActions) {

			// if the parent state has the conditions for this action's preconditions, we can use it here
			if ( inState(action.Preconditions, parent.state) ) {

				// apply the action's effects to the parent state
				HashSet<KeyValuePair<string,object>> currentState = populateState (parent.state, action.Effects);
				//Debug.Log(GoapAgent.prettyPrint(currentState));
				Node node = new Node(parent, parent.runningCost + action.cost, currentState, action);

				if (goalInState (goal, currentState)) {
					// we found a solution!
					leaves.Add (node);
					foundOne = true;
				} else {
					// test all the remaining actions and branch out the tree
					HashSet<GOAPAction> subset = actionSubset(usableActions, action);
					bool found = buildGraph(node, leaves, subset, goal);
					if (found)
						foundOne = true;
				}


			}
		}

		return foundOne;
	}

	/**
	 * Create a subset of the actions excluding the removeMe one. Creates a new set.
	 */
	protected HashSet<GOAPAction> actionSubset(HashSet<GOAPAction> actions, GOAPAction removeMe) {
		HashSet<GOAPAction> subset = new HashSet<GOAPAction> ();
		foreach (GOAPAction a in actions) {
			if (!a.Equals(removeMe))
				subset.Add(a);
		}
		return subset;
	}

	/*
	 * Checks if at least one goal is met. 
	 * to-do: Create a system for weighting towards paths that fulfill more goals
	 */
	protected bool goalInState(HashSet<KeyValuePair<string,object>> test, HashSet<KeyValuePair<string,object>> state) {
		bool match = false;
		foreach (KeyValuePair<string,object> t in test) {
			foreach (KeyValuePair<string,object> s in state) {
				if (s.Equals (t)) {
					match = true;
					break;
				}
			}
		}
		return match;
	}

	/*
	 * Check that all items in 'test' are in 'state'. If just one does not match or is not there
	 * then this returns false.
	 */
	protected bool inState(HashSet<KeyValuePair<string,object>> test, HashSet<KeyValuePair<string,object>> state) {
		bool allMatch = true;
		foreach (KeyValuePair<string,object> t in test) {
			bool match = false;
			foreach (KeyValuePair<string,object> s in state) {
				if (s.Equals(t)) {
					match = true;
					break;
				}
			}
			if (!match)
				allMatch = false;
		}
		return allMatch;
	}
	
	/**
	 * Apply the stateChange to the currentState
	 */
	protected HashSet<KeyValuePair<string,object>> populateState(HashSet<KeyValuePair<string,object>> currentState, HashSet<KeyValuePair<string,object>> stateChange) {
		HashSet<KeyValuePair<string,object>> state = new HashSet<KeyValuePair<string,object>> ();
		// copy the KVPs over as new objects
		foreach (KeyValuePair<string,object> s in currentState) {
			state.Add(new KeyValuePair<string, object>(s.Key,s.Value));
		}

		foreach (KeyValuePair<string,object> change in stateChange) {
			// if the key exists in the current state, update the Value
			bool exists = false;

			foreach (KeyValuePair<string,object> s in state) {
				if (s.Key.Equals(change.Key)) {
					exists = true;
					break;
				}
			}

			if (exists) {
				state.RemoveWhere( (KeyValuePair<string,object> kvp) => { return kvp.Key.Equals (change.Key); } );
				KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key,change.Value);
				state.Add(updated);
			}
			// if it does not exist in the current state, add it
			else {
				state.Add(new KeyValuePair<string, object>(change.Key,change.Value));
			}
		}
		return state;
	}

	/**
	 * Used for building up the graph and holding the running costs of actions.
	 */
	protected class Node {
		public Node parent;
		public float runningCost;
		public HashSet<KeyValuePair<string,object>> state;
		public GOAPAction action;

		public Node(Node parent, float runningCost, HashSet<KeyValuePair<string,object>> state, GOAPAction action) {
			this.parent = parent;
			this.runningCost = runningCost;
			this.state = state;
			this.action = action;
		}
	}

}

