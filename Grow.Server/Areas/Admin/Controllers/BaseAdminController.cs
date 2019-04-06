using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
    public abstract class BaseAdminController : Controller
    {
        protected readonly GrowDbContext Context;

        protected BaseAdminController(GrowDbContext context)
        {
            Context = context;
        }
    }
}