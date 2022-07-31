using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
	private AudioSource bubbleSound;
	private ParticleSystem bubbleParticleEffect;
	private Image bubbleImage;
	private Animator animator;
	private Vector2Int keyOfBubbleOnGrid;

	private readonly int animatorState_Idle = Animator.StringToHash("Idle");
	private readonly int animatorState_Destroy = Animator.StringToHash("Destroy");
	private void Awake()
	{
		bubbleSound = GetComponent<AudioSource>();
		bubbleParticleEffect = GetComponent<ParticleSystem>();
		bubbleImage = GetComponent<Image>();
		animator = GetComponent<Animator>();
		animator.keepAnimatorControllerStateOnDisable = true;
	}

	public void DestroyBubble()
	{
		bubbleParticleEffect.Play();
		animator.SetTrigger(animatorState_Destroy);
		bubbleImage.raycastTarget = false;
	}

	public void TouchBubble()
	{
		if (!BoardController.singletonBoardController.IsBoardRefreshing() && BoardController.singletonBoardController.IsBoardClickable())
		{
			bubbleSound.Play();
			BoardController.singletonBoardController.RemoveBubbleFromBoard(keyOfBubbleOnGrid);
		}
	}

	public void InitializeBubble(Vector2Int key, Color color)
	{
		keyOfBubbleOnGrid = key;
		bubbleImage.color = color;
		bubbleParticleEffect.startColor = bubbleImage.color;
		animator.SetTrigger(animatorState_Idle);
		bubbleImage.raycastTarget = true;
	}

	public void UpdateBubblePositionOnGrid(Vector2Int newKey, Transform newPosition)
	{
		keyOfBubbleOnGrid = newKey;
		transform.position = newPosition.transform.position;
	}

	public bool IsBubbleSimilar(Bubble comparingBubble) => comparingBubble.bubbleImage.color == this.bubbleImage.color;
}
