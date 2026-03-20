using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AxisLock : CinemachineExtension
{
	public bool lockX = false;

	public bool lockY = false;

	public bool lockZ = false;


	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage == CinemachineCore.Stage.Body)
		{
			Vector3 pos = state.RawPosition;

			// 获取目标位置（玩家位置）
			if (vcam.Follow != null)
			{
				if (lockX)
					pos.x = vcam.transform.position.x;

				if (lockY)
					pos.y = vcam.transform.position.y;

				if (lockZ)
					pos.z = vcam.transform.position.z;
			}

			state.RawPosition = pos;
		}
	}

	
}
