using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mobatako.model;
using mobatako.utils.network;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using mobatako.common;

namespace mobatako.utils.network
{
    class RestProcessor
    {
        //API実行用
        HttpClient _client = new HttpClient();


        //サーバー疎通確認API実行
        public async Task<bool> ServerIsAlive()
        {
            var reqSrv = new HttpRequestMessage(HttpMethod.Get, Strings.WEB_API_BASE_URL + "v1");
            reqSrv.Headers.Add("ContentType", "application/json");
            reqSrv.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            var resSrv = await _client.SendAsync(reqSrv);
            return resSrv.IsSuccessStatusCode;
        }
    }
}
