using Silk.NET.Input;

namespace UniversityGameProject.Input;

public enum KeyboardButton
{
    W,
    A,
    S,
    D,
    Space,
    Shift,
    Tab,
    Escape,
    Enter,
    Unknown,
}

public class InputEventKey : InputEvent
{
    public KeyboardButton Button { get; init; }
    public InputEventKey(InputServer server, KeyboardButton button, bool isPressed = false) : base(server)
    {
        Button = button;
        IsInvoked = isPressed;
    }

    public InputEventKey(InputServer server, Key button, bool isPressed = false) : base(server)
    {
        Button = button switch
        {
            Key.W => KeyboardButton.W,
            Key.S => KeyboardButton.S,
            Key.A => KeyboardButton.A,
            Key.D => KeyboardButton.D,
            Key.Space => KeyboardButton.Space,
            Key.ShiftRight => KeyboardButton.Shift,
            Key.Tab => KeyboardButton.Tab,
            Key.Escape => KeyboardButton.Escape,
            Key.Enter => KeyboardButton.Enter,
            _ => KeyboardButton.Unknown,
        };

        IsInvoked = isPressed;
    }
}    
