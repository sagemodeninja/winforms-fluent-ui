using System;

namespace WinForms.Fluent.UI.Utilities.Enums
{
    [Flags]
    public enum SetWindowPos : uint
    {
        NoSize = 0x0001,
        NoMove = 0x0002,
        NoZOrder = 0x0004,
        NoRedraw = 0x0008,
        NoActivate = 0x0010,
        DrawFrame = 0x0020,
        FrameChanged = 0x0020,
        ShowWindow = 0x0040,
        HideWindow = 0x0080,
        NoCopyBits = 0x0100,
        NoOwnerZOrder = 0x0200,
        NoReposition = 0x0200,
        NoSendChanging = 0x0400,
        DeferErase = 0x2000,
        AsyncWindowPos = 0x4000
    }
}