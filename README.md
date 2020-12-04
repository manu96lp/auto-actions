[![.NET Framework](https://badgen.net/badge/Powered%20by/.NET/6134eb)](https://www.microsoft.com/es-es/download/details.aspx?id=55170)

# [C# | .NET] Auto Actions

Auto Actions is a simple Windows Console Application that lets you execute a desired set of previously defined actions periodically just like a script. You can build scripts in a way that lets you simulate key presses, mouse clicks, etc.

## Keyboard example

This is an example that simulates a message typing every minute.

```
keyboard press VK_M
keyboard press VK_I
keyboard press VK_N
keyboard press VK_U
keyboard press VK_T
keyboard press VK_E

keyboard press RETURN

wait 60000
```

## Mouse example

This is an example that simulates a 3 seconds long mouse click every twenty seconds.

```
mouse hold left

wait 3000

mouse release left

wait 17000
```

## Key Codes

```
LBUTTON
RBUTTON
CANCEL
MBUTTON
XBUTTON1
XBUTTON2
BACK
TAB
CLEAR
RETURN
SHIFT
CONTROL
MENU
PAUSE
CAPITAL
HANGUL
HANGUL
HANGUL
JUNJA
FINAL
HANJA
HANJA
ESCAPE
CONVERT
NONCONVERT
ACCEPT
MODECHANGE
SPACE
PRIOR
NEXT
END
HOME
LEFT
UP
RIGHT
DOWN
SELECT
PRINT
EXECUTE
SNAPSHOT
INSERT
DELETE
HELP
VK_0
VK_1
VK_2
VK_3
VK_4
VK_5
VK_6
VK_7
VK_8
VK_9
VK_A
VK_B
VK_C
VK_D
VK_E
VK_F
VK_G
VK_H
VK_I
VK_J
VK_K
VK_L
VK_M
VK_N
VK_O
VK_P
VK_Q
VK_R
VK_S
VK_T
VK_U
VK_V
VK_W
VK_X
VK_Y
VK_Z
LWIN
RWIN
APPS
SLEEP
NUMPAD0
NUMPAD1
NUMPAD2
NUMPAD3
NUMPAD4
NUMPAD5
NUMPAD6
NUMPAD7
NUMPAD8
NUMPAD9
MULTIPLY
ADD
SEPARATOR
SUBTRACT
DECIMAL
DIVIDE
F1
F2
F3
F4
F5
F6
F7
F8
F9
F10
F11
F12
F13
F14
F15
F16
F17
F18
F19
F20
F21
F22
F23
F24
NUMLOCK
SCROLL
LSHIFT
RSHIFT
LCONTROL
RCONTROL
LMENU
RMENU
BROWSER_BACK
BROWSER_FORWARD
BROWSER_REFRESH
BROWSER_STOP
BROWSER_SEARCH
BROWSER_FAVORITES
BROWSER_HOME
VOLUME_MUTE
VOLUME_DOWN
VOLUME_UP
MEDIA_NEXT_TRACK
MEDIA_PREV_TRACK
MEDIA_STOP
MEDIA_PLAY_PAUSE
LAUNCH_MAIL
LAUNCH_MEDIA_SELECT
LAUNCH_APP1
LAUNCH_APP2
OEM_1
OEM_PLUS
OEM_COMMA
OEM_MINUS
OEM_PERIOD
OEM_2
OEM_3
OEM_4
OEM_5
OEM_6
OEM_7
OEM_8
OEM_102
PROCESSKEY
PACKET
ATTN
CRSEL
EXSEL
EREOF
PLAY
ZOOM
NONAME
PA1
OEM_CLEAR
```