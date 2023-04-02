﻿using Data.Contracts;
using Data.Repositories;
using ECommerce.Utility;
using Entities.Municipality;
using Entities.User.Owned;
using Entities.Useres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using presentation.Models;
using presentation.Models.Municipality;

namespace presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MunicipalityController : ControllerBase
    {
        private readonly IMunicipalityRepository municipalityRepository;

        public MunicipalityController(IMunicipalityRepository _municipalityRepository)
        {
            municipalityRepository = _municipalityRepository;
        }
       
        [HttpPost]
        public async Task<ActionResult> SuignUp([FromForm] SignupMunicipalityDto loginMunicipalityDto, CancellationToken cancellationToken)
        {
            var municipality = new Municipality()
            {
                Name = loginMunicipalityDto.Name,
                Region = loginMunicipalityDto.Region,
                CreateDate = DateTime.Today.ToShamsi(),
            };
            await municipalityRepository.AddAsync(municipality, loginMunicipalityDto.Password, cancellationToken);

            return Content($"{loginMunicipalityDto.Name} : با موفقیت اضافه شد ");
        }

        [HttpPost]
        public async Task<ActionResult<Municipality>> Login(string Name, string password, CancellationToken cancellationToken)
        {
            var municipality = await municipalityRepository.LoginAsync(Name, password, cancellationToken);
            if (municipality == null)
                return Content($"{Name} یافت نشد");
            return municipality;
        }


        //تغییر دسترسی کاربر


    }
}