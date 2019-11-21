namespace Logger_Viewer
{
    public static class MessageWriter
    {
        private static LoggerViewerForm formInstance = null;
        public delegate void MessageWriterFormClosed();
        private static MessageWriterFormClosed callbackToBeCalled = null;

        public static void InitViewer(MessageWriterFormClosed callback)
        {
			formInstance = new LoggerViewerForm();
			formInstance.MyFormClosed += FormInstance_FormClosed;
            callbackToBeCalled = callback;
        }

        private static void FormInstance_FormClosed()
        {
            if (callbackToBeCalled != null)
                callbackToBeCalled.Invoke();
        }

        public static void LogMessage(string message, bool overrideLastLine = false)
        {
            if (formInstance != null && !formInstance.IsDisposed)
                formInstance.AppendText(message, overrideLastLine);
        }

        public static void ShowDebugger()
        {
            if (formInstance != null && !formInstance.IsDisposed)
                formInstance.Show();
        }

        public static void HideDebugger()
        {
            if (formInstance != null && !formInstance.IsDisposed)
                formInstance.Hide();
        }
    }
}
