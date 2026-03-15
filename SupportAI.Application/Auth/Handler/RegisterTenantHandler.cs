using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SupportAI.Application.Auth.Commands;
using SupportAI.Application.Auth.DTOs;
using SupportAI.Application.Interfaces;
using SupportAI.Domain.Entities;
using SupportAI.Domain.Enum;

namespace SupportAI.Application.Auth.Handler
{
    public class RegisterTenantHandler : IRequestHandler<RegisterTenantCommand, AuthResponse>
    {
        private readonly ITenantRepository tenantRepository;
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public RegisterTenantHandler(
            ITenantRepository tenantRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            this.tenantRepository = tenantRepository;
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse> Handle(
            RegisterTenantCommand request,
            CancellationToken cancellationToken)
        {
            var tenant = new Tenant(request.TenantName);

            await tenantRepository.AddAsync(tenant);

            var password = passwordHasher.Hash(request.Password);

            var user = new User(
                                Guid.NewGuid(),
                                request.Email,
                                password,
                                UserRoles.Admin);

            await userRepository.AddAsync(user);

            var token = jwtTokenGenerator.GenerateToken(user);

            return new AuthResponse(token);
        }
    }
}
