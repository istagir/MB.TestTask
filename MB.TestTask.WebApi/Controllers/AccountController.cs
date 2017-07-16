using MB.TestTask.Authentication;
using MB.TestTask.Models;
using MB.TestTask.Services.Interfaces;
using MB.TestTask.WebAPI.Infrastructure;
using MB.TestTask.WebAPI.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MB.TestTask.WebAPI.Controllers
{
    [RoutePrefix("api/Account")]
    [ValidateModel]
    public class AccountController : ApiController
    {
        private readonly IAuthenticator _authenticator;
        private readonly IUserService _userService;
        private readonly IUnityContainer _container;
        private readonly string _anonymouseUser;
        private readonly IHashingService _hashingService;

        public AccountController(
            IUnityContainer container,
            IAuthenticator authenticator,
            IUserService userService,
            string anonymouseUser,
            IHashingService hashingService)
        {
            if (authenticator == null)
            {
                throw new ArgumentNullException(nameof(authenticator));
            }
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            if (string.IsNullOrEmpty(anonymouseUser))
            {
                throw new ArgumentNullException(nameof(anonymouseUser));
            }
            if (hashingService == null)
            {
                throw new ArgumentNullException(nameof(hashingService));
            }
            _authenticator = authenticator;
            _userService = userService;
            _container = container;
            _anonymouseUser = anonymouseUser;
            _hashingService = hashingService;

        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            if (_container.Resolve<IServerContext>().CurrentUser.Login == _anonymouseUser)
            {
                return Unauthorized();
            }

            var result = _userService.GetList().Select(u => new
            {
                Login = u.Login,
                Name = u.Name,
                SignInCount = u.SignInCount,
                LastSignInTime = u.LastSignInTime
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            if (_container.Resolve<IServerContext>().CurrentUser.Login == _anonymouseUser)
            {
                return Unauthorized();
            }

            var filter = new UserInfoFilter { Ids = new List<long> { id } };
            var result = _userService.GetList(filter).Select(u => new
            {
                Login = u.Login,
                Name = u.Name,
                SignInCount = u.SignInCount,
                LastSignInTime = u.LastSignInTime
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] long page, [FromUri] string orderBy, [FromUri] bool ascend)
        {
            if (_container.Resolve<IServerContext>().CurrentUser.Login == _anonymouseUser)
            {
                return Unauthorized();
            }

            var filter = new UserInfoFilter
            {
                Page = page,
                PageSize = 20,
                OrderBy = new List<OrderingField>() { new OrderingField { Field = orderBy, Ascend = ascend } }
            };
            var result = _userService.GetList(filter).Select(u => new
            {
                Login = u.Login,
                Name = u.Name,
                SignInCount = u.SignInCount,
                LastSignInTime = u.LastSignInTime
            });

            return Ok(result);
        }

        [HttpPost]
        [Route("SignIn")]
        public IHttpActionResult Post([FromBody] LoginBindingModel value)
        {
            var result = _authenticator.Authenticate(value.Login, value.Password);
            if (result == null)
            {
                return BadRequest("Вход не выполнен. Пользователь и/или пароль некорректны.");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Register([FromBody] RegisterBindingModel model)
        {
            var filter = new UserInfoFilter { Logins = new List<string> { model.Login } };
            var existingUser = _userService.GetList(filter);
            if (existingUser.Any())
            {
                return BadRequest("Пользователь с таким почтовым ящиком уже зарегистрирован.");
            }

            _userService.Add(new UserInfo
            {
                Login = model.Login,
                Name = model.UserName,
                PasswordHash = _hashingService.GetHash(model.Login + model.Password)
            });

            return Ok("Новый пользователь успешно создан.");
        }
    }
}