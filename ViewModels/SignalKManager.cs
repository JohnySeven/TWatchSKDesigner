﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Models.SK;

namespace TWatchSKDesigner.ViewModels
{
    public class SignalKManager : ViewModelBase
    {
        private string? _token = "";

        public bool TokenIsPresent => !string.IsNullOrEmpty(_token);

        private string? _ServerAddress;

        public string? ServerAddress
        {
            get { return _ServerAddress; }
            set { _ServerAddress = value; OnPropertyChanged(nameof(ServerAddress)); }
        }

        private List<SKPath> _skPaths = null;

        public SKPath[] GetSignalKPaths()
        {
            return _skPaths?.ToArray() ?? Array.Empty<SKPath>();
        }


        private Lazy<HttpClient> client = new Lazy<HttpClient>(InitHttpClient);
        

        public async Task<OperationResult> Authorize(string server, string user, string password)
        {
            var http = client.Value;

            try
            {

                var requestJson = JsonConvert.SerializeObject(new { username = user, password = password });
                var response = await http.PostAsync($"http://{server}/signalk/v1/auth/login", new Models.SK.JsonContent(requestJson));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var tokenInfoJson = await response.Content.ReadAsStringAsync();

                    var tokenInfo = JsonConvert.DeserializeObject<AuthorizationResult>(tokenInfoJson);
                    _token = tokenInfo?.Token;
                    ServerAddress = server;
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                    OnPropertyChanged(nameof(tokenInfo));

                    return new OperationResult();
                }
                else
                {
                    return new OperationResult()
                    {
                        ErrorMessage = response.ReasonPhrase ?? "Connection failed!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<OperationResult> LoadSKPaths()
        {
            var http = client.Value;
            var uri = $"http://{_ServerAddress}/signalk/v1/api/";

            try
            {
                var response = await http.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jApiData = JObject.Parse(json);
                    var self = jApiData["self"]?.ToString();
                    if (self != null)
                    {
                        self = self.Substring("vessels.".Length);
                        var paths = new List<SKPath>();
                        var jSelf = jApiData["vessels"][self] as JObject;
                        if(jSelf != null)
                        {
                            foreach (var name in jSelf)
                            {
                                if (name.Value?.Type == JTokenType.Object)
                                {
                                    EnumeratePaths((JObject)name.Value, "", paths);
                                }
                            }

                            _skPaths = paths;

                            return new OperationResult();
                        }
                        else
                        {
                            return new OperationResult("Unable to find self in vessels object!");
                        }
                    }
                    else
                    {
                        return new OperationResult("Token self is expected, but not found!");
                    }
                }
                else
                {
                    return new OperationResult(response.ReasonPhrase ?? "Error!");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult(ex.Message);
            }
        }

        private void EnumeratePaths(JObject path, string currentPath, List<SKPath> paths)
        {
            if(path.ContainsKey("value"))
            {
                paths.Add(new SKPath(currentPath)
                {
                    Source = path["$source"]?.ToString() ?? "",
                    Meta = path["meta"] as JObject
                });
            }
            else
            {
                foreach(var key in path)
                {
                    if(key.Value?.Type == JTokenType.Object)
                    {
                        EnumeratePaths((JObject)key.Value, currentPath == "" ? key.Key : currentPath + "." + key.Key, paths);
                    }
                }
            }
        }

        public async Task<OperationResult<JObject>> DownloadView()
        {
            var http = client.Value;
            var uri = $"http://{_ServerAddress}/signalk/v1/applicationData/global/twatch/1.0/ui";
            try
            {
                var response = await http.GetAsync(uri);

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(responseJson);

                    if (jObject.ContainsKey("default"))
                    {
                        return new OperationResult<JObject>((JObject)jObject["default"] ?? throw new Exception("Invalid response!"));
                    }
                    else
                    {
                        return new OperationResult<JObject>(new JObject());
                    }
                }
                else
                {
                    return new OperationResult<JObject>(response.ReasonPhrase ?? "Request error!");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<JObject>(ex.Message);
            }
        }

        public Task<OperationResult> StoreView(JObject viewDefinition)
        {
            throw new NotImplementedException();
        }

        private static HttpClient InitHttpClient()
        {
            var ret = new HttpClient();

            ret.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return ret;
        }
    }
}