using System.Diagnostics;
using System.Windows;
using desktop.ViewModels;
using Microsoft.VisualStudio.Threading;

namespace desktop.Services
{
    public static class CustomMessageBox
    {
        static JoinableTaskFactory taskFactory;
        static CustomMessageBox()
        {            
            taskFactory = new JoinableTaskFactory(new JoinableTaskContext());
        }

        public static bool? Show(string message)
        {
            try
            {
                CustomMessageBoxViewModel model = new CustomMessageBoxViewModel(message, MessageBoxType.Ok);
                CustomMessageBoxWindow messageBox = new CustomMessageBoxWindow()
                {
                    DataContext = model
                };
                model.SetWindow(messageBox);

                // Optional: set owner
                if (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) is Window owner)
                {
                    messageBox.Owner = owner;
                }

                bool? result = messageBox.ShowDialog();                

                return result;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        public static async Task<MessageBoxResult?> ShowAsync(String message)
        {
            try
            {
                return await taskFactory.RunAsync<MessageBoxResult?>(async () =>
                {
                    CustomMessageBoxViewModel model = new CustomMessageBoxViewModel(message, MessageBoxType.Ok);
                    CustomMessageBoxWindow messageBox = new CustomMessageBoxWindow()
                    {
                        DataContext = model
                    };
                    model.SetWindow(messageBox);

                    // Optional: set owner
                    if (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) is Window owner)
                    {
                        messageBox.Owner = owner;
                    }

                    messageBox.ShowDialog();

                    MessageBoxResult result = await model.Result;

                    return result;
                });
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
                throw;
            }
        }
    }
}