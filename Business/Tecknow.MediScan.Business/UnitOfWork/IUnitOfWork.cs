using System;
using Tecknow.MediScan.Business.RepositoryPattern.Interfaces;
using Tecknow.MediScan.Entities;

namespace Tecknow.MediScan.Business.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TenantMaster> TenanatRepository { get; }
        IGenericRepository<ApplicationUserMaster> UserRepository { get; }
        IGenericRepository<RoleMaster> RoleRepository { get; }
        IGenericRepository<AddressMaster> AddressRepository { get; }
        void Save();



    }
}