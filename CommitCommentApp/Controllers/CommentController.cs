using CommitCommentApp.Constants;
using CommitCommentApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;


namespace CommitCommentApp.Controllers
{
    public class CommentController : Controller
    {
     
        public ActionResult Index()
        {
            return View();
        }

        public List<Commit> GetCommits()
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Authentication-Token", APIEndPoints.apiKey);
            webClient.Headers.Add("user-agent", "User1");
            string content = webClient.DownloadString(APIEndPoints.gitRepoURL);
            List<Commit> _listCommits = JsonConvert.DeserializeObject<List<Commit>>(content);
            return _listCommits;
        }

        public List<CommitComment> GetCommitComments(List<Commit> listCommits)
        {
            List<CommitComment> _list = new List<CommitComment>();

            for (int i = 0; i < listCommits.Count; i++)
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("Authentication-Token", APIEndPoints.apiKey);
                webClient.Headers.Add("user-agent", listCommits[i].sha);
                string URL = "https://api.github.com/repos/octocat/Hello-World/commits/";
                string EndPoints = listCommits[i].sha + "/comments";
                string URLEndPoints = URL + EndPoints;
                string content = webClient.DownloadString(URLEndPoints);
                var listCommitComments = JsonConvert.DeserializeObject<List<CommitComment>>(content);
                _list.AddRange(listCommitComments);
            }
            return _list;

        }

        [HttpGet]
        public JsonResult GetCommitCommentList()
        {
            List<Commit> _listSHA = GetCommits();
            List<CommitComment> _listCommits = GetCommitComments(_listSHA);
            var x = from listCommits in _listCommits
                    select new
                    {
                        commit_id = listCommits.commit_id,
                        body = listCommits.body,
                        wordcount = listCommits.body.Length
                    };
            return Json(x, JsonRequestBehavior.AllowGet);
        }
    }
}

