using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Intefaces
{


    public interface IUpdateService
    {
        Task<Result<UpdateInfo>> CheckNewVersion();
    }
}
