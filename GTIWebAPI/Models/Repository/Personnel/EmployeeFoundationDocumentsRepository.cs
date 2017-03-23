using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeeFoundationDocumentsRepository : IRepository<EmployeeFoundationDocument>
    {
        private IDbContextFactory factory;
        public EmployeeFoundationDocumentsRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeFoundationDocumentsRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public EmployeeFoundationDocument Add(EmployeeFoundationDocument foundationDocument)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                foundationDocument.Id = foundationDocument.NewId(db);
                db.EmployeeFoundationDocuments.Add(foundationDocument);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeFoundationDocumentExists(foundationDocument.Id))
                    {
                        throw new ConflictException();
                    }
                    else
                    {
                        throw;
                    }
                }
                foundationDocument = Get(foundationDocument.Id);
            }
            return foundationDocument;
        }

        public EmployeeFoundationDocument Delete(int id)
        {
            EmployeeFoundationDocument employeeFoundationDoc = new EmployeeFoundationDocument();

            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeFoundationDoc = Get(id);

                if (employeeFoundationDoc == null)
                {
                    throw new NotFoundException();
                }

                employeeFoundationDoc.Deleted = true;
                db.MarkAsModified(employeeFoundationDoc);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeFoundationDocumentExists(id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return employeeFoundationDoc;
        }

        public EmployeeFoundationDocument Edit(EmployeeFoundationDocument employeeFoundationDoc)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(employeeFoundationDoc);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeFoundationDocumentExists(employeeFoundationDoc.Id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
                employeeFoundationDoc = Get(employeeFoundationDoc.Id);
            }
            return employeeFoundationDoc;
        }

        public EmployeeFoundationDocument Get(int id)
        {
            EmployeeFoundationDocument doc = new EmployeeFoundationDocument();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                doc =
                    db.EmployeeFoundationDocuments
                    .Where(d => d.Id == id)
                    .Include(d => d.FoundationDocument)
                    .FirstOrDefault();
            }
            if (doc == null)
            {
                throw new NotFoundException();
            }
            return doc;
        }

        public List<EmployeeFoundationDocument> GetAll()
        {
            List<EmployeeFoundationDocument> list = new List<EmployeeFoundationDocument>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list = db.EmployeeFoundationDocuments
                    .Where(p => p.Deleted != true)
                    .Include(d => d.FoundationDocument)
                    .ToList();
            }
            if (list == null)
            {
                throw new NotFoundException();
            }
            return list;
        }

        public List<EmployeeFoundationDocument> GetByEmployeeId(int employeeId)
        {
            List<EmployeeFoundationDocument> list = new List<EmployeeFoundationDocument>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                list =
                    db.EmployeeFoundationDocuments
                    .Where(p => p.Deleted != true && p.EmployeeId == employeeId)
                    .Include(d => d.FoundationDocument)
                    .ToList();
            }
            if (list == null)
            {
                throw new NotFoundException();
            }
            return list;
        }

        private bool EmployeeFoundationDocumentExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeFoundationDocuments.Count(e => e.Id == id) > 0;
            }
        }
    }
}
