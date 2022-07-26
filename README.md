
This repository aims to present an implementation of in-memory database usage and JWT authentication.
# 1. Jwt
You can see more about jwt at https://jwt.io/
## 1.1 Packages
Installing the packages: **Microsoft.AspNetCore.Authentication** and **Microsoft.AspNetCore.Authentication.JwtBearer**

## 1.2 User class and Validation

Entity to be used as the basis for token generation and authentication:

    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
    public enum Role
	{
	    Administrator,
	    Manager,
	    Employee,
	    Intern
	}

The **GetAsync** method will be used to fetch user and validate credentials. I created this method in **UserRepositoryReader** class.

    public async Task<User> GetAsync(string name, string password)
    {
        return await _userContext.Users.FirstOrDefaultAsync(u => u.Password == password && u.Name == name);
    }

## 1.3 Token Secret
We need to create a string that will be our private key for token generation and validation. I created this key in the **appsettings.Development.json** file and configured an Options class to read the key's value.

      "TokenSecret": {
	    "Secret": "bc9e75d87d374d8bbcddf4c7dc80ef1a"
	  }
	  
The **TokenSecretOptions** class:

    public class TokenSecretOptions
    {
        public string Secret { get; set; }
    }

## 1.4 Token Generating
Create a class called **TokenService** and within it a method called **GenerateToken**, here I specify the token encryption type, expiration time and put values in the claims.


    public class TokenService : ITokenService
    {
        private readonly TokenSecretOptions _options;
    
        public TokenService(IOptions<TokenSecretOptions> options)
        {
            _options = options.Value;
        }
    
        public string GenerateToken(User user)
        {
            Log.Information($"Generating Token for user {user.Name}");
    
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_options.Secret);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature);
            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                })
            };
    
            var token = jwtTokenHandler.CreateToken(descriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }

## 1.5 Startup configuration
In my **Startup** class I added the following code snippets:

    public void ConfigureServices(IServiceCollection services)
        {
    	    ...
            var secret = Encoding.ASCII.GetBytes(Configuration.GetSection("TokenSecret")["Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
	        {
	            x.RequireHttpsMetadata = false;
	            x.SaveToken = true;
	            x.TokenValidationParameters = new TokenValidationParameters
	            {
	                ValidateIssuerSigningKey = true,
	                IssuerSigningKey = new SymmetricSecurityKey(secret),
	                ValidateIssuer = false,
	                ValidateAudience = false
	            };
	        });
            ...
    	}
Be careful in the app configuration, the order matters.

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        ...
        app.UseAuthentication();
        app.UseAuthorization();
        ...
    }

## 1.6 Controllers configuration
I created a controller called **LoginController** to generate the token. Note that this class does not have the Authorize tag.

    [ApiController]
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
    
        public LoginController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
    
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AuthenticateAsync([FromBody] LoginRequest request)
        {
            var user = await _userService.GetAsync(request.Name, request.Password);
    
            if (user is null)
                return BadRequest("Invalid Name or Password");
    
            return Ok(_tokenService.GenerateToken(user));
        }
    }


In the **UserController** I defined some end points for specific use by some users, based on Role.

    [ApiController]
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
    
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
    
        [HttpPost]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
        {
            var user = await _userService.CreateAsync(request);
    
            if (user is null)
                return BadRequest("User already registered");
    
            return Created(nameof(CreateUserAsync),(UserResponse)user);
        }
    
        [HttpGet]
        [Authorize(Roles = "Manager,Administrator,Employee")]
        public ActionResult GetAllUsers()
        {
            var users = _userService.Get();
    
            if (users is null || !users.Any())
                return NoContent();
    
            return Ok(users.Select(c => (UserResponse)c));
        }
    
        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteUserAsync([FromQuery] Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
    }


#  2. In-Memory Database

## 2.1 Packages
Installing the package **Microsoft.EntityFrameworkCore.InMemory**

## 2.2 Database Configuration

In my **Startup** class I added the following code snippets:

	public void ConfigureServices(IServiceCollection services)
	{
	    services.AddDbContext<UserContext>(option => option.UseInMemoryDatabase("UserDatabase"));
		...
	}
	
And this is the implementation of **UserContext**:

    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
        }
    
        public DbSet<User> Users { get; set; }
    }
	
## 2.3 Seed Configuration
In my **Program** class I added the following code snippets
 
    public static void Main(string[] args)
    {
		...
        var host = CreateHostBuilder(args).Build();
        host.SeedDatabase();
        host.Run();
    }
    
	private static void SeedDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var userContext = services.GetRequiredService<UserContext>();
        userContext.Seed();
    }

The SeedDatabase function generates an instance of the **UserContext** context and calls the **Seed** function which inserts data into the database.

     public static void Seed(this UserContext context)
    {
        if (!context.Users.Any())
        {
            var users = new List<User> {
                 new User(Guid.NewGuid(), "bigboss", "qwerty123", Role.Administrator),
                 new User(Guid.NewGuid(), "littleboss", "abc123", Role.Manager),
                 new User(Guid.NewGuid(), "worker", "123456789", Role.Employee),
                 new User(Guid.NewGuid(), "noob", "p@ssw0rd", Role.Intern)
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }




## Give a Star 
If you found this Implementation helpful or used it in your Projects, do give it a star. Thanks!

## This project was built with
* [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Swagger](https://swagger.io/)
* [Serilog](https://serilog.net/)
* [Jwt](https://jwt.io/)
* [EF Core In-Memory Database](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/)

## My contacts
* [LinkedIn](https://www.linkedin.com/in/henry-saldanha-3b930b98/)
