using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Helpers
{
    public static class GithubHelper
    {
        private static Lazy<GitHubClient> Client = new Lazy<GitHubClient>(() =>new GitHubClient(new ProductHeaderValue("TWatchSK-Designer")));
        private static Lazy<HttpClient> Http = new Lazy<HttpClient>();
        /// <summary>
        /// Returns specific release from repository.
        /// </summary>
        /// <param name="owner">Owner of Github repository.</param>
        /// <param name="repository">Name of repository.</param>
        /// <param name="whereFunc">If null it will return latest release, if specified it will select first match.</param>
        /// <returns>Release instance or null</returns>
        public static async Task<Release?> GetRelease(string owner, string repository, Func<Release, bool> whereFunc = null)
        {
            var releases = await GetReleases(owner, repository);

            if (whereFunc != null)
            {
                return releases.OrderByDescending(r => r.CreatedAt)
                               .Where(whereFunc)
                               .FirstOrDefault();
            }
            else
            {
                return releases.OrderByDescending(r => r.CreatedAt)
                              .FirstOrDefault();
            }
        }

        internal static async Task<bool> DownloadFile(string url, string path, Intefaces.ITaskStatusMonitor statusMonitor)
        {
            var ret = false;
            var http = Http.Value;

            try
            {
                using (var outputFile = File.OpenWrite(path))
                {
                    using (var downloadStream = await http.GetStreamAsync(url))
                    {
                        await downloadStream.CopyToAsync(outputFile);
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                statusMonitor.OnFail(ex);
            }

            return ret;
        }

        /// <summary>
        /// Returns a list of releases
        /// </summary>
        /// <param name="owner">Owner of Github repository.</param>
        /// <param name="repository">Name of repository.</param>
        /// <returns></returns>
        public static Task<IReadOnlyList<Release>> GetReleases(string owner, string repository)
        {
            var client = Client.Value;

            return client.Repository.Release.GetAll(owner, repository);
        }

        internal static async Task<bool> DownloadAsset(ReleaseAsset asset, Intefaces.ITaskStatusMonitor statusMonitor, string path)
        {
            var ret = false;
            var http = Http.Value;

            try
            {
                using (var outputFile = File.OpenWrite(path))
                {
                    using (var downloadStream = await http.GetStreamAsync(asset.BrowserDownloadUrl))
                    {
                        await downloadStream.CopyToAsync(outputFile);
                        ret = true;

                    }
                }
            }
            catch (Exception ex)
            {
                statusMonitor.OnFail(ex);
            }

            return ret;
        }
    }
}
