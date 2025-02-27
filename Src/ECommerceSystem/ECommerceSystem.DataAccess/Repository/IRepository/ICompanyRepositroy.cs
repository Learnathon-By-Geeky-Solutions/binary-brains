﻿using ECommerceSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommerceSystem.DataAccess.Repository.IRepository.IRepository;

namespace ECommerceSystem.DataAccess.Repository.IRepository
{
    public interface ICompanyRepositroy : IRepository<Company>
    {
        void Update(Company obj);
    }
}
