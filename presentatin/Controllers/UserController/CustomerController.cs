
using ECommerce.Utility;
using Entities.User;
using Entities.User.Owned;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using presentation.Models;

namespace presentation.Controllers.UserController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        //private readonly ICustomerRepository _customerRepository;
        
        //public CustomerController(ICustomerRepository _customerRepository)
        //{
        //    this._customerRepository = _customerRepository;
        //}

        //[HttpPost]
        //public async Task<ActionResult<CustomerDto  >> SuignUp([FromBody]CustomerDto customerDto, CancellationToken cancellationToken)
        //{
        //    var customer = new Customer()
        //    {
        //        FullName = new FullName
        //        {
        //            FirstName = customerDto.FirstName,
        //        },
        //        Addresses = new List<Address>
        //        {
        //            new Address()
        //            {
        //                AddressTitle = customerDto.AddressTitle,
        //                City = customerDto.City,
        //            }

        //        },
        //        Gender = customerDto.Gender,
        //        CreateDate = DateTime.Today.ToShamsi(),
        //    };


        //     await _customerRepository.AddAsync(customer, customerDto.Password, cancellationToken);

        //    return customerDto;

        //}

    }
}
