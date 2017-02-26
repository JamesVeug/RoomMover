public class TapButton : Triggerable
{
    public AudioClipType PressSound = AudioClipType.ButtonPress;

    public Triggerable TriggerOnPush = null;

    private bool isPressed = false;

    public void OnPush()
    {
        if(TriggerOnPush != null)
        {
            if (!isPressed)
            {
                TriggerOnPush.Toggle();
                AudioManager.Instance.PlaySound(PressSound);
            }
            isPressed = !isPressed;

        }
    }

    public override void Toggle()
    {
        OnPush();
    }
}
