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
    public class MedicController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly AppDBContext _context;
        private readonly IConfiguration _appsettings;
        private readonly CMSEncryptor _encryptor;
        private readonly IWebHostEnvironment _env;

        public MedicController(AppDBContext context, IConfiguration appsettings, IRepository repository, IWebHostEnvironment env)
        {
            _context = context;
            _appsettings = appsettings;
            _repository = repository;
            _encryptor = new CMSEncryptor(appsettings);
            _env = env;
        }


        [HttpPost("CreateMedic")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateMedic(MedicDto MedicC, int UserID, int HcID)
        {
            Respuesta<object> respuesta = new();
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            try
            {
                string validate = validateText(MedicC.);
                if (validate != "validado")
                {
                    //El Nombre o apellido no tiene el formato
                    respuesta.Ok = 0;
                    respuesta.Message = validate;
                    return BadRequest(respuesta);
                }

                //algo como buscar el Email en la base AD para luego ver si ese usuario exite y no esta duplicado en la base de datos



                if (await _context.UserTbl.Where(u => u.DeletedAt == null && u.UserID == UserID).FirstOrDefaultAsync() == null)
                {
                   
                        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                        MedicTbl medic = new MedicTbl();
                        // Existe en el AD, No existe usuario registrado y ademas el rol existe en la db

                        medic.License = MedicC.License;
                        medic.Specialization = MedicC.Specialization;
                        medic.Country = MedicC.Country;
                        medic.Degree = MedicC.Degree;
                        medic.PatientsAvg = MedicC.PatientsAvg;
                        medic.PhoneNumber = MedicC.PhoneNumber;
                        medic.WebSite = MedicC.WebSite;
                        medic.MedicTypeType = MedicC.MedicTypeType;
                        medic.Response = MedicC.Response;
                        medic.PoliciesAccepted = MedicC.PoliciesAccepted;

                        medic.CreatedAt = DateTime.UtcNow;
                        medic.PoliciesAcceptedAt = DateTime.UtcNow;
                        medic.Status = 1;
                        var medicID = await _repository.CreateAsyncInt<MedicTbl>(medic);
                        
                        UserMedicHCRel userMedicHC = new UserMedicHCRel();
                        userMedicHC.UserID = UserID;
                        userMedicHC.MedicID = medicID;
                        userMedicHC.HealthCenterID = HcID;
                        userMedicHC.Direction = MedicC.Direction;
                        userMedicHC.RollID = MedicC.RolID;
                        userMedicHC.CreatedAt = DateTime.UtcNow;
                        userMedicHC.Status = 1;
                        await _repository.CreateAsync<UserMedicHCRel>(userMedicHC);
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



        [HttpPut("UpdateMedic")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateMedicBd(MedicDto medic, int id)
        {
            Respuesta<object> respuesta = new();
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            try
            {
                if (medic.Status != 1 && medic.Status != 2)
                {
                    //el ususario no existe o esta eliminado
                    respuesta.Ok = 0;
                    respuesta.Message = "El Estatus ingresado no es válido";
                    return BadRequest(respuesta);
                }
                if (medic.MedicTypeType == null)
                {
                    //El rol no existe
                    respuesta.Ok = 0;
                    respuesta.Message = "El Rol asignado no exitse";
                    return NotFound(respuesta);
                }
                
                var dbemailvalidate = await _context.MedicTbl.Where(dbu => dbu.MedicID == id && dbu.Status != 0).FirstOrDefaultAsync();
                if (dbemailvalidate == null)
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "El Medico no existe";
                    return BadRequest(respuesta);
                }
                var userdb = await _repository.SelectById<MedicTbl>(id);
                if (userdb == null || userdb.DeleteedAt != null)
                {
                    //el ususario no existe o esta eliminado
                    respuesta.Ok = 0;
                    respuesta.Message = "Usuario no encontrado";
                    return NotFound(respuesta);
                };

                if (await _context.UserTbl.Where(u => u.DeletedAt == null && u.UserID == UserID).FirstOrDefaultAsync() == null)
                {

                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                    MedicTbl medic = new MedicTbl();
                    // Existe en el AD, No existe usuario registrado y ademas el rol existe en la db

                    medic.License = MedicC.License;
                    medic.Specialization = MedicC.Specialization;
                    medic.Country = MedicC.Country;
                    medic.Degree = MedicC.Degree;
                    medic.PatientsAvg = MedicC.PatientsAvg;
                    medic.PhoneNumber = MedicC.PhoneNumber;
                    medic.WebSite = MedicC.WebSite;
                    medic.MedicTypeType = MedicC.MedicTypeType;
                    medic.Response = MedicC.Response;
                    medic.PoliciesAccepted = MedicC.PoliciesAccepted;

                    medic.CreatedAt = DateTime.UtcNow;
                    medic.PoliciesAcceptedAt = DateTime.UtcNow;
                    medic.Status = 1;
                    var medicID = await _repository.CreateAsyncInt<MedicTbl>(medic);
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

        [HttpDelete("DeleteMedic")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteMedicBd(int id)
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


    }
}
