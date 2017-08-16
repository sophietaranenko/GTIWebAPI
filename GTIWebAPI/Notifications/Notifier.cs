using GTIWebAPI.Models.Employees;
using System.Collections.Generic;
using System;
using GTIWebAPI.Models.Repository;

namespace GTIWebAPI.Notifications
{

    public interface INotifier
    {
        void Notify(Notification notification);
    }



    public class Notifier : INotifier
    {
        private UnitOfWork unitOfWork;

        public Notifier(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Notify(Notification notification)
        {

            //add
            //notify 
        }
    }

}
