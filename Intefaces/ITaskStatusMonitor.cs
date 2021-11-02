using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Intefaces
{
    public interface ITaskStatusMonitor
    {
        void OnStarted();
        void OnProgress(string message);
        void OnCompleted();
        void OnError(string message);
        void OnFail(string message);
        void OnFail(Exception ex);

        ITaskStatusMonitor CreateSubTask();
    }
}
