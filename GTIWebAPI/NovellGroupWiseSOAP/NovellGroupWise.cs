using GTIWebAPI.Exceptions;
using GTIWebAPI.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.WebReference;
using GTIWebAPI.Models.Service;
using System.Web;
using System.IO;

namespace GTIWebAPI.NovellGroupWiseSOAP
{
    public interface INovellGroupWise : IDisposable
    {
        NovellGroupWisePostOfficeConnection Connect(string login, string password);

        List<NovellGroupWiseContactBook> GetBooks();

        List<NovellGroupWiseContact> GetContacts(String bookId);

        List<NovellGroupWiseMail> Incoming();

        List<NovellGroupWiseMail> Outcoming();

        void SendEmail(NovellGroupWiseMail mail);

        WebReference.Folder[] getFolders();

        List<NovellGroupWiseMail> OpenFolder(Folder folder);

        string exportData(WebReference.AttachmentItemInfo info);

        NovellGroupWiseMail ReadMail(string id);

    }



    public class NovellGroupWise : INovellGroupWise
    {

        WebReference.GroupWiseBinding ws;

        WebReference.GroupWiseEventsBinding es;

        public NovellGroupWise()
        {
            ws = new WebReference.GroupWiseBinding();
            es = new WebReference.GroupWiseEventsBinding();
        }

        public NovellGroupWise(string postOffice, string sessionId)
        {

            ws = new WebReference.GroupWiseBinding();
            es = new WebReference.GroupWiseEventsBinding();
            String str = "http://" + postOffice + ":7191/soap";
            ws.Url = str;
            es.Url = str;

            ws.session = new WebReference.@string();
            ws.session.Text = new String[1];
            ws.session.Text[0] = sessionId;

            es.session = new WebReference.@string();
            es.session.Text = new String[1];
            es.session.Text[0] = sessionId;
        }

        public NovellGroupWisePostOfficeConnection Connect(string login, string password)
        {
            string postOffice = Properties.Settings.Default.SMTPIPAddress;
            return Connect(login, password, postOffice);
        }

        private NovellGroupWisePostOfficeConnection Connect(string login, string password, string postOffice)
        {
            WebReference.loginRequest req = new WebReference.loginRequest();
            WebReference.loginResponse resp = new WebReference.loginResponse();
            NovellGroupWisePostOfficeConnection connection = new NovellGroupWisePostOfficeConnection();
            connection.PostOffice = postOffice;
            try
            {
                String str = "http://" + connection.PostOffice + ":7191/soap";
                ws.Url = str;
                es.Url = str;

                WebReference.PlainText pt = new WebReference.PlainText();
                pt.username = login;
                pt.password = password;

                req.auth = pt;

                req.userid = true;
                req.useridSpecified = true;

                resp = ws.loginRequest(req);

                if (0 == resp.status.code)
                {
                    connection.SessionId = resp.session;
                }
                else if (59923 == resp.status.code)
                {
                    return Connect(login, password, resp.redirectToHost[0].ipAddress);
                }
                else if (53505 == resp.status.code)
                {
                    throw new NotOnPostOfficeException("User not on post office");
                }
            }
            catch (Exception e)
            {
                throw new NovellGroupWiseException("Cannot connect to Post Office");
            }

          //  NovellUser user = 
            return connection;
        }


        public List<NovellGroupWiseContactBook> GetBooks()
        {
            String str;
            List<NovellGroupWiseContactBook> books = new List<NovellGroupWiseContactBook>();
            WebReference.AddressBook book;
            WebReference.getAddressBookListRequest req = new WebReference.getAddressBookListRequest();
            WebReference.getAddressBookListResponse resp;
            resp = ws.getAddressBookListRequest(req);
            if (0 == resp.status.code)
            {

                for (int i = 0; i < resp.books.Length; i++)
                {
                    book = (WebReference.AddressBook)resp.books.GetValue(i);
                    if (book.isPersonal)
                    {
                        if (book.isFrequentContacts)
                        {
                            str = "F";
                        }
                        else
                        {
                            str = "P";
                        }
                    }
                    else
                    {
                        str = "S";
                    }
                    NovellGroupWiseContactBook novellGWbook = new NovellGroupWiseContactBook();
                    novellGWbook.Name = book.name;
                    novellGWbook.Id = book.id;
                    books.Add(novellGWbook);
                }

            }
            else
            {
                throw new NovellGroupWiseException("Cannot get Novell Group Wise AddressBooks");
            }
            return books;
        }


        public List<NovellGroupWiseContact> GetContacts(String bookId)
        {
            String cont;
            String email;
            String name;
            String str;
            String uuid;
            WebReference.AddressBookItem item;
            WebReference.Contact contact;
            WebReference.createCursorRequest creq = new WebReference.createCursorRequest();
            WebReference.createCursorResponse cresp;
            WebReference.destroyCursorRequest dreq = new WebReference.destroyCursorRequest();
            WebReference.destroyCursorResponse dres;
            WebReference.readCursorRequest rreq = new WebReference.readCursorRequest();
            WebReference.readCursorResponse rresp;
            cont = bookId;
            List<NovellGroupWiseContact> contacts = new List<NovellGroupWiseContact>();
            creq.container = cont;
            cresp = ws.createCursorRequest(creq);
            if (0 != cresp.status.code)
            {
                throw new NovellGroupWiseException("Cannot connect to Novell Group Wise AddressBookCollection");
            }
            for (;;)
            {
                rreq.container = cont;
                rreq.count = 100;
                rreq.cursor = cresp.cursor;
                try
                {
                    rresp = ws.readCursorRequest(rreq);
                }
                catch (InvalidOperationException ex)
                {
                    System.Console.WriteLine(ex);
                    continue;
                }
                if (0 != rresp.status.code)
                {
                    break;
                }
                for (int i = 0; null != rresp.items && null != rresp.items.item && i < rresp.items.item.Length; i++)
                {
                    NovellGroupWiseContact novellContact = new NovellGroupWiseContact();
                    item = (WebReference.AddressBookItem)rresp.items.item[i];
                    name = item.name;
                    novellContact.Name = item.name;
                    email = null;
                    uuid = item.uuid;
                    novellContact.UUId = item.uuid;
                    if (item is WebReference.Contact)
                    {
                        contact = (WebReference.Contact)item;
                        str = "C";
                        novellContact.TYpe = "Contact";
                        if (null != contact.emailList && null != contact.emailList.primary)
                        {
                            novellContact.Email = contact.emailList.primary;
                        }
                        if (null == name || 0 == name.Length)
                        {
                            if (null != contact.fullName.firstName)
                            {
                                novellContact.FirstName = contact.fullName.firstName;
                                novellContact.Name = contact.fullName.firstName;

                                if (null != contact.fullName.lastName)
                                {
                                    novellContact.Name += " ";
                                    novellContact.Name += contact.fullName.lastName;
                                    novellContact.LastNAme = contact.fullName.lastName;
                                }
                            }
                            else if (null != contact.fullName.lastName)
                            {
                                novellContact.Name = contact.fullName.lastName;
                                novellContact.LastNAme = contact.fullName.lastName;
                            }
                            else
                            {
                                novellContact.Name = contact.fullName.displayName;
                                novellContact.LastNAme = contact.fullName.displayName;
                            }
                        }
                    }
                    else if (item is WebReference.Resource)
                    {
                        novellContact.TYpe = "Resource";
                        novellContact.Email = ((WebReference.Resource)item).email;
                    }
                    else if (item is WebReference.Group)
                    {
                        novellContact.TYpe = "Group";
                    }
                    else if (item is WebReference.Organization)
                    {
                        novellContact.TYpe = "Organization";
                        novellContact.Email = ((WebReference.Organization)item).website;
                    }
                    else
                    {
                        str = "U";
                        novellContact.TYpe = "Unknown";
                    }
                    contacts.Add(novellContact);
                }
                if (null == rresp.items || null == rresp.items.item || 100 != rresp.items.item.Length)
                {
                    break;
                }
            }
            dreq.container = cont;
            dreq.cursor = cresp.cursor;
            dres = ws.destroyCursorRequest(dreq);
            return contacts;
        }


        public List<NovellGroupWiseMail> Incoming()
        {
            //подключение 
            List<NovellGroupWiseMail> incomingEmails = new List<NovellGroupWiseMail>();
            WebReference.Folder[] folders = getFolders();
            foreach (var folder in folders)
            {
                if (folder.name == "Mailbox")
                {
                    incomingEmails = OpenFolder(folder);
                }
                incomingEmails.Select(c => { c.Direction = "Incoming"; return c; }).ToList();
            }
            return incomingEmails;
        }

        public List<NovellGroupWiseMail> Outcoming()
        {
            List<NovellGroupWiseMail> incomingEmails = new List<NovellGroupWiseMail>();
            WebReference.Folder[] folders = getFolders();
            foreach (var folder in folders)
            {
                if (folder.name == "Sent Items")
                {
                    incomingEmails = OpenFolder(folder);
                }
                incomingEmails.Select(c => { c.Direction = "Outcoming"; return c; }).ToList();
            }
            return incomingEmails;
        }

        /// <summary>
        /// Attachment should be with ABSOLUTE PATH (with AppPath()) 
        /// </summary
        public void SendEmail(NovellGroupWiseMail mail)
        {
            WebReference.Mail mailToSend = new WebReference.Mail();
            WebReference.MessagePart[] part;
            WebReference.NameAndEmail item;
            WebReference.Recipient[] recips;
            WebReference.sendItemRequest req;
            WebReference.sendItemResponse resp;
            mailToSend.subject = mail.Subject;

            var att = new WebReference.AttachmentItemInfo();

            List<AttachmentItemInfo> atts = new List<AttachmentItemInfo>();

            if (mail.Attachments != null)
            { 
            foreach(var attachment in mail.Attachments)
            { 
                var fs = new FileStream(attachment, FileMode.OpenOrCreate);
                using (var binaryReader = new BinaryReader(fs))
                {
                    att.data = binaryReader.ReadBytes((int)fs.Length);
                }
                att.name = System.IO.Path.GetFileName(attachment);
                atts.Add(att);
            }
            }

            mailToSend.attachments = atts.ToArray();
            mailToSend.hasAttachment = true;

            if (0 != mail.Text.Length)
            {
                part = new WebReference.MessagePart[1];
                part[0] = new WebReference.MessagePart();
                part[0].Value = System.Text.UTF8Encoding.UTF8.GetBytes(mail.Text);
                mailToSend.message = part;
            }
            recips = new WebReference.Recipient[mail.Recipients.Count];
            for (int i = 0; i < mail.Recipients.Count; i++)
            {
                recips[i] = new WebReference.Recipient();
                recips[i].displayName = mail.Recipients[i].Name;
                recips[i].email = mail.Recipients[i].Email;
                recips[i].uuid = mail.Recipients[i].Id;
            }
            mailToSend.distribution = new WebReference.Distribution();
            mailToSend.distribution.recipients = recips;
            req = new WebReference.sendItemRequest();
            req.item = mailToSend;
            resp = ws.sendItemRequest(req);
        }

        public WebReference.Folder[] getFolders()
        {
            WebReference.getFolderListRequest req = new WebReference.getFolderListRequest();
            WebReference.getFolderListResponse resp;
            req.recurse = true;
            req.parent = "folders";
            resp = ws.getFolderListRequest(req);
            if (0 == resp.status.code)
            {
                return resp.folders;
            }
            else
            {
                throw new NovellGroupWiseException("Cannot get Novell Group Wise Folders");
            }
        }

        public List<NovellGroupWiseMail> OpenFolder(Folder folder)
        {
            String date;
            String str;

            WebReference.createCursorRequest creq = new WebReference.createCursorRequest();
            WebReference.createCursorResponse cresp;
            WebReference.destroyCursorRequest dreq = new WebReference.destroyCursorRequest();
            WebReference.destroyCursorResponse dres;
            WebReference.readCursorRequest rreq = new WebReference.readCursorRequest();
            WebReference.readCursorResponse rresp;
            WebReference.Mail item;

            creq.container = folder.id;
            creq.view = null;
            cresp = ws.createCursorRequest(creq);

            if (0 != cresp.status.code)
            {
                throw new NovellGroupWiseException(String.Format("Can not open Group Wise {0}", folder.name));
            }

            string letters = "";
            List<NovellGroupWiseMail> mails = new List<NovellGroupWiseMail>();

            for (;;)
            {
                rreq.container = folder.id;
                rreq.count = 100;
                rreq.cursor = cresp.cursor;
                rresp = ws.readCursorRequest(rreq);
                if (0 != rresp.status.code || null == rresp.items ||
                      null == rresp.items.item)
                {
                    break;
                }
                for (int i = 0; i < rresp.items.item.Length; i++)
                {
                    item = (WebReference.Mail)rresp.items.item[i];
                    //учитываем только письма, остальное не интересно 
                    //что бывает остальное - смотрим в Samples 
                  //  if (item is WebReference.Mail || item is WebReference.PhoneMessage)
                     if (item is WebReference.Mail )
                        {
                        NovellGroupWiseMail mail = new NovellGroupWiseMail();
                        mail.Id = item.id;
                        //дата доставки письма 
                        mail.DateDelivered = item.delivered;

                        //прочитано пользователем 
                        if (null != item.status && item.status.read)
                        {
                            mail.Read = true;
                        }
                        if (null != item.distribution && null != item.distribution.from && null != item.distribution.from.displayName)
                        {
                            //от кого 
                            mail.From = item.distribution.from.displayName;
                            mail.FromEmail = item.distribution.from.email;

                            //кому 
                            mail.To = item.distribution.to;

                            if (item.distribution.recipients != null)
                            {
                                mail.Recipients = new List<GroupWiseMailRecipient>();
                                foreach (var recipient in item.distribution.recipients)
                                {
                                    GroupWiseMailRecipient rec = new GroupWiseMailRecipient()
                                    {
                                        Id = recipient.uuid,
                                        Email = recipient.email,
                                        Name = recipient.displayName
                                    };
                                    mail.Recipients.Add(rec);
                                }
                            }
                        }
                        //тема письма
                        mail.Subject = item.subject;

                        //id письма 
                        mail.Id = item.id;

                        mails.Add(mail);
                    }
                }
                //не более чем 100, кстати почему? 
                if (100 != rresp.items.item.Length)
                {
                    break;
                }
            }
            //удаляем курсор (черная магия) 
            dreq.container = folder.id;
            dreq.cursor = cresp.cursor;
            dres = ws.destroyCursorRequest(dreq);
            return mails;
        }




        public string exportData(WebReference.AttachmentItemInfo info)
        {
            int offset = 0;
            WebReference.getAttachmentRequest req;
            WebReference.getAttachmentResponse resp;

            String fileName = Guid.NewGuid().ToString();
            // String path = "c:\\temp\\" + info.name;
            String path = HttpContext.Current.Server.MapPath("~/TemporaryFiles/") + fileName + System.IO.Path.GetExtension(info.name);
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
            System.IO.BinaryWriter fw = new System.IO.BinaryWriter(fs);

            for (;;)
            {
                req = new WebReference.getAttachmentRequest();
                req.id = info.id.Value;
                req.length = 6144;
                req.offset = offset;
                resp = ws.getAttachmentRequestMessage(req);
                if (0 != resp.status.code)
                {
                    break;
                }
                fw.Write(resp.part.Value);
                offset = resp.part.offset;
                if (0 == offset)
                {
                    break;
                }
            }
            fs.Close();
            path = path.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
            return path;
        }

        public NovellGroupWiseMail ReadMail(string id)
        {
            WebReference.Mail item;
            WebReference.getItemRequest req = new WebReference.getItemRequest();
            WebReference.getItemResponse resp;
            NovellGroupWiseMail mail = new NovellGroupWiseMail();

            req.id = id;
            req.view = "default message peek attachments recipientStatus";
            resp = ws.getItemRequest(req);

            if (0 == resp.status.code)
            {
                item = (WebReference.Mail)resp.item;
                if (item is WebReference.Mail)
                {
                    String str = item.GetType().ToString();
                    int i = str.LastIndexOf('.');
                    if (-1 != i)
                    {
                        str = str.Substring(i + 1);
                    }

                    mail.Recipients = new List<GroupWiseMailRecipient>();
                    Recipient[] rec = item.distribution.recipients;
                    for(int j = 0; j < rec.Count(); j++)
                    {
                        GroupWiseMailRecipient recipient = new GroupWiseMailRecipient
                        {
                            Id = rec[j].uuid,
                            Email = rec[j].email,
                            DistributionType = rec[j].distType.ToString(),
                            Name = rec[j].displayName
                        };
                        mail.Recipients.Add(recipient);
                    }


                    mail.To = item.distribution.to;
                    mail.BC = item.distribution.bc;
                    mail.CC = item.distribution.cc;

               

                    String Text = str;
                    mail.HasAttachments = item.hasAttachment;
                    if (null != item.distribution && null != item.distribution.from &&
                          null != item.distribution.from.displayName)
                    {
                        mail.From = item.distribution.from.displayName;
                    }
                    if (null != item.distribution.to)
                    {
                        mail.To = item.distribution.to;
                    }
                    if (null != item.subject)
                    {
                        mail.Subject = item.subject;
                    }
                    if (null != item.message && 0 != item.message.Length)
                    {
                        WebReference.MessagePart part = (WebReference.MessagePart)item.message.GetValue(0);
                        System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
                        mail.Text = utf8.GetString(part.Value);
                    }
                    if (mail.HasAttachments)
                    {
                        WebReference.AttachmentItemInfo info;
                        List<string> attachmentsArray = new List<string>();
                        for (i = 0; i < item.attachments.Length; i++)
                        {
                            info = (WebReference.AttachmentItemInfo)item.attachments.GetValue(i);
                            if (null == info || info.id.itemReference)
                            {
                                continue;
                            }
                            attachmentsArray.Add(exportData(info));
                        }
                        mail.Attachments = attachmentsArray;
                    }
                }
            }

            return mail;
        }

        public void Dispose()
        {
            if (ws != null)
            {
                ws.Dispose();
            }
            if (es != null)
            {
                es.Dispose();
            }
        }


    }
}
