using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecuenceBack.Models;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace SecuenceBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly AppDBContext _context;
        private readonly IConfiguration _appsettings;
        private readonly CMSEncryptor _encryptor;
        private readonly IWebHostEnvironment _env;

        public UserController(AppDBContext context, IConfiguration appsettings, IRepository repository, IWebHostEnvironment env)
        {
            _context = context;
            _appsettings = appsettings;
            _repository = repository;
            _encryptor = new CMSEncryptor(appsettings);
            _env = env;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.UserTbl.Where(u => u.Email == model.Email && u.Status != 0).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (_encryptor.Compare(model.Password, user.Password))
                    {
                        var UserMedic = await _context.UserMedicHCRel.Where(u => u.UserID == user.UserID).FirstOrDefaultAsync();

                        var rol = await _repository.SelectById<RolTbl>(UserMedic.RollID);
                        List<PermissionsTbl> permissions = new List<PermissionsTbl>();
                        if (rol != null)
                        {
                            var rolp = await _context.RolPermissionsRel.Where(rp => rp.RolID == rol.RolID).ToListAsync();
                            foreach (RolPermissionsRel rolper in rolp)
                            {
                                permissions.Add(await _context.PermissionsTbl.Where(p => p.PermissionsID == rolper.PermissionsID).FirstOrDefaultAsync());
                            }
                        }

                        var token = createToken(user, rol, permissions);
                        respuesta.Ok = 1;
                        respuesta.Data.Add(new
                        {
                            token,
                            permissions,
                            UserMedic.RollID,
                            UserMedic.HealthCenterID
                        });
                    }
                    else
                    {
                        respuesta.Ok = 0;
                        respuesta.Message = "El Correo electrónico/Contraseña no coinciden";
                        return BadRequest(respuesta);
                    }
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "El Correo electrónico/Contraseña no coinciden";
                    return NotFound(respuesta);
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message;
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }


        [HttpPost("CreateUser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateUser(UserCreate userC)
        {
            Respuesta<object> respuesta = new();
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            try
            {
                string validate = validateText(userC.FullName);
                if (validate != "validado")
                {
                    //El Nombre o apellido no tiene el formato
                    respuesta.Ok = 0;
                    respuesta.Message = validate;
                    return BadRequest(respuesta);
                }

                //algo como buscar el Email en la base AD para luego ver si ese usuario exite y no esta duplicado en la base de datos



                if (await _context.UserTbl.Where(u => u.Email == userC.Email && u.DeletedAt == null).FirstOrDefaultAsync() == null)
                {


                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                    UserTbl user = new UserTbl();
                    // Existe en el AD, No existe usuario registrado y ademas el rol existe en la db



                    user.Email = userC.Email.ToLower();
                    user.FullName = textInfo.ToTitleCase(userC.FullName.TrimEnd().TrimStart().ToLower());
                    user.Password = _encryptor.Encrypt(userC.Password);
                    user.Country = userC.Country;
                    user.Photo = userC.Photo;
                    user.PolicesAccepted = userC.PolicesAccepted;
                    user.UserType = userC.UserType;
                    user.CreatedAt = DateTime.UtcNow;
                    user.PolicesAcceptedAt = DateTime.UtcNow;
                    user.Status = 1;
                    await _repository.CreateAsync<UserTbl>(user);
                    respuesta.Ok = 1;
                    respuesta.Message = "Registro exitoso";
                }
                else
                {
                    //El usuario esta duplicado
                    respuesta.Ok = 0;
                    respuesta.Message = "El correo electrónico ya está registrado";
                    return BadRequest(respuesta);
                }
            }

            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        [HttpGet("GetAllUsers")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll(int HealthCenterFilter, int pagNumber = 1, int pagSize = 10, string emailFilter = null, string? statusFilter = "Ambos")
        {
            Respuesta<object> respuesta = new();
            try
            {

                pagNumber = pagNumber < 1 ? 1 : pagNumber;
                pagSize = pagSize < 1 ? 10 : pagSize;
                int totalActive = await _context.UserTbl.CountAsync(ua => ua.Status == 1);
                int totalInactive = await _context.UserTbl.CountAsync(ua => ua.Status == 0);
                var usersList = await GetUserList(pagNumber, pagSize, emailFilter, statusFilter, HealthCenterFilter);
                int totalItems = await _context.UserTbl.Where(u => u.DeletedAt == null).CountAsync();
                int items = usersList.Count;

                usersList = usersList.Skip((pagNumber - 1) * pagSize).Take(pagSize).ToList();
                if (items > 0)
                {
                    respuesta.Data.Add(new
                    {
                        totalItems = items,
                        totalPages = (int)Math.Ceiling((double)items / pagSize),
                        actualPage = pagNumber,
                        users = usersList,
                        Totaluser = totalItems,
                        TotalActive = totalActive,
                        TotalInactive = totalInactive,
                    });
                    respuesta.Ok = 1;
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Búsqueda sin coincidencias";
                    return Ok(respuesta);
                }
                //respuesta.Message = "Success";
                respuesta.Message = "Búsqueda exitosa";
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }


        [HttpPut("UpdateUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserBd(UserUpdate user, int id)
        {
            Respuesta<object> respuesta = new();
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            try
            {
                if (user.Status != 1 && user.Status != 2)
                {
                    //el ususario no existe o esta eliminado
                    respuesta.Ok = 0;
                    respuesta.Message = "El Estatus ingresado no es válido";
                    return BadRequest(respuesta);
                }
                if (user.UserType == null)
                {
                    //El rol no existe
                    respuesta.Ok = 0;
                    respuesta.Message = "El Rol asignado no exitse";
                    return NotFound(respuesta);
                }
                string validate = validateText(user.FullName);
                if (validate != "validado")
                {
                    //El rol no existe
                    respuesta.Ok = 0;
                    respuesta.Message = validate;
                    return BadRequest(respuesta);
                }
                var dbemailvalidate = await _context.UserTbl.Where(dbu => dbu.Email == user.Email && dbu.Status != 0).FirstOrDefaultAsync();
                if (dbemailvalidate == null && dbemailvalidate.UserID != id)
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "El email ya esta en uso";
                    return BadRequest(respuesta);
                }
                var userdb = await _repository.SelectById<UserTbl>(id);
                if (userdb == null || userdb.DeletedAt != null)
                {
                    //el ususario no existe o esta eliminado
                    respuesta.Ok = 0;
                    respuesta.Message = "Usuario no encontrado";
                    return NotFound(respuesta);
                }
                userdb.FullName = textInfo.ToTitleCase(user.FullName.ToLower());
                userdb.Email = user.Email;
                userdb.UserType = user.UserType;
                userdb.Status = user.Status;
                userdb.Country = user.Country;
                userdb.Password = _encryptor.Encrypt(user.Password);
                userdb.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(userdb);
                respuesta.Ok = 1;
                respuesta.Message = "Usuario Modificado";
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpDelete("DeleteUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUserBd(int id)
        {
            Respuesta<object> respuesta = new();
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            try
            {
                var deleteUser = await _repository.SelectById<UserTbl>(id);
                if (deleteUser != null && deleteUser.DeletedAt == null)
                {
                    deleteUser.DeletedAt = DateTime.Now;
                    deleteUser.Status = 0;
                    await _repository.UpdateAsync(deleteUser);
                    respuesta.Ok = 1;
                    //respuesta.Message = "Success";
                    respuesta.Message = "Usuario Eliminado";
                    }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Usuario no encontrado";
                    }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }



        ////HttpGet("PasswordRecovery")]
        ////public async Task<IActionResult> PasswordRecovery(string email)
        ////{
        ////    Respuesta<object> respuesta = new();
        ////    try
        ////    {
        ////        var user = await _context.UserAccess.Where(u => u.Email == email && u.DeletedAt == null && u.Status == 1).FirstOrDefaultAsync();
        ////        if (user == null)
        ////        {
        ////            respuesta.Ok = 0;
        ////            respuesta.Message = "No se encontro el correo electrónico";
        ////            return NotFound(respuesta);
        ////        }

        ////        user.PasswordRecoveryExpiration = DateTime.Now.AddDays(1);
        ////        var password = GeneratePassword();
        ////        string asunto = "RECUPERACIÓN DE CONTRASEÑA";
        ////        string body = RecoveryPasswordBody(user, password);

        ////        var pw = _encryptor.Encrypt(password);
        ////        user.Password = pw;
        ////        await _repository.UpdateAsync(user);

        ////        var response = await EmailManager.SendEmail(
        ////            this._appsettings["SendGrid:API_KEY"],
        ////            this._appsettings["SendGrid:Email"],
        ////            this._appsettings["SendGrid:Name"],
        ////            email, asunto, body);

        ////        respuesta.Ok = 1;
        ////        respuesta.Message = "Contraseña enviada al correo";
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        respuesta.Ok = 0;
        ////        respuesta.Message = e.Message + " " + e.InnerException;
        ////        return BadRequest(respuesta);
        ////    }
        ////    return Ok(respuesta);
        ////}[

        // Lista de Funciones Privada del controlador
        private SigningCredentials GetSigningCredentials()
        {
            var _key = _appsettings["JWTSettings:SecurityKey"];
            var key = Encoding.UTF8.GetBytes(_key);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private List<Claim> GetClaims(UserTbl user, RolTbl roles, List<PermissionsTbl> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName)
            };
            claims.Add(new Claim(ClaimTypes.Authentication, user.UserID.ToString()));
            if (roles != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.Name));
                if (permissions != null)
                {
                    foreach (PermissionsTbl permision in permissions)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, permision.Name));
                    }
                }
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _appsettings["JWTSettings:validIssuer"],
                audience: _appsettings["JWTSettings:validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_appsettings["JWTSettings:expiryInMinutes"])),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
        private string createToken(UserTbl user, RolTbl rol, List<PermissionsTbl> permissions)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user, rol, permissions);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }
        private string validateText(string firfullname)
        {
            string validate = "validado";
            Regex regex = new Regex("^[a-zA-ZÀ-ÿ\\u00f1\\u00d1]+(\\s*[a-zA-ZÀ-ÿ\\u00f1\\u00d1]*)*[a-zA-ZÀ-ÿ\\u00f1\\u00d1]+$");
            if (!regex.IsMatch(firfullname.TrimEnd().TrimStart()))
            {
                //revisa el formato del rol, nombre y apellido
                validate = "Los campos de nombre y apellido solo pueden tener letras";
            }
            return validate;
        }
        private async Task<List<UserDto>> GetUserList(int pagNumber, int pagSize, string emailFilter, string statusFilter, int HealthCenterFilter)
        {
            try
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                var users = new List<UserDto>();
                if (statusFilter != "Ambos")
                {
                    users = await _context.UserTbl
                                .Where(us => us.Status == Enums.StatusStToInt[statusFilter] && (string.IsNullOrEmpty(emailFilter) || us.Email.Contains(emailFilter)))
                                .Join(_context.UserMedicHCRel.Where(um => um.HealthCenterID == HealthCenterFilter), us => us.UserID, um => um.UserID,
                                (us, um) => new UserDto
                                {
                                    UserID = us.UserID,
                                    FullName = us.FullName,
                                    Email = us.Email,
                                    Country = us.Country,
                                    UserType = us.UserType,
                                    StatusName = Enums.StatusIntToSt[us.Status]
                                })
                                .OrderBy(x => x.UserID)
                                .ToListAsync();
                }
                else
                {
                    users = await _context.UserTbl
                        .Where(us => (string.IsNullOrEmpty(emailFilter) || us.Email.Contains(emailFilter)))
                        .Join(_context.UserMedicHCRel.Where(um => um.HealthCenterID == HealthCenterFilter), us => us.UserID, um => um.UserID,
                        (us, um) => new UserDto
                        {
                            UserID = us.UserID,
                            FullName = us.FullName,
                            Email = us.Email,
                            Country = us.Country,
                            UserType = us.UserType,
                            StatusName = Enums.StatusIntToSt[us.Status]
                        })
                        .OrderBy(x => x.UserID)
                        .ToListAsync();
                }
                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
