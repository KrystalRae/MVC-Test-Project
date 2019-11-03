using LocifyTechnicalTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocifyTechnicalTest.Managers
{
    public class UserManager
    {
        private HttpClient _client;

        public UserManager(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserModel> GetUser()
        {
            var jsonUser = await _client.GetStringAsync("https://jsonplaceholder.typicode.com/users/1");
            UserModel user = JsonConvert.DeserializeObject<UserModel>(jsonUser);
            List<PostModel> userPosts = await GetUserPosts();
            user.Posts = userPosts;
            return (user);
        }

        public async Task<List<PostModel>> GetUserPosts()
        {
            var jsonUserPosts = await _client.GetStringAsync("https://jsonplaceholder.typicode.com/posts?userId=1");
            List<PostModel> posts = JsonConvert.DeserializeObject<List<PostModel>>(jsonUserPosts);
                       
            List<string> urls = new List<string>();
            foreach (PostModel post in posts)
            {
                urls.Add("https://jsonplaceholder.typicode.com/comments?postId=" + post.Id);
            }

            // I researched how to make the calls in parallel, as it is bad practise to be sending requests within a foreach loop. https://stackoverflow.com/questions/12337671/using-async-await-for-multiple-tasks
            var commentsResults = await Task.WhenAll(urls.Select(x => _client.GetStringAsync(x)));

            List<CommentModel> allComments = new List<CommentModel>();
            foreach (var comments in commentsResults)
            {
                allComments.AddRange(JsonConvert.DeserializeObject<List<CommentModel>>(comments));
            }

            foreach (PostModel post in posts)
            {
                post.Comments = allComments.Where(x => x.PostId == post.Id).ToList();
            }

            return (posts);
            
            //foreach (PostModel post in posts)
            //{
            //    string url = "https://jsonplaceholder.typicode.com/comments?postId=" + post.Id;
            //    var jsonPostComments = await _client.GetStringAsync(url);
            //    List<CommentModel> postComments = JsonConvert.DeserializeObject<List<CommentModel>>(jsonPostComments);
            //    post.Comments = postComments;
            //}

            //return (posts);
        }
    }
}
