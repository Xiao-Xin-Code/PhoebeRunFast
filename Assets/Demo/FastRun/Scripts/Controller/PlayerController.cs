using Frame;
using QMVC;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;



public class PlayerController : BaseController
{
	protected override void Init()
	{
		control = new InputController();
		control.Player.Enable();
		control.Player.Left.performed += OnLeftPressed;
		control.Player.Right.performed += OnRightPressed;
		control.Player.Up.performed += OnUpPressed;
		control.Player.Down.performed += OnDownPressed;

		MonoService.Instance.AddUpdateListener(TouchInput);
		MonoService.Instance.AddFixedUpdateListener(PlayerMove);

		RoadSystem roadsystem = this.GetSystem<RoadSystem>();
		roadsystem.playerController = this;
	}

	InputController control;
	float speed = 5;
	float space = 1;
	float upForce = 10;

	[SerializeField] GameObject commCollision;
	[SerializeField] GameObject halfCollision;



	private void OnLeftPressed(InputAction.CallbackContext callback)
	{
		Debug.Log("Left");
		LeftMovement();
	}

	private void OnRightPressed(InputAction.CallbackContext callback)
	{
		Debug.Log("Right");
		RightMovement();
	}

	private void OnUpPressed(InputAction.CallbackContext callback)
	{
		Debug.Log("Up");
		commCollision.SetActive(true);
		halfCollision.SetActive(false);

		GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
		GetComponent<Rigidbody>().AddForce(Vector3.up * upForce, ForceMode.Impulse);
	}

	private void OnDownPressed(InputAction.CallbackContext callback)
	{
		Debug.Log("Down");
		commCollision.SetActive(false);
		halfCollision.SetActive(true);
	}


	

	private void PlayerMove()
	{
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	public bool isTurn;

	bool isDrag = false;
	Vector2 startPos;
	bool useTouch = true;
	float minDis = 100;

	private void TouchInput()
	{
		if (useTouch) 
		{
			Mouse mouse = Mouse.current;

			if (mouse.leftButton.wasPressedThisFrame)
			{
				startPos = mouse.position.ReadValue();
			}

			if (mouse.leftButton.isPressed)
			{
				isDrag = true;
			}

			if (mouse.leftButton.wasReleasedThisFrame)
			{
				if (isDrag)
				{
					Vector2 dur = mouse.position.ReadValue() - startPos;
					float angle = Vector2.Angle(Vector2.left, dur);
					Debug.Log(dur);
					if (angle >= 0 && angle <= 30)
					{
						if (dur.x < -minDis) 
						{
							if (isTurn)
							{
								isTurn = false;
								LeftTurn(90);
							}
							else
							{
								Debug.Log("Left");
								LeftMovement();
							}
						}
					}
					else if (angle >= 60 && angle <= 120)
					{
						if (dur.y > 0 && dur.y > minDis)
						{
							Debug.Log("Up");
						}
						else if (dur.y < 0 && dur.y < -minDis) 
						{
							Debug.Log("Down");
						}
					}
					else if (angle >= 150 && angle <= 180) 
					{
						if (dur.x > minDis)  
						{
							if (isTurn)
							{
								isTurn = false;
								RightTurn(90);
							}
							else
							{
								Debug.Log("Right");
								RightMovement();
							}
						}
					}
				}
			}
		}
	}




	private void LeftMovement()
	{
		transform.position -= transform.right * space;
	}

	private void RightMovement()
	{
		transform.position += transform.right * space;
	}

	private void LeftTurn(float angle)
	{
		transform.Rotate(new Vector3(0, -angle, 0));
	}

	private void RightTurn(float angle)
	{
		transform.Rotate(new Vector3(0, angle, 0));
	}



}
