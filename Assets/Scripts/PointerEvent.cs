public class PointerEvent
{
    private System.Action action;

    public void AddListener(System.Action action)
    {
        this.action += action;
    }

    public void RemoveListeners()
    {
        this.action = null;
    }

    public void DoInvoke(System.Action OnInvoke = null)
    {
        action?.Invoke();
        OnInvoke?.Invoke();
    }
}