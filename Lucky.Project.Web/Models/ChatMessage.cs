using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Models
{
    public class ChatMessage
    {
        public int Type { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }
    public class GroupChatMessage
    {
        public string GroupName { get; set; }
        public string Content { get; set; }
    }
}
