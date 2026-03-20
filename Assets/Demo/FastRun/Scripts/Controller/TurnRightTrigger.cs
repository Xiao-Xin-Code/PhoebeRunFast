using QMVC;
using UnityEngine;

public class TurnRightTrigger : BaseTrigger
{

	protected override void OnValidTriggerEnter(Collider other)
	{
		other.GetComponent<PlayerController>().isTurn = true;
		this.SendCommand<PlayerCommandChannel.TurnRightTriggerCommand>();
	}

	protected override void OnValidTriggerStay(Collider other)
	{
		
	}
}
