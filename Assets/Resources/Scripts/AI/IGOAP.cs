using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGOAP {

	HashSet<KeyValuePair<string, object>> getWorldState();

	HashSet<KeyValuePair<string, object>> createGoalState();

	void planFailed(HashSet<KeyValuePair<string, object>> failedGoal);

	void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions);

	void actionsFinished();

	void planAborted(GOAPAction aborter);

	bool moveAgent(GOAPAction nextAction);
}
