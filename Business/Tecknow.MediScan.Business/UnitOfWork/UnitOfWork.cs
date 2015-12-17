using System;
using System.Data.Entity;
using Tecknow.MediScan.Business.RepositoryPattern.Interfaces;
using Tecknow.MediScan.Business.RepositoryPattern.Repository;

using Tecknow.MediScan.Entities;

namespace Tecknow.MediScan.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context = new DataBaseContext();
        private IGenericRepository<AddressMaster> _addressRepository;
        private IGenericRepository<RoleMaster> _roleRepository;
        private IGenericRepository<TenantMaster> _tenantRepository;
        private IGenericRepository<ApplicationUserMaster> _userRepository;

        public IGenericRepository<TenantMaster> TenanatRepository
        {
            get
            {
                _tenantRepository = new GenericRepository<TenantMaster>(_context);
                return _tenantRepository;
            }
        }

        public IGenericRepository<ApplicationUserMaster> UserRepository
        {
            get
            {
                _userRepository = new GenericRepository<ApplicationUserMaster>(_context);
                return _userRepository;
            }
        }

        public IGenericRepository<RoleMaster> RoleRepository
        {
            get
            {
                _roleRepository = new GenericRepository<RoleMaster>(_context);
                return _roleRepository;
            }
        }

        public IGenericRepository<AddressMaster> AddressRepository
        {
            get
            {
                _addressRepository = new GenericRepository<AddressMaster>(_context);
                return _addressRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


   
}