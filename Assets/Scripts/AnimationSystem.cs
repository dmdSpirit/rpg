using System;

public class AnimationSystem : MonoSingleton<AnimationSystem>
{
    public event Action<bool> OnBlockingAnimationStateChange;

    public void StartAnimation()
    {
        OnBlockingAnimationStateChange?.Invoke(true);
    }

    public void StopAnimation()
    {
        OnBlockingAnimationStateChange?.Invoke(false);
    }
}