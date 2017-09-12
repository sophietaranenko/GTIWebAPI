using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.NovellGroupWiseSOAP
{
    public class NovellGroupWiseMail
    {
        public string Id { get; set; }

        public string Subject { get; set; }

        public string From { get; set; }

        public string FromEmail { get; set; }

        public string To { get; set; }

        public string BC { get; set; }

        public string CC { get; set; }

        public DateTime DateDelivered { get; set; }

        public bool Read { get; set; }

        public bool HasAttachments { get; set; }

        public string Text { get; set; }

        public string HtmlText { get; set; }

        public List<GroupWiseMailRecipient> Recipients { get; set; }

        public List<string> Attachments { get; set; }

        public string Direction { get; set; }


    }

    public class GroupWiseMailRecipient
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string DistributionType { get; set; }
    }
}
