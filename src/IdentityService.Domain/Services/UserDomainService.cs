﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Learning.Domain;

namespace IdentityService.Domain
{
    public class UserDomainService
    {
        private readonly IUserRepository _repository;
        private readonly IJwtService _jwtService;
        public UserDomainService(IUserRepository repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<JwtDto> LoginByUserNameAsync(string userName, string password)
        {
            // TODO: 发布领域事件登录日志成功后者失败
            User? user = await _repository.GetUserByName(userName);
            if (user == null)
            {
                throw new BusinessException("用户名或密码错误!");
            }
            if (user.Password != EncryptHelper.MD5Encrypt(password))
            {
                throw new BusinessException("用户名或密码错误!");
            }

            user.UpdateLastLoginTime();
            // 每次登录version加1,可以适用于限制同一个账号登录。 TODO:需要考虑一下多客户端
            user.UpdateVersion();

            await _repository.UpdateAsync(user);

            long iat = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString(),ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iat, iat.ToString(), ClaimValueTypes.Integer64),
                new Claim(Contants.Version,user.JwtVersion.ToString(),ClaimValueTypes.Integer32)
            };


            if (user.UserName == "admin")
            {
                claims.Add(new Claim("role", "Admin"));
            }
            return _jwtService.CreateToken(user.Id, claims);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<JwtDto?> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            bool result = _jwtService.ValidateToken(accessToken, refreshToken, out long userId);
            if (!result)
            {
                //throw new BusinessException("token非法");
                return null;
            }

            // TODO：后续可以加是否禁用
            User? user = await _repository.GetAsync(userId);
            if (user == null)
            {
                //throw new BusinessException("token非法");
                return null;
            }

            // TODO:如果refresh token没有过期,那么重新生成refresh token时就不更新refresh token
            long iat = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString(),ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iat, iat.ToString(), ClaimValueTypes.Integer64)
            };

            if (user.UserName == "admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String));
            }

            return _jwtService.CreateToken(user.Id, claims);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="nickName">昵称</param>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<User> CreateUserAsync(string userName, string nickName, string? email, string password)
        {
            // 校验用户名和邮箱是否已存在
            var user = await _repository.GetUserByName(userName);
            if (user != null)
            {
                throw new BusinessException("用户名已存在!");
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                user = await _repository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    throw new BusinessException("邮箱已存在!");
                }
            }

            user = User.Create(userName, nickName, email, password);
            // 创建用户
            await _repository.InsertAsync(user);
            return user;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="nickName">昵称</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<User> UpdateUserAsync(long id, string nickName, string? email)
        {
            var user = await _repository.GetAsync(id);
            if (user == null)
            {
                throw new BusinessException("target not found");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var checkUser = await _repository.GetUserByEmailAsync(email);
                if (checkUser != null && checkUser.Id != user.Id)
                {
                    throw new BusinessException("邮箱已存在!");
                }
            }
            user.Update(nickName, email);
            return user;
        }
    }
}
