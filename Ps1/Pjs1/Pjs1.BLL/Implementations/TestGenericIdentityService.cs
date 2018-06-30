using Microsoft.EntityFrameworkCore;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.Enums;
using Pjs1.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Pjs1.Common.GenericDbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using System.Linq;

namespace Pjs1.BLL.Implementations
{
    public class TestGenericIdentityService : ITestGenericIdentityService
    {
        private readonly IEntityFrameworkRepository<GenericUser, MsSqlGenericDb> _userRepository;
        private readonly IEntityFrameworkRepository<GenericRole, MsSqlGenericDb> _roleRepository;
        private readonly IEntityFrameworkRepository<GenericUserRole, MsSqlGenericDb> _userRoleRepository;

        private readonly UserManager<GenericUser> _userManager;
        private readonly SignInManager<GenericUser> _signInManager;

        private readonly RoleManager<GenericRole> _roleManager;
        

        public TestGenericIdentityService(
            IEntityFrameworkRepository<GenericUser, MsSqlGenericDb> userRepository,
            IEntityFrameworkRepository<GenericRole, MsSqlGenericDb> roleRepository,
            IEntityFrameworkRepository<GenericUserRole, MsSqlGenericDb> userRoleRepository,
            UserManager<GenericUser> userManager,
            SignInManager<GenericUser> signInManager,
            RoleManager<GenericRole> roleManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<GenericUser> TestUser(GenericUser user)
        {
            var passWord = "12345678Aa!";
            var usr = await _userManager.CreateAsync(user, passWord);

            var result = await _userRepository.GetAll(g => g.UserName == user.UserName).FirstOrDefaultAsync();
            var canSignInAsync = await _signInManager.CanSignInAsync(user);
            var singinpasswprd = await _signInManager.PasswordSignInAsync(user, passWord, false, false);
            var tokenConfirmEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmemailResult = await _userManager.ConfirmEmailAsync(user, tokenConfirmEmail);
            var singinpasswprd2 = await _signInManager.PasswordSignInAsync(user, passWord, false, false);

            var genericRole = new GenericRole { Name = "TestR", IsActive = true };
            var resultRoleManager = await _roleManager.CreateAsync(genericRole);
            var resultRole = await _roleRepository.GetAll(g => g.Name.Equals(genericRole.Name)).FirstOrDefaultAsync();
            //await _userRoleRepository.AddAsync(new GenericUserRole { UserId = result.Id, RoleId = resultRole.Id });

            var resultAgain = await _userManager.FindByIdAsync("2");

            //_roleManager.Roles.Where(w => w.)
            var s = await _userManager.AddToRolesAsync(resultAgain, new[] {"TestR"});
            var roles = await _userManager.GetRolesAsync(resultAgain);
            var users = await _userManager.GetUsersInRoleAsync("TestR");
            return user;
        }

    }
}
