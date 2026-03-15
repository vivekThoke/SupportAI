using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SupportAI.Application.Auth.Commands;
using SupportAI.Application.Auth.DTOs;
using SupportAI.Application.Interfaces;

namespace SupportAI.Application.Auth.Handler
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public LoginHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse> Handle(
            LoginCommand request, 
            CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.email);

            if (user == null)
                throw new Exception("Invalid Credentials");

            var isValid = passwordHasher.Verify(request.password, user.PasswordHash);

            if (!isValid)
                throw new Exception("Invalid Credentials");

            var token = jwtTokenGenerator.GenerateToken(user);

            return new AuthResponse(token);
        }

    }
}
