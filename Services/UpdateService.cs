using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Helpers;
using TWatchSKDesigner.Intefaces;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Services
{
    public class UpdateService : IUpdateService
    {
        public async Task<Result<UpdateInfo>> CheckNewVersion()
        {
            Result<UpdateInfo> ret = null;

            try
            {
                var assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;
                var currentVersion = $"v{assemblyVersion.Major}.{assemblyVersion.Minor}";
                var availableReleases = await GithubHelper.GetReleases("JohnySeven", "TWatchSKDesigner");

                foreach(var release in availableReleases)
                {
                    var releaseVersion = release.TagName;

                    if(string.Compare(releaseVersion,currentVersion) > 0)
                    {
                        ret = new Result<UpdateInfo>()
                        {
                            IsSuccess = true,
                            Code = "OK",
                            Data = new UpdateInfo()
                            {
                                IsNewVersion = true,
                                Version = releaseVersion,
                                ReleaseNotes = release.Body,
                                ShowUri = new Uri(release.HtmlUrl)
                            }
                        };

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ret = new Result<UpdateInfo>()
                {
                    Code = "ERROR",
                    ErrorMessage = ex.Message
                };
            }

            return ret;
        }
    }
}
