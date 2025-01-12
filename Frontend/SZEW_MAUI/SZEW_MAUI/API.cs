using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;

namespace SZEW_MAUI {
    internal static class API {
        private static string? token;
        private static string? login;
        private static string? password;
        private static string address = "http://localhost:80/api/";
        private class loginResponse {
            public string? token { get; set; }
        }
        private class profileResponse {
            public int? id {
                get; set;
            }
            public string? login {
                get; set;
            }
            public string? name {
                get; set;
            }
            public string? surname {
                get; set;
            }
            public string? email {
                get; set;
            }
            public string? passwordHash {
                get; set;
            }
            public int? userType {
                get; set;
            }
        }
        private static profileResponse? profile;
        public static void setCredentials(string username, string pass) {
            login = username;
            password = pass;
            SecureStorage.SetAsync("login", login);
            SecureStorage.SetAsync("password", password);
        }
        private static bool retrieveCredentials() {
            try {
                var usernameTask = SecureStorage.GetAsync("login");
                var passTask = SecureStorage.GetAsync("password");
                if(usernameTask.Wait(10) && passTask.Wait(10)) {
                    if(usernameTask.Result != null && passTask.Result != null) {
                        login = usernameTask.Result;
                        password = passTask.Result;
                        return true;
                    }
                }
                return false;
            }
            catch {
                return false;
            }
        }
        private static bool retrieveToken() {
            if(login == null || password == null) {
                return false;
            }
            var request = new RestRequest("Auth/login", Method.Post);
            var client = new RestClient(address);
            request.AddJsonBody("{ \"login\": \"" + login + "\", \"password\": \"" + password + "\" }");
            try {
                var executeTask = client.ExecuteAsync(request);
                if(executeTask.Wait(1000)) {
                    var response = executeTask.Result;
                    if(response != null) {
                        if(response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null) {
                            var tokenObj = JsonSerializer.Deserialize<loginResponse>(response.Content);
                            if(tokenObj != null) {
                                token = tokenObj.token;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch {
                return false;
            }
        }
        public static int loginStatus() {
            //Return 0 if cannot log in,
            //1 if logged in as admin,
            //2 if logged in as mechanic.
            if(login == null || password == null) {
                if(!retrieveCredentials()) {
                    return 0;
                }
            }
            if(token == null) {
                if(!retrieveToken()) {
                    return 0;
                }
            }
            var request = new RestRequest("User/profile", Method.Get);
            var client = new RestClient(address);
            request.AddHeader("Authorization", "Bearer " + token);
            try {
                var executeTask = client.ExecuteAsync(request);
                if(executeTask.Wait(100)) {
                    var response = executeTask.Result;
                    if(response != null) {
                        if(response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null) {
                            var userObj = JsonSerializer.Deserialize<profileResponse>(response.Content);
                            if(userObj != null && userObj.userType != null) {
                                profile = userObj;
                                return (int)userObj.userType;
                            }
                        }
                    }
                }
                return 0;
            }
            catch {
                return 0;
            }
        }
    }
}
