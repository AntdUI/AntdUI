// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Vanara.PInvoke
{
    public static partial class User32
    {
        /// <summary>
        /// <para>Retrieves the cursor position for the last message retrieved by the GetMessage function.</para>
        /// <para>To determine the current position of the cursor, use the GetCursorPos function.</para>
        /// </summary>
        /// <returns>
        /// <para>Type: <c>Type: <c>DWORD</c></c></para>
        /// <para>
        /// The return value specifies the x- and y-coordinates of the cursor position. The x-coordinate is the low order <c>short</c> and
        /// the y-coordinate is the high-order <c>short</c>.
        /// </para>
        /// </returns>
        /// <remarks>
        /// <para>
        /// As noted above, the x-coordinate is in the low-order <c>short</c> of the return value; the y-coordinate is in the high-order
        /// <c>short</c> (both represent signed values because they can take negative values on systems with multiple monitors). If the
        /// return value is assigned to a variable, you can use the MAKEPOINTS macro to obtain a POINTS structure from the return value. You
        /// can also use the GET_X_LPARAM or GET_Y_LPARAM macro to extract the x- or y-coordinate.
        /// </para>
        /// <para>
        /// <c>Important</c> Do not use the LOWORD or HIWORD macros to extract the x- and y- coordinates of the cursor position because these
        /// macros return incorrect results on systems with multiple monitors. Systems with multiple monitors can have negative x- and y-
        /// coordinates, and <c>LOWORD</c> and <c>HIWORD</c> treat the coordinates as unsigned quantities.
        /// </para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getmessagepos DWORD GetMessagePos( );
        [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
        public static extern uint GetMessagePos();

        /// <summary>
        /// <para>
        /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without
        /// waiting for the thread to process the message.
        /// </para>
        /// <para>To post a message in the message queue associated with a thread, use the PostThreadMessage function.</para>
        /// </summary>
        /// <param name="hWnd">
        /// <para>Type: <c>HWND</c></para>
        /// <para>A handle to the window whose window procedure is to receive the message. The following values have special meanings.</para>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>HWND_BROADCAST ((HWND)0xffff)</term>
        /// <term>
        /// The message is posted to all top-level windows in the system, including disabled or invisible unowned windows, overlapped
        /// windows, and pop-up windows. The message is not posted to child windows.
        /// </term>
        /// </item>
        /// <item>
        /// <term>NULL</term>
        /// <term>
        /// The function behaves like a call to PostThreadMessage with the dwThreadId parameter set to the identifier of the current thread.
        /// </term>
        /// </item>
        /// </list>
        /// <para>
        /// Starting with Windows Vista, message posting is subject to UIPI. The thread of a process can post messages only to message queues
        /// of threads in processes of lesser or equal integrity level.
        /// </para>
        /// </param>
        /// <param name="Msg">
        /// <para>Type: <c>UINT</c></para>
        /// <para>The message to be posted.</para>
        /// <para>For lists of the system-provided messages, see System-Defined Messages.</para>
        /// </param>
        /// <param name="wParam">
        /// <para>Type: <c>WPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <param name="lParam">
        /// <para>Type: <c>LPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>Type: <c>BOOL</c></c></para>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError. <c>GetLastError</c>
        /// returns <c>ERROR_NOT_ENOUGH_QUOTA</c> when the limit is hit.
        /// </para>
        /// </returns>
        /// <remarks>
        /// <para>When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).</para>
        /// <para>Messages in a message queue are retrieved by calls to the GetMessage or PeekMessage function.</para>
        /// <para>
        /// Applications that need to communicate using <c>HWND_BROADCAST</c> should use the RegisterWindowMessage function to obtain a
        /// unique message for inter-application communication.
        /// </para>
        /// <para>
        /// The system only does marshaling for system messages (those in the range 0 to (WM_USER-1)). To send other messages (those &gt;=
        /// <c>WM_USER</c>) to another process, you must do custom marshaling.
        /// </para>
        /// <para>
        /// If you send a message in the range below WM_USER to the asynchronous message functions ( <c>PostMessage</c>, SendNotifyMessage,
        /// and SendMessageCallback), its message parameters cannot include pointers. Otherwise, the operation will fail. The functions will
        /// return before the receiving thread has had a chance to process the message and the sender will free the memory before it is used.
        /// </para>
        /// <para>Do not post the WM_QUIT message using <c>PostMessage</c>; use the PostQuitMessage function.</para>
        /// <para>
        /// An accessibility application can use <c>PostMessage</c> to post WM_APPCOMMAND messages to the shell to launch applications. This
        /// functionality is not guaranteed to work for other types of applications.
        /// </para>
        /// <para>
        /// There is a limit of 10,000 posted messages per message queue. This limit should be sufficiently large. If your application
        /// exceeds the limit, it should be redesigned to avoid consuming so many system resources. To adjust this limit, modify the
        /// following registry key.
        /// </para>
        /// <para><c>HKEY_LOCAL_MACHINE</c><c>SOFTWARE</c><c>Microsoft</c><c>Windows NT</c><c>CurrentVersion</c><c>Windows</c><c>USERPostMessageLimit</c></para>
        /// <para>The minimum acceptable value is 4000.</para>
        /// <para>Examples</para>
        /// <para>
        /// The following example shows how to post a private window message using the <c>PostMessage</c> function. Assume you defined a
        /// private window message called <c>WM_COMPLETE</c>:
        /// </para>
        /// <para>You can post a message to the message queue associated with the thread that created the specified window as shown below:</para>
        /// <para>For more examples, see Initiating a Data Link.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-postmessagea BOOL PostMessageA( HWND hWnd, UINT Msg,
        // WPARAM wParam, LPARAM lParam );
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage([Optional] HWND hWnd, uint Msg, [Optional] IntPtr wParam, [Optional] IntPtr lParam);

        /// <summary>
        /// <para>
        /// Sends the specified message to a window or windows. The <c>SendMessage</c> function calls the window procedure for the specified
        /// window and does not return until the window procedure has processed the message.
        /// </para>
        /// <para>
        /// To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function. To post a message to a
        /// thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        /// </para>
        /// </summary>
        /// <param name="hWnd">
        /// <para>Type: <c>HWND</c></para>
        /// <para>
        /// A handle to the window whose window procedure will receive the message. If this parameter is <c>HWND_BROADCAST</c>
        /// ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows,
        /// overlapped windows, and pop-up windows; but the message is not sent to child windows.
        /// </para>
        /// <para>
        /// Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes of
        /// lesser or equal integrity level.
        /// </para>
        /// </param>
        /// <param name="msg">
        /// <para>Type: <c>UINT</c></para>
        /// <para>The message to be sent.</para>
        /// <para>For lists of the system-provided messages, see System-Defined Messages.</para>
        /// </param>
        /// <param name="wParam">
        /// <para>Type: <c>WPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <param name="lParam">
        /// <para>Type: <c>LPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>LRESULT</c></para>
        /// <para>The return value specifies the result of the message processing; it depends on the message sent.</para>
        /// </returns>
        /// <remarks>
        /// <para>When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).</para>
        /// <para>
        /// Applications that need to communicate using <c>HWND_BROADCAST</c> should use the RegisterWindowMessage function to obtain a
        /// unique message for inter-application communication.
        /// </para>
        /// <para>
        /// The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)). To send other messages (those &gt;=
        /// <c>WM_USER</c>) to another process, you must do custom marshalling.
        /// </para>
        /// <para>
        /// If the specified window was created by the calling thread, the window procedure is called immediately as a subroutine. If the
        /// specified window was created by a different thread, the system switches to that thread and calls the appropriate window
        /// procedure. Messages sent between threads are processed only when the receiving thread executes message retrieval code. The
        /// sending thread is blocked until the receiving thread processes the message. However, the sending thread will process incoming
        /// nonqueued messages while waiting for its message to be processed. To prevent this, use SendMessageTimeout with SMTO_BLOCK set.
        /// For more information on nonqueued messages, see Nonqueued Messages.
        /// </para>
        /// <para>
        /// An accessibility application can use <c>SendMessage</c> to send WM_APPCOMMAND messages to the shell to launch applications. This
        /// functionality is not guaranteed to work for other types of applications.
        /// </para>
        /// <para>Examples</para>
        /// <para>For an example, see Displaying Keyboard Input.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [System.Security.SecurityCritical]
        public static extern IntPtr SendMessage(HWND hWnd, uint msg, [In, Optional] IntPtr wParam, [In, Out, Optional] IntPtr lParam);

        /// <summary>
        /// <para>
        /// Sends the specified message to a window or windows. The <c>SendMessage</c> function calls the window procedure for the specified
        /// window and does not return until the window procedure has processed the message.
        /// </para>
        /// <para>
        /// To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function. To post a message to a
        /// thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        /// </para>
        /// </summary>
        /// <param name="hWnd">
        /// <para>Type: <c>HWND</c></para>
        /// <para>
        /// A handle to the window whose window procedure will receive the message. If this parameter is <c>HWND_BROADCAST</c>
        /// ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows,
        /// overlapped windows, and pop-up windows; but the message is not sent to child windows.
        /// </para>
        /// <para>
        /// Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes of
        /// lesser or equal integrity level.
        /// </para>
        /// </param>
        /// <param name="msg">
        /// <para>Type: <c>UINT</c></para>
        /// <para>The message to be sent.</para>
        /// <para>For lists of the system-provided messages, see System-Defined Messages.</para>
        /// </param>
        /// <param name="wParam">
        /// <para>Type: <c>WPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <param name="lParam">
        /// <para>Type: <c>LPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>LRESULT</c></para>
        /// <para>The return value specifies the result of the message processing; it depends on the message sent.</para>
        /// </returns>
        /// <remarks>
        /// <para>When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).</para>
        /// <para>
        /// Applications that need to communicate using <c>HWND_BROADCAST</c> should use the RegisterWindowMessage function to obtain a
        /// unique message for inter-application communication.
        /// </para>
        /// <para>
        /// The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)). To send other messages (those &gt;=
        /// <c>WM_USER</c>) to another process, you must do custom marshalling.
        /// </para>
        /// <para>
        /// If the specified window was created by the calling thread, the window procedure is called immediately as a subroutine. If the
        /// specified window was created by a different thread, the system switches to that thread and calls the appropriate window
        /// procedure. Messages sent between threads are processed only when the receiving thread executes message retrieval code. The
        /// sending thread is blocked until the receiving thread processes the message. However, the sending thread will process incoming
        /// nonqueued messages while waiting for its message to be processed. To prevent this, use SendMessageTimeout with SMTO_BLOCK set.
        /// For more information on nonqueued messages, see Nonqueued Messages.
        /// </para>
        /// <para>
        /// An accessibility application can use <c>SendMessage</c> to send WM_APPCOMMAND messages to the shell to launch applications. This
        /// functionality is not guaranteed to work for other types of applications.
        /// </para>
        /// <para>Examples</para>
        /// <para>For an example, see Displaying Keyboard Input.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [System.Security.SecurityCritical]
        public static extern IntPtr SendMessage(HWND hWnd, uint msg, [In, Optional] IntPtr wParam, string lParam);

        /// <summary>
        /// <para>
        /// Sends the specified message to a window or windows. The <c>SendMessage</c> function calls the window procedure for the specified
        /// window and does not return until the window procedure has processed the message.
        /// </para>
        /// <para>
        /// To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function. To post a message to a
        /// thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        /// </para>
        /// </summary>
        /// <param name="hWnd">
        /// <para>Type: <c>HWND</c></para>
        /// <para>
        /// A handle to the window whose window procedure will receive the message. If this parameter is <c>HWND_BROADCAST</c>
        /// ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows,
        /// overlapped windows, and pop-up windows; but the message is not sent to child windows.
        /// </para>
        /// <para>
        /// Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes of
        /// lesser or equal integrity level.
        /// </para>
        /// </param>
        /// <param name="msg">
        /// <para>Type: <c>UINT</c></para>
        /// <para>The message to be sent.</para>
        /// <para>For lists of the system-provided messages, see System-Defined Messages.</para>
        /// </param>
        /// <param name="wParam">
        /// <para>Type: <c>WPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <param name="lParam">
        /// <para>Type: <c>LPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>LRESULT</c></para>
        /// <para>The return value specifies the result of the message processing; it depends on the message sent.</para>
        /// </returns>
        /// <remarks>
        /// <para>When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).</para>
        /// <para>
        /// Applications that need to communicate using <c>HWND_BROADCAST</c> should use the RegisterWindowMessage function to obtain a
        /// unique message for inter-application communication.
        /// </para>
        /// <para>
        /// The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)). To send other messages (those &gt;=
        /// <c>WM_USER</c>) to another process, you must do custom marshalling.
        /// </para>
        /// <para>
        /// If the specified window was created by the calling thread, the window procedure is called immediately as a subroutine. If the
        /// specified window was created by a different thread, the system switches to that thread and calls the appropriate window
        /// procedure. Messages sent between threads are processed only when the receiving thread executes message retrieval code. The
        /// sending thread is blocked until the receiving thread processes the message. However, the sending thread will process incoming
        /// nonqueued messages while waiting for its message to be processed. To prevent this, use SendMessageTimeout with SMTO_BLOCK set.
        /// For more information on nonqueued messages, see Nonqueued Messages.
        /// </para>
        /// <para>
        /// An accessibility application can use <c>SendMessage</c> to send WM_APPCOMMAND messages to the shell to launch applications. This
        /// functionality is not guaranteed to work for other types of applications.
        /// </para>
        /// <para>Examples</para>
        /// <para>For an example, see Displaying Keyboard Input.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [System.Security.SecurityCritical]
        public static extern IntPtr SendMessage(HWND hWnd, uint msg, ref int wParam, [In, Out] StringBuilder lParam);

        /// <summary>
        /// <para>
        /// Sends the specified message to a window or windows. The <c>SendMessage</c> function calls the window procedure for the specified
        /// window and does not return until the window procedure has processed the message.
        /// </para>
        /// <para>
        /// To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function. To post a message to a
        /// thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        /// </para>
        /// </summary>
        /// <param name="hWnd">
        /// <para>Type: <c>HWND</c></para>
        /// <para>
        /// A handle to the window whose window procedure will receive the message. If this parameter is <c>HWND_BROADCAST</c>
        /// ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows,
        /// overlapped windows, and pop-up windows; but the message is not sent to child windows.
        /// </para>
        /// <para>
        /// Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes of
        /// lesser or equal integrity level.
        /// </para>
        /// </param>
        /// <param name="msg">
        /// <para>Type: <c>UINT</c></para>
        /// <para>The message to be sent.</para>
        /// <para>For lists of the system-provided messages, see System-Defined Messages.</para>
        /// </param>
        /// <param name="wParam">
        /// <para>Type: <c>WPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <param name="lParam">
        /// <para>Type: <c>LPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>LRESULT</c></para>
        /// <para>The return value specifies the result of the message processing; it depends on the message sent.</para>
        /// </returns>
        /// <remarks>
        /// <para>When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).</para>
        /// <para>
        /// Applications that need to communicate using <c>HWND_BROADCAST</c> should use the RegisterWindowMessage function to obtain a
        /// unique message for inter-application communication.
        /// </para>
        /// <para>
        /// The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)). To send other messages (those &gt;=
        /// <c>WM_USER</c>) to another process, you must do custom marshalling.
        /// </para>
        /// <para>
        /// If the specified window was created by the calling thread, the window procedure is called immediately as a subroutine. If the
        /// specified window was created by a different thread, the system switches to that thread and calls the appropriate window
        /// procedure. Messages sent between threads are processed only when the receiving thread executes message retrieval code. The
        /// sending thread is blocked until the receiving thread processes the message. However, the sending thread will process incoming
        /// nonqueued messages while waiting for its message to be processed. To prevent this, use SendMessageTimeout with SMTO_BLOCK set.
        /// For more information on nonqueued messages, see Nonqueued Messages.
        /// </para>
        /// <para>
        /// An accessibility application can use <c>SendMessage</c> to send WM_APPCOMMAND messages to the shell to launch applications. This
        /// functionality is not guaranteed to work for other types of applications.
        /// </para>
        /// <para>Examples</para>
        /// <para>For an example, see Displaying Keyboard Input.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [System.Security.SecurityCritical]
        public static extern IntPtr SendMessage(HWND hWnd, uint msg, [In, Optional] IntPtr wParam, [In, Out] StringBuilder lParam);

        /// <summary>
        /// <para>
        /// Sends the specified message to a window or windows. The <c>SendMessage</c> function calls the window procedure for the specified
        /// window and does not return until the window procedure has processed the message.
        /// </para>
        /// <para>
        /// To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function. To post a message to a
        /// thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        /// </para>
        /// </summary>
        /// <typeparam name="TMsg">The type of the message. This can be any type that converts to <see cref="uint"/>.</typeparam>
        /// <param name="hWnd"><para>Type: <c>HWND</c></para>
        /// <para>
        /// A handle to the window whose window procedure will receive the message. If this parameter is <c>HWND_BROADCAST</c>
        /// ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows,
        /// overlapped windows, and pop-up windows; but the message is not sent to child windows.
        /// </para>
        /// <para>
        /// Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes of
        /// lesser or equal integrity level.
        /// </para></param>
        /// <param name="msg"><para>Type: <c>UINT</c></para>
        /// <para>The message to be sent.</para>
        /// <para>For lists of the system-provided messages, see System-Defined Messages.</para></param>
        /// <param name="wParam"><para>Type: <c>WPARAM</c></para>
        /// <para>Additional message-specific information.</para></param>
        /// <param name="lParam"><para>Type: <c>LPARAM</c></para>
        /// <para>Additional message-specific information.</para></param>
        /// <returns>
        /// <para>Type: <c>LRESULT</c></para>
        /// <para>The return value specifies the result of the message processing; it depends on the message sent.</para>
        /// </returns>
        /// <remarks>
        /// <para>When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).</para>
        /// <para>
        /// Applications that need to communicate using <c>HWND_BROADCAST</c> should use the RegisterWindowMessage function to obtain a
        /// unique message for inter-application communication.
        /// </para>
        /// <para>
        /// The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)). To send other messages (those &gt;=
        /// <c>WM_USER</c>) to another process, you must do custom marshalling.
        /// </para>
        /// <para>
        /// If the specified window was created by the calling thread, the window procedure is called immediately as a subroutine. If the
        /// specified window was created by a different thread, the system switches to that thread and calls the appropriate window
        /// procedure. Messages sent between threads are processed only when the receiving thread executes message retrieval code. The
        /// sending thread is blocked until the receiving thread processes the message. However, the sending thread will process incoming
        /// nonqueued messages while waiting for its message to be processed. To prevent this, use SendMessageTimeout with SMTO_BLOCK set.
        /// For more information on nonqueued messages, see Nonqueued Messages.
        /// </para>
        /// <para>
        /// An accessibility application can use <c>SendMessage</c> to send WM_APPCOMMAND messages to the shell to launch applications. This
        /// functionality is not guaranteed to work for other types of applications.
        /// </para>
        /// <para>Examples</para>
        /// <para>For an example, see Displaying Keyboard Input.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
        public static IntPtr SendMessage<TMsg>(HWND hWnd, TMsg msg, [Optional] IntPtr wParam, [Optional] IntPtr lParam)
            where TMsg : struct, IConvertible
            => SendMessage(hWnd, Convert.ToUInt32(msg), IntPtr.Zero, IntPtr.Zero);

        /// <summary>
        /// <para>
        /// Sends the specified message to a window or windows. The <c>SendMessage</c> function calls the window procedure for the specified
        /// window and does not return until the window procedure has processed the message.
        /// </para>
        /// <para>
        /// To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function. To post a message to a
        /// thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        /// </para>
        /// </summary>
        /// <typeparam name="TMsg">The type of the message. This can be any type that converts to <see cref="uint"/>.</typeparam>
        /// <typeparam name="TWP">The type of the <paramref name="wParam"/> parameter. This can be any type that converts to <see cref="long" />.</typeparam>
        /// <param name="hWnd">
        /// <para>Type: <c>HWND</c></para>
        /// <para>
        /// A handle to the window whose window procedure will receive the message. If this parameter is <c>HWND_BROADCAST</c>
        /// ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows,
        /// overlapped windows, and pop-up windows; but the message is not sent to child windows.
        /// </para>
        /// <para>
        /// Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes of
        /// lesser or equal integrity level.
        /// </para>
        /// </param>
        /// <param name="msg">
        /// <para>Type: <c>UINT</c></para>
        /// <para>The message to be sent.</para>
        /// <para>For lists of the system-provided messages, see System-Defined Messages.</para>
        /// </param>
        /// <param name="wParam">
        /// <para>Type: <c>WPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <param name="lParam">
        /// <para>Type: <c>LPARAM</c></para>
        /// <para>Additional message-specific information.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>LRESULT</c></para>
        /// <para>The return value specifies the result of the message processing; it depends on the message sent.</para>
        /// </returns>
        /// <remarks>
        /// <para>When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).</para>
        /// <para>
        /// Applications that need to communicate using <c>HWND_BROADCAST</c> should use the RegisterWindowMessage function to obtain a
        /// unique message for inter-application communication.
        /// </para>
        /// <para>
        /// The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)). To send other messages (those &gt;=
        /// <c>WM_USER</c>) to another process, you must do custom marshalling.
        /// </para>
        /// <para>
        /// If the specified window was created by the calling thread, the window procedure is called immediately as a subroutine. If the
        /// specified window was created by a different thread, the system switches to that thread and calls the appropriate window
        /// procedure. Messages sent between threads are processed only when the receiving thread executes message retrieval code. The
        /// sending thread is blocked until the receiving thread processes the message. However, the sending thread will process incoming
        /// nonqueued messages while waiting for its message to be processed. To prevent this, use SendMessageTimeout with SMTO_BLOCK set.
        /// For more information on nonqueued messages, see Nonqueued Messages.
        /// </para>
        /// <para>
        /// An accessibility application can use <c>SendMessage</c> to send WM_APPCOMMAND messages to the shell to launch applications. This
        /// functionality is not guaranteed to work for other types of applications.
        /// </para>
        /// <para>Examples</para>
        /// <para>For an example, see Displaying Keyboard Input.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
        public static IntPtr SendMessage<TMsg, TWP>(HWND hWnd, TMsg msg, TWP wParam, [Optional] IntPtr lParam)
            where TMsg : struct, IConvertible where TWP : struct, IConvertible
            => SendMessage(hWnd, Convert.ToUInt32(msg), (IntPtr)Convert.ToInt64(wParam), IntPtr.Zero);
    }
}