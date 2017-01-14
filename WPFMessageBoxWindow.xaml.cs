using System;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace MessageBoxUtils
{
    /// <summary>
    ///     Interaction logic for WPFMessageBoxWindow.xaml
    /// </summary>
    public partial class WPFMessageBoxWindow
    {
        [ThreadStatic] private static WPFMessageBoxWindow _messageBoxWindow;

        private MessageBoxViewModel _viewModel;

        public WPFMessageBoxWindow()
        {
            InitializeComponent();
        }

        public static MessageBoxResult Show(
            Action<Window> setOwner,
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult,
            MessageBoxOptions options,
            int timeout)
        {
            if ((options & MessageBoxOptions.DefaultDesktopOnly) == MessageBoxOptions.DefaultDesktopOnly)
            {
            }

            if ((options & MessageBoxOptions.ServiceNotification) == MessageBoxOptions.ServiceNotification)
            {
            }

            _messageBoxWindow = new WPFMessageBoxWindow();

            setOwner(_messageBoxWindow);

            PlayMessageBeep(icon);

            _messageBoxWindow._viewModel = new MessageBoxViewModel(_messageBoxWindow, caption, messageBoxText, button,
                icon, defaultResult, options, timeout);
            _messageBoxWindow.DataContext = _messageBoxWindow._viewModel;
            _messageBoxWindow.ShowDialog();
            return _messageBoxWindow._viewModel.Result;
        }

        private static void PlayMessageBeep(MessageBoxImage icon)
        {
            switch (icon)
            {
                //case MessageBoxImage.Hand:
                //case MessageBoxImage.Stop:
                case MessageBoxImage.Error:
                    SystemSounds.Hand.Play();
                    break;

                //case MessageBoxImage.Exclamation:
                case MessageBoxImage.Warning:
                    SystemSounds.Exclamation.Play();
                    break;

                case MessageBoxImage.Question:
                    SystemSounds.Question.Play();
                    break;

                //case MessageBoxImage.Asterisk:
                case MessageBoxImage.Information:
                    SystemSounds.Asterisk.Play();
                    break;

                default:
                    SystemSounds.Beep.Play();
                    break;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            // removes the application icon from the window top left corner
            // this is different than just hiding it
            WindowHelper.RemoveIcon(this);

            switch (_viewModel.Options)
            {
                case MessageBoxOptions.None:
                    break;

                case MessageBoxOptions.RightAlign:
                    WindowHelper.SetRightAligned(this);
                    break;

                case MessageBoxOptions.RtlReading:
                    break;

                case MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading:
                    break;
            }

            // disable close button if needed and remove resize menu items from the window system menu
            var systemMenuHelper = new SystemMenuHelper(this);

            if (_viewModel.ButtonOption == MessageBoxButton.YesNo)
                systemMenuHelper.DisableCloseButton = true;

            systemMenuHelper.RemoveResizeMenu = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                _viewModel.EscapeCommand.Execute(null);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _viewModel.CloseCommand.Execute(null);
        }
    }
}