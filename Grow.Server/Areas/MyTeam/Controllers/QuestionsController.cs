using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Areas.MyTeam.Model;
using Grow.Server.Areas.MyTeam.Model.ViewModels;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grow.Server.Areas.MyTeam.Controllers
{
    public class QuestionsController : BaseTeamController
    {
        public QuestionsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger) 
            : base(dbContext, appSettings, logger)
        {
        }

        public IActionResult Index()
        {
            var faq = DbContext
                .CommonQuestion
                .Where(e => e.Contest.Year == MyTeamQuery.Select(t => t.Contest.Year).Single())
                .ToList();

            FillViewBag();
            return View(faq);
        }
        
        private void FillViewBag()
        {
        }
    }
}