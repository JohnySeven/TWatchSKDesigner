using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Intefaces;
using TWatchSKDesigner.Modals;

namespace TWatchSKDesigner.Models
{
    public class ProgressWindowTaskMonitor : ITaskStatusMonitor
    {
        ProgressWindow? current = null;

        public ProgressWindowTaskMonitor()
        {

        }

        private ProgressWindowTaskMonitor(ProgressWindow window)
        {
            current = window;
        }

        public Task Run(Func<Task> func)
        {
            return ProgressWindow.ShowProgress(async (pw) =>
            {
                current = pw;
                await func();
                current = null;
            });
        }

        public ITaskStatusMonitor CreateSubTask()
        {
            return new ProgressWindowTaskMonitor(current ?? throw new InvalidOperationException());
        }

        public void OnCompleted()
        {
            current?.Update("Done!");
        }

        public void OnError(string message)
        {
            current?.Update($"Error: " + message);
        }

        public void OnFail(string message)
        {
            current?.Update($"Fail: " + message);
        }

        public void OnFail(Exception ex)
        {
            current?.Update($"Error: " + ex.Message);
        }

        public void OnProgress(string message)
        {
            current?.Update(message);
        }

        public void OnStarted()
        {
            
        }
    }
}
