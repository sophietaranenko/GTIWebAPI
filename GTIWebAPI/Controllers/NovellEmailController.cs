using GTIWebAPI.Filters;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.NovelleDirectory;
using GTIWebAPI.NovellGroupWiseSOAP;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/NovellEmail")]
    public class NovellEmailController : ApiController
    {
        IDbContextFactory factory;
        INovellGroupWiseFactory novellFactory;

        public NovellEmailController()
        {
            factory = new DbContextFactory();
            novellFactory = new NovellGroupWiseFactory();
        }

        public NovellEmailController(IDbContextFactory factory, INovellGroupWiseFactory novellFactory)
        {
            this.factory = factory;
            this.novellFactory = novellFactory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetIncoming")]
        [ResponseType(typeof(IEnumerable<NovellGroupWiseMail>))]
        public IHttpActionResult GetIncomingEmails()
        {
            List<NovellGroupWiseMail> incomingMails = new List<NovellGroupWiseMail>();
            using (INovellGroupWise novell = novellFactory.CreateNovellGroupWise())
            {
                incomingMails = novell.Incoming();
            }
            return Ok(incomingMails);
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetOutcoming")]
        [ResponseType(typeof(IEnumerable<NovellGroupWiseMail>))]
        public IHttpActionResult GetOutcomingEmails()
        {
            List<NovellGroupWiseMail> outcomingMails = new List<NovellGroupWiseMail>();
            using (INovellGroupWise novell = novellFactory.CreateNovellGroupWise())
            {
                outcomingMails = novell.Outcoming();
            }
            return Ok(outcomingMails);
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        public IHttpActionResult GetEmailsByOrganizationId(int organizationId)
        {
            List<string> organizationEmails = new List<string>();

            //сформировать список всех email-ов, ассоциированных с компанией 
            UnitOfWork unitOfWork = new UnitOfWork(factory);
            string organizationEmail = unitOfWork.OrganizationsRepository.Get(d => d.Id == organizationId && d.Email != null).Select(d => d.Email).FirstOrDefault();
            if (organizationEmail != null)
            {
                organizationEmails.Add(organizationEmail);
            }
            List<OrganizationContactPerson> contactPersons = unitOfWork.OrganizationContactPersonsRepository.Get(d => d.OrganizationId == organizationId).ToList();
            foreach (var person in contactPersons)
            {
                if (person.Email != null)
                {

                        organizationEmails.Add(person.Email);
                }
                List<string> emails = unitOfWork.OrganizationContactPersonContactsRepository
                    .Get(d => d.OrganizationContactPersonId == person.Id && d.ContactType.Name == "E-mail" && d.Value != null)
                    .Select(d => d.Value)
                    .ToList();
                organizationEmails.AddRange(emails);
            }


            List<NovellGroupWiseMail> allMails = new List<NovellGroupWiseMail>();
            using (INovellGroupWise novell = novellFactory.CreateNovellGroupWise())
            {
                allMails.AddRange(novell.Incoming());
                allMails.AddRange(novell.Outcoming());
            }

            organizationEmails = organizationEmails.Distinct().ToList();

            List<NovellGroupWiseMail> organizationMails = new List<NovellGroupWiseMail>();
            foreach (var email in organizationEmails)
            {
                organizationMails.AddRange(allMails.Where(d => d.From != null).Where(a => a.From.Contains(email)));
                organizationMails.AddRange(allMails.Where(d => d.To != null).Where(a => a.To.Contains(email)));
                organizationMails.AddRange(allMails.Where(d => d.CC != null).Where(a => a.CC.Contains(email)));
                organizationMails.AddRange(allMails.Where(d => d.BC != null).Where(a => a.BC.Contains(email)));
                //List<NovellGroupWiseMail> mails = allMails.Where(d => d.From != null & d.From.Contains(email)).ToList();
                //mails = allMails.Where(d => d.To != null).Where(a => a.To.Contains(email)).ToList();
                //mails = allMails.Where(d => d.CC != null & d.CC.Contains(email)).ToList();
                //mails = allMails.Where(d => d.BC != null & d.BC.Contains(email)).ToList();
            }
            organizationMails = organizationMails.OrderBy(d => d.DateDelivered).ToList();
            return Ok(organizationMails);
        }


        [GTIFilter]
        [HttpGet]
        [Route("GetEmail")]
        [ResponseType(typeof(NovellGroupWiseMail))]
        public IHttpActionResult GetEmail(string id)
        {
            NovellGroupWiseMail mail = new NovellGroupWiseMail();
            using (INovellGroupWise novell = novellFactory.CreateNovellGroupWise())
            {
                mail = novell.ReadMail(id);
            }
            return Ok(mail);
        }

        [GTIFilter]
        [HttpPost]
        [Route("SendEmail")]
        public IHttpActionResult SendEmail(NovellGroupWiseMail mail)
        {
            using (INovellGroupWise novell = novellFactory.CreateNovellGroupWise())
            {
                novell.SendEmail(mail);
            }
            return Ok(mail);
        }

    }
}
