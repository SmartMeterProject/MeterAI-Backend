using System.Linq.Expressions;
using Business.Abstract;
using Business.Security.JWT;
using Core.Utilities.Results;
using DataAccess.Abstract.Models;
using Entity.Dtos;
using Entity.Models;
using Entity.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Counter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMeterService _meterService;
        private readonly CounterDbContext _context;

        public AuthController(IAuthService authService, CounterDbContext context, IMeterService meterService)
        {
            _authService = authService;
            _context = context;
            _meterService = meterService;
        }
        [HttpPost("register")]
        public async Task<IDataResult<RegisterResponse>> Register(UserForRegisterDto userForRegisterDto)
        {
            var result = await _authService.Register(userForRegisterDto);
            var token = await _authService.CreateAccessToken(result.Data);
            
            return new DataResult<RegisterResponse>(new RegisterResponse{Token=token},result.Success,result.Message);
        }

        [HttpPost("login")]
        public async Task<IDataResult<LoginResponse>> Login(UserForLoginDto userForLoginDto)
        {
            var result = await _authService.Login(userForLoginDto);
            if (result.Success)
            {
                var token = await _authService.CreateAccessToken(result.Data);
                var meterInfo = await _meterService.GetMeterInfo(result.Data.UserId);

                return new SuccessDataResult<LoginResponse>(new LoginResponse
                {
                    Token = token,
                    GetMeterInfoResponse = new GetMeterInfoResponse()
                    {
                        ErrorMessage = meterInfo.ErrorMessage,
                        IsSuccess = meterInfo.IsSuccess,
                        UserHouses = meterInfo.UserHouses
                    }
                },
                result.Success, result.Message);
            }
            return new ErrorDataResult<LoginResponse>(null, result.Success, result.Message);
        }
    }
}
