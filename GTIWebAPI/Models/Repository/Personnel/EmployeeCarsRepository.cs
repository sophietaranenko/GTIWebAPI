﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Context;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Models.Repository
{
    public class EmployeeCarRepository : IRepository<EmployeeCar>
    {
        private IDbContextFactory factory;
        public EmployeeCarRepository()
        {
            factory = new DbContextFactory();
        }

        public EmployeeCarRepository(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        public List<EmployeeCar> GetAll()
        {
            List<EmployeeCar> cars = new List<EmployeeCar>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                cars = db.EmployeeCars.Where(p => p.Deleted != true).ToList();
            }
            if (cars == null)
            {
                throw new NotFoundException();
            }
            return cars;
        }

        public List<EmployeeCar> GetByEmployeeId(int employeeId)
        {
            List<EmployeeCar> cars = new List<EmployeeCar>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                cars = db.EmployeeCars.Where(p => p.Deleted != true && p.EmployeeId == employeeId).ToList();
            }
            if (cars == null)
            {
                throw new NotFoundException();
            }
            return cars;
        }

        public EmployeeCar Get(int id)
        {
            EmployeeCar car = new EmployeeCar();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                car = db.EmployeeCars.Where(d => d.Id == id).FirstOrDefault();
            }
            if (car == null)
            {
                throw new NotFoundException();
            }
            return car;
        }

        public EmployeeCar Add(EmployeeCar car)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                car.Id = car.NewId(db);
                db.EmployeeCars.Add(car);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EmployeeCarExists(car.Id))
                    {
                        throw new ConflictException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return car;
        }

        public EmployeeCar Edit(EmployeeCar car)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                db.MarkAsModified(car);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeCarExists(car.Id))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return car;
        }

        public EmployeeCar Delete(int carId)
        {
            EmployeeCar employeeCar = new EmployeeCar();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                employeeCar = db.EmployeeCars.Where(d => d.Id == carId).FirstOrDefault();
                if (employeeCar == null)
                {
                    throw new NotFoundException();
                }
                employeeCar.Deleted = true;
                db.MarkAsModified(employeeCar);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeCarExists(carId))
                    {
                        throw new NotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return employeeCar;
        }

        

        public bool EmployeeCarExists(int id)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                return db.EmployeeCars.Count(e => e.Id == id) > 0;
            }
        }
    }
}
