using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommitCommentApp.Models
{
    public class CommitComment
    {
        public string commit_id { get; set; }
        public string body { get; set; }
        public string wordcount { get; set; }

    }
}