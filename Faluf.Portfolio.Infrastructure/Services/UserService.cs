﻿using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Services;

namespace Faluf.Portfolio.Infrastructure.Services;

public class UserService : Service<ApplicationUser, UserModel, UserDTO>
{
	public UserService(HttpClient httpClient) : base(httpClient, "users") { }
}