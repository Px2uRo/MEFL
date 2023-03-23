﻿using System.Text;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net;

namespace CoreLaunching.MicrosoftAuth
{
    public class MSAuthAccount
    {
        #region 常用的
        private const string _clientId = "328557da-2a39-4e7b-b0ee-7595406faba6";
        private const string _authority = "https://login.microsoftonline.com/consumers";
        private static IPublicClientApplication app = PublicClientApplicationBuilder.Create(_clientId)
                                                    .WithAuthority(_authority)
                                                    .WithDefaultRedirectUri()
                                                    .Build();
        private static string[] scopes = new[] { "xboxlive.signin" };
        public static string MinecraftAppUrl = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize?prompt=login&client_id=00000000402b5328&response_type=code&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL&redirect_uri=https:%2F%2Flogin.live.com%2Foauth20_desktop.srf";
        #endregion
        private static MSAPlayerInfoWithRefreshToken GetInfoFromToken(string token)
        {
            #region XBL(XboxLive) 验证
            var Content = "{\"Properties\": {\"AuthMethod\": \"RPS\",\"SiteName\": \"user.auth.xboxlive.com\",\"RpsTicket\": \"" + token + "\"},\"RelyingParty\": \"http://auth.xboxlive.com\",\"TokenType\": \"JWT\"}";
            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(@"https://user.auth.xboxlive.com/user/authenticate");
            request2.Method = "POST";
            byte[] byteArray2 = Encoding.UTF8.GetBytes(Content);
            request2.ContentLength = byteArray2.Length;
            request2.ContentType = "application/json";
            request2.Accept = "application/json";
            Stream dataStream2 = request2.GetRequestStream();
            dataStream2.Write(byteArray2, 0, byteArray2.Length);
            dataStream2.Close();
            WebResponse response2 = request2.GetResponse();
            dataStream2 = response2.GetResponseStream();
            StreamReader reader2 = new StreamReader(dataStream2);
            var responseFromServer = reader2.ReadToEnd();
            var XBLRoot = JsonConvert.DeserializeObject<XBLResponse>(responseFromServer);
            #endregion
            #region XSTS 验证
            Content = "{ \"Properties\": {\"SandboxId\": \"RETAIL\",\"UserTokens\":[\"" + XBLRoot.Token + "\"]},\"RelyingParty\": \"rp://api.minecraftservices.com/\",\"TokenType\": \"JWT\"}";
            HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(@"https://xsts.auth.xboxlive.com/xsts/authorize");
            request3.Method = "POST";
            byte[] byteArray3 = Encoding.UTF8.GetBytes(Content);
            request3.ContentLength = byteArray3.Length;
            request3.ContentType = "application/json";
            request3.Accept = "application/json";
            Stream dataStream3 = request3.GetRequestStream();
            dataStream3.Write(byteArray3, 0, byteArray3.Length);
            dataStream3.Close();
            WebResponse response3 = request3.GetResponse();
            dataStream3 = response3.GetResponseStream();
            StreamReader reader3 = new StreamReader(dataStream3);
            responseFromServer = reader3.ReadToEnd();
            var XSTSRoot = JsonConvert.DeserializeObject<XBLResponse>(responseFromServer);
            #endregion
            #region 登录 Minecraft
            Content = "{\"identityToken\":\"XBL3.0 x=" + XSTSRoot.DisplayClaims.Xui[0].Uhs + ";" + XSTSRoot.Token + "\"}";
            HttpWebRequest request4 = (HttpWebRequest)WebRequest.Create(@"https://api.minecraftservices.com/authentication/login_with_xbox");
            request4.Method = "POST";
            byte[] byteArray4 = Encoding.UTF8.GetBytes(Content);
            request4.ContentLength = byteArray4.Length;
            request4.ContentType = "application/json";
            request4.Accept = "application/json";
            Stream dataStream4 = request4.GetRequestStream();
            dataStream4.Write(byteArray4, 0, byteArray4.Length);
            dataStream4.Close();
            WebResponse response4 = request4.GetResponse();
            dataStream4 = response4.GetResponseStream();
            StreamReader reader4 = new StreamReader(dataStream4);
            responseFromServer = reader4.ReadToEnd();
            var login_with_xboxResponseRoot = JsonConvert.DeserializeObject<Login_With_XboxResponse>(responseFromServer);
            #endregion
            #region 检查游戏所有权
            HttpWebRequest request5 = (HttpWebRequest)WebRequest.Create(@"https://api.minecraftservices.com/entitlements/mcstore");
            request5.Method = "GET";
            request5.Headers.Add("Authorization: Bearer " + login_with_xboxResponseRoot.AccessToken);
            WebResponse response5 = request5.GetResponse();
            var dataStream5 = response5.GetResponseStream();
            StreamReader reader5 = new StreamReader(dataStream5);
            responseFromServer = reader5.ReadToEnd();
            var OwnershipRoot = JsonConvert.DeserializeObject<Ownership>(responseFromServer);
            #endregion
            #region 获取个人资料
            if (OwnershipRoot.Items.Count != 0)
            {
                HttpWebRequest request6 = (HttpWebRequest)WebRequest.Create(@"https://api.minecraftservices.com/minecraft/profile");
                request6.Method = "GET";
                request6.Headers.Add("Authorization: Bearer " + login_with_xboxResponseRoot.AccessToken);
                WebResponse response6 = request6.GetResponse();
                var dataStream6 = response6.GetResponseStream();
                StreamReader reader6 = new StreamReader(dataStream6);
                responseFromServer = reader6.ReadToEnd();
                MSAPlayerInfoWithRefreshToken playerInfoRoot = JsonConvert.DeserializeObject<MSAPlayerInfoWithRefreshToken>(responseFromServer);
                playerInfoRoot.AccessToken = login_with_xboxResponseRoot.AccessToken;
                return playerInfoRoot;
            }
            else
            {
                return null;
            }
            #endregion
        }
        public static MSAPlayerInfoWithRefreshToken GetInfoWithRefreshTokenFromCode(string code)
        {
            string ref_token = "";
            string token = "";
            #region 微软登录
            string Content = "client_id=00000000402b5328" +
            "&code=" + code.Replace("https://login.live.com/oauth20_desktop.srf?code=", "") +
            "&grant_type=authorization_code" +
            "&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf" +
            "&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://login.live.com/oauth20_token.srf");
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(Content);
            request.ContentLength = byteArray.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            var MSARoot = JsonConvert.DeserializeObject<MSAuthResponse>(responseFromServer);
            ref_token = MSARoot.RefreshToken;
            token = MSARoot.AccessToken;
            #endregion
            var res = GetInfoFromToken(token);
            res.RefreshToken = ref_token;
            return res;
        }
        public static MSAPlayerInfoWithRefreshToken GetInfoWithRefreshTokenFromRefreshToken(string token)
        {
            string ref_token = "";
            #region 微软登录
            string Content = "client_id=00000000402b5328" +
            "&refresh_token=" + token +
            "&grant_type=refresh_token" +
            "&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf" +
            "&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://login.live.com/oauth20_token.srf");
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(Content);
            request.ContentLength = byteArray.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            var MSARoot = JsonConvert.DeserializeObject<MSAuthResponse>(responseFromServer);
            ref_token = MSARoot.RefreshToken;
            token = MSARoot.AccessToken;
            #endregion
            var res = GetInfoFromToken(token);
            res.RefreshToken = ref_token;
            return res;
        }
        public static async Task<string> GetNewTokenAsync()
        {
            var result = await AcquireToken(app, scopes, false);
            return result.AccessToken;
        }
        private static async Task<AuthenticationResult> AcquireToken(IPublicClientApplication app, string[] scopes, bool useEmbaddedView)
        {
            AuthenticationResult result;
            try
            {
                var accounts = await app.GetAccountsAsync();

                // Try to acquire an access token from the cache. If an interaction is required, 
                // MsalUiRequiredException will be thrown.
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                            .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                // Acquiring an access token interactively. MSAL will cache it so we can use AcquireTokenSilent
                // on future calls.
                result = await app.AcquireTokenInteractive(scopes)
                            .WithUseEmbeddedWebView(useEmbaddedView)
                            .ExecuteAsync();
            }

            return result;
        }
        public static async Task<MSAPlayerInfoWithRefreshToken> SinginNewAsync()
        {
            return GetInfoFromToken(await GetNewTokenAsync());
        }
        public static async Task<MSAPlayerInfoWithRefreshToken> GetInfoFromCacheAsync()
        {
            AuthenticationResult result;
            try
            {
                var accounts = await app.GetAccountsAsync();

                // Try to acquire an access token from the cache. If an interaction is required, 
                // MsalUiRequiredException will be thrown.
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                            .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                // Acquiring an access token interactively. MSAL will cache it so we can use AcquireTokenSilent
                // on future calls.
                result = await app.AcquireTokenInteractive(scopes)
                            .WithUseEmbeddedWebView(true)
                            .ExecuteAsync();
            }
            return GetInfoFromToken(result.AccessToken);
        }
        public static async Task<MSAPlayerInfoWithRefreshToken> GetInfoFromAccountAsync(IAccount account)
        {
            var result = await app.AcquireTokenSilent(scopes, account)
                            .ExecuteAsync();
            return GetInfoFromToken(result.AccessToken);
        }
    }
}