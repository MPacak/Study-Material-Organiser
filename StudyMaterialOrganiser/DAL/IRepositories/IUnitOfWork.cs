﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IUnitOfWork
    {
        IGroupRepository Group{ get; }
		IUserGroupRepository UserGroup { get; }
		ILogRepository Log { get; }
        IUserRepository User { get; }
        void Save();
    }
}
