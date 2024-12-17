using AutoMapper;
using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StudyMaterialOrganiser.ViewModels;

namespace StudyMaterialOrganiser.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult List()
        {
            try
            {
                var allUsers = _userService.GetAll();
                var usersDtos = _mapper.Map<ICollection<UserDto>>(allUsers);
                return View(usersDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the list of users.");
                TempData["ToastMessage"] = "An error occurred while processing your request.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Approve(int id)
        {
            try
            {
                var userToUpdate = _userService.GetById(id);


                if (userToUpdate.Role == 2)
                {
                    return Json(new { success = false, message = "Admin Role cannot be changed", type = "error" });
                }
                if (userToUpdate.Role == 1)
                {
                    return Json(new { success = false, message = "Role cannot be increased further", type = "error" });
                }
                userToUpdate.Role = 1;
                _userService.Update(id, userToUpdate);
                return Json(new { success = true, message = $"{userToUpdate.Username} role updated successfully!", type = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving a user");
                return Json(new { success = false, message = ex.Message, type = "error" });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Disapprove(int id)
        {
            try
            {
                var userToUpdate = _userService.GetById(id);


                if (userToUpdate.Role == 2)
                {
                    return Json(new { success = false, message = "Admin Role cannot be changed", type = "error" });
                }

                if (userToUpdate.Role == 0)
                {
                    return Json(new { success = false, message = "Role cannot be lowered further", type = "error" });
                }
                userToUpdate.Role = 0;
                _userService.Update(id, userToUpdate);
                return Json(new { success = true, message = $"{userToUpdate.Username} role updated successfully!", type = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while disapproving a user");
                return Json(new { success = false, message = ex.Message, type = "error" });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult LoadUserList()
        {
            try
            {
                var users = _userService.GetAll();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return PartialView("_UserList", userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the user list.");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new UserRegistrationDto();
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserRegistrationDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                _userService.Create(model);


                TempData["ToastMessage"] = $"User {model.UserName} created successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user");
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                return View(_mapper.Map<UserEditVM>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user");
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("List");
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserEditVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = _userService.GetById(model.Id);
                if (user == null)
                {
                    TempData["ToastMessage"] = "User not found.";
                    TempData["ToastType"] = "error";
                    return RedirectToAction(nameof(List));
                }

                var existingUserByUsername = _userService.GetByUserName(model.Username);
                if (existingUserByUsername != null && existingUserByUsername.Id != model.Id)
                {
                    ModelState.AddModelError("Username", "Username is already in use");
                    return View(model);
                }

                var existingUserByEmail = _userService.GetByEmail(model.Email);
                if (existingUserByEmail != null && existingUserByEmail.Id != model.Id)
                {
                    ModelState.AddModelError("Email", "Email is already in use");
                    return View(model);
                }


                var toUpdate = _mapper.Map<UserDto>(model);
                _userService.Update(model.Id, toUpdate);

                TempData["ToastMessage"] = $"User {model.Username} updated successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(List));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user");
                TempData["ToastMessage"] = "An error occurred while processing your request.";
                TempData["ToastType"] = "error";
                return View(model);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                var model = _mapper.Map<UserDto>(user);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the user with ID {UserId}.", id);
                TempData["ToastMessage"] = "An error occurred while processing your request.";
                TempData["ToastType"] = "error";
                return RedirectToAction(nameof(List));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(UserDto user)
        {
            try
            {
                _userService.Delete(user.Id);
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user with ID {UserId}.", user.Id);
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(user);
            }


        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser(UserRegistrationDto user)
        {
            if (!ModelState.IsValid) return View(user);

            try
            {
                user.Role = 0;
                _userService.Create(user);

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user");
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(user);

            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult LogIn(UserLoginDto request)
        {
            if (!ModelState.IsValid) return View();

            try
            {

                var existingUser = _userService.GetByName(request.Username);
                if (existingUser == null)
                {
                    throw new InvalidOperationException("User does not exist");
                }
                if (existingUser.IsDeleted == true)
                {
                    throw new InvalidOperationException("Username no longer exists");
                }


                var authentication = _userService.Authenticate(request.Username, request.Password);
                if (authentication == null)
                {
                    throw new InvalidOperationException("Wrong Password");
                }





                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, existingUser.Role switch
                {
                    2 => "Admin",
                    1 => "User",
                    0 => "NonUser",
                    _ => throw new ArgumentOutOfRangeException()
                }),

                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);


                var authProperties = new AuthenticationProperties
                {

                };

                Task.Run(async () =>
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties)
                ).GetAwaiter().GetResult();


                TempData["ToastMessage"] = "You have successfully logged in";
                TempData["ToastType"] = "success";
                if (existingUser.Role.ToString() == "2")
                    return RedirectToAction("ListAdmin", "Project");
                if (existingUser.Role.ToString() == "1")
                    return RedirectToAction("List", "Application");

                return RedirectToAction("List", "Project");



            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View();
            }
        }
        [AllowAnonymous]
        public IActionResult Logout()
        {
            Task.Run(async () =>
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme)
            ).GetAwaiter().GetResult();

            return View();
        }

        [Authorize(Roles = "NonUser,User,Admin")]
        public IActionResult ProfileDetails()
        {
            try
            {
                var username = HttpContext.User.Identity.Name;
                var user = _userService.GetByUserName(username);

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user");
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("index", "Home");
            }
        }


        [Authorize(Roles = "NonUser,User,Admin")]
        public IActionResult ProfileDetailsPartial()
        {
            var username = HttpContext.User.Identity.Name;
            var user = _userService.GetByUserName(username);

            return PartialView("_ProfileDetailsPartial", user);
        }

        [Authorize(Roles = "NonUser,User,Admin")]
        public IActionResult ProfileEdit(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                var model = _mapper.Map<UserDto>(user);
                return PartialView("_EditModalPartial", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user");
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("ProfileDetails", new { id });
            }
        }

        [Authorize(Roles = "NonUser,User,Admin")]
        [HttpPost]
        public IActionResult ProfileEdit(int id, UserDto user)
        {
            var existingUserByUsername = _userService.GetByUserName(user.Username);
            if (existingUserByUsername != null && existingUserByUsername.Id != user.Id)
            {
                ModelState.AddModelError("Username", "Username is already in use");
                return PartialView("_EditModalPartial", user);
            }

            var existingUserByEmail = _userService.GetByEmail(user.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != user.Id)
            {
                ModelState.AddModelError("Email", "Email is already in use");
                return PartialView("_EditModalPartial", user);
            }

            try
            {
                _userService.Update(id, user);

                Task.Run(async () =>
                    await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme)
                ).GetAwaiter().GetResult();

                var updatedUserName = _userService.GetById(id);
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, updatedUserName.Username),
                new Claim(ClaimTypes.Role, updatedUserName.Role switch
                {
                    2 => "Admin",
                    1 => "User",
                    0 => "NonUser",
                    _ => throw new ArgumentOutOfRangeException()
                }), new Claim(ClaimTypes.NameIdentifier, updatedUserName.Id.ToString())
            };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);


                var authProperties = new AuthenticationProperties
                {

                };


                Task.Run(async () =>
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties)
                ).GetAwaiter().GetResult();


                return PartialView("_ProfileDetailsPartial", updatedUserName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing details.");
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("ProfileEdit");
            }
        }
    }
}