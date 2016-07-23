using UnityEngine;
using System.Collections;

public interface FSMState {

	void Update (FSMState fsm, GameObject obj);
}
