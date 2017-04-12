using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository
{
    public class UnitOfWork : IDisposable
    {
        private IAppDbContext context;

        public UnitOfWork(IAppDbContext context)
        {
            this.context = context;
        }
            

        private GenericRepository<UserRight> userRightsRepository;
        public GenericRepository<UserRight> UserRightsRepository
        {
            get
            {

                if (this.userRightsRepository == null)
                {
                    this.userRightsRepository = new GenericRepository<UserRight>(context);
                }
                return userRightsRepository;
            }
        }

        private GenericRepository<UserRightMask> userRightMasksRepository;
        public GenericRepository<UserRightMask> UserRightMasksRepository
        {
            get
            {

                if (this.userRightMasksRepository == null)
                {
                    this.userRightMasksRepository = new GenericRepository<UserRightMask>(context);
                }
                return userRightMasksRepository;
            }
        }

       
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
